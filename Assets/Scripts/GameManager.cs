using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject card;

    [SerializeField]
    private Transform deck, playerCardsTransform, AICards;

    [SerializeField]
    private Transform garbagePile;

    //List has to be public for interaction
    public List<GameObject> cardsList;
    public List<GameObject> playerList;
    public List<GameObject> aiList;
    public List<GameObject> garbageList;

    [SerializeField][Tooltip("This has to be a multiple of 3")]
    private int totalCards = 60;
    private int rockRemaining = 20;
    private int paperRemaining = 20;
    private int scissorsRemaining = 20;

    private string s = "Scissors";
    private string p = "Paper";
    private string r = "Rock";

    Transform prevCard = null;

    int handSize = 3;

    private Transform deckPrevCard;
    [SerializeField][Tooltip("Used in positioning the cards in a deck")]
    float padding = 5;

    //[SerializeField][Tooltip("Used in positioning the card's x and y in a deck")]
    //private float lrPadding = 0.01f;

    //phases: 0 - create cards 
    //phase 1: serve cards to both players
    //phase 2: AI chooses random card - it rises slightly
    //phase 3: allow player to choose card - this is where the normal game begins after start sequence
    //phase 4: check winner - do pause and add points
    //phase 5: move cards in both lists to trash
    //phase 6: if deck is empty move everything from trash pile to the deck again
    //Restart from phase 1


    [SerializeField]
    private float dramaticPauseTime = 2f;

    //private bool allowedToClick = false;

    private GameObject PlayerShowdownCard;
    private GameObject AIShowdownCard;

    private int playerPoints = 0;
    private int AIPoints = 0;

    public TextMeshProUGUI pScoreText;
    public TextMeshProUGUI aiScoreText;

    public Transform aiShowdownTransform;

    void Start()
    {
        rockRemaining = totalCards / 3;
        paperRemaining = totalCards / 3;
        scissorsRemaining = totalCards / 3;

        /*
        deck = GameObject.Find("spawnsite").transform;
        playerCards = GameObject.Find("Player One").transform;
        AICards = GameObject.Find("AI player").transform;
        */
        

        CreateCardsSequence();//card gen sequence

        //organize the cards into their right spots
        serveCards();
    }

    void CreateCardsSequence()
    {
        //spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            //ideally choose random card to pick
            //rock = 1, scissors = 2, paper = 3
            

            int rNum = Random.Range(1, 4);//the int version of random range is minInclusive and maxExclusive


            //Debug.Log("Random num" + i);
            //consider using switch case 

            if (rNum == 1)
            {
                if (rockRemaining > 0)
                {
                    createCard(r);
                }
                else if (scissorsRemaining > 0)
                {
                    createCard(s);
                }
                else
                {
                    createCard(p);
                }
            }
            else if (rNum == 2)
            {
                if (scissorsRemaining > 0)
                {
                    createCard(s);
                }
                else if(paperRemaining > 0)
                {
                    createCard(p);
                }
                else
                {
                    createCard(r);
                }
            }
            else
            {
                if (paperRemaining > 0)
                {
                    createCard(p);
                }
                else if (rockRemaining > 0)
                {
                    createCard(r);
                }
                else
                {
                    createCard(s);
                }
            }
        }
    }

    private void createCard(string type)
    {
        //we need to instantiate at the previous card's location
        if (deckPrevCard == null)
        {
            GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
            Card cScript = cardObject.GetComponent<Card>();
            cScript.setType(type);
            cardsList.Add(cardObject);
            
            cardObject.transform.parent = deck;

            deckPrevCard = cardObject.transform;
        }
        else
        {
            float newY = deckPrevCard.position.y + padding;
            Vector3 cardPos = new Vector3(deck.position.x,newY, deck.position.z);

            GameObject cardObject = Instantiate(card, cardPos, Quaternion.identity);
            Card cScript = cardObject.GetComponent<Card>();
            cScript.setType(type);
            cardsList.Add(cardObject);

            cardObject.transform.parent = deck;
            
            deckPrevCard = cardObject.transform;
        }


        //handles the object counting
        if (type == r)
        {
            rockRemaining--;
            //Debug.Log("rock remaining: " + rockRemaining);
        }
        else if (type == p)
        {
            paperRemaining--;
            //Debug.Log("Papers remaining: " + paperRemaining);
        }
        else
        {
            scissorsRemaining--;
            //Debug.Log("Scissors remaining: " + scissorsRemaining);
        }

    }

    private void serveCards()
    {
        //take top 3 cards, give to AI, take next 3 and give to player

        //can use a coroutine for this 
        for (int i = 1; i <= handSize; i++)
        {
            int lastnum = cardsList.Count;
            GameObject cardObject = cardsList[lastnum - i];

            Card cScript = cardObject.GetComponent<Card>();

            //essentaily want to position objects like this - at the parent position, with the sprite's width (allowing for the determination of the amount of cards) and some padding between them
            float cardPosX = AICards.position.x + (i * cScript.getWidth()) + padding;
            cardObject.transform.position = new Vector3(cardPosX, AICards.position.y,AICards.position.z);

            //change parent
            cardObject.transform.parent = AICards;
            //Debug.Log(i);
            aiList.Add(cardObject);
            cardsList.Remove(cardObject);
        }
        
        for (int i = 1; i <= handSize; i++)
        {
            int lastnum = cardsList.Count;
            GameObject cardObject = cardsList[lastnum - i];
            playerList.Add(cardObject);
            cardsList.Remove(cardObject);

            Card cScript = cardObject.GetComponent<Card>();

            //essentaily want to position objects like this - at the parent position, with the sprite's width (allowing for the determination of the amount of cards) and some padding between them
            float cardPosX = playerCardsTransform.position.x + (i * cScript.getWidth()) + padding;
            cardObject.transform.position = new Vector3(cardPosX, playerCardsTransform.position.y, playerCardsTransform.position.z);

            //change parent
            cardObject.transform.parent = playerCardsTransform;

        }

        //after both players have their cards in hand, let AI choose 
        AIChoose();

    }

    private void AIChoose()
    {
        //get the aiList and choose a random item. Then move that card down. move to next phase
        int rNum = Random.Range(1,4);

        GameObject aiChosenCard = aiList[rNum - 1];
        aiChosenCard.transform.position = new Vector3(aiChosenCard.transform.position.x, aiChosenCard.transform.position.y - 2, aiChosenCard.transform.position.z);

        AIShowdownCard = aiChosenCard;
        playerChoose();
    }

    private void playerChoose()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Card cardInHand = playerList[i].gameObject.GetComponent<Card>();
            cardInHand.setFlipped(true);
            cardInHand.setChooseable(true);
            //Debug.Log(cardInHand + " player list found");
        }

        //We go through a list of player's cards, then we allow them to be chosen and flipped in the card script


        //have that card slightly raise up
        //check what card the player clicks on 
        

    }



    public int getHandSize()
    {
        return handSize;
    }

    public void getPlayerCardChosen(GameObject cardChosen)
    {
        //set card to the player's chosen card
        PlayerShowdownCard = cardChosen;

        //set chooseable to false for each card
        for (int i = 0; i < playerList.Count; i++)
        {
            Card cardInHand = playerList[i].gameObject.GetComponent<Card>();
            cardInHand.setChooseable(false);
        }

        //once this has been called, the player's card is chosen so move on to next step
        StartCoroutine(checkWinnerPhaseOnePause());

    }

    IEnumerator checkWinnerPhaseOnePause()
    {
        yield return new WaitForSeconds(1f);
        checkWinnerPhaseOne();
    }

    private void checkWinnerPhaseOne()
    {
        //move AI card forward
        AIShowdownCard.transform.position = aiShowdownTransform.position;

        //do dramatic pause
        StartCoroutine(pause());
    }

    private IEnumerator pause()
    {

        yield return new WaitForSeconds(dramaticPauseTime);
        checkWinnderPhaseTwo();

    }

    private void checkWinnderPhaseTwo()
    {
        Debug.Log("Check winner phase 2");

        //flip the AI card
        AIShowdownCard.gameObject.GetComponent<Card>().setFlipped(true);

        //THIS SECTION for testing
        /*
        for (int i = 0; i < aiList.Count; i++)
        {
            aiList[i].transform.name = playerList[i].transform.name + i ;

        }
        */


        //compare values of both showdown cards
        //reward points
        string pCard = PlayerShowdownCard.gameObject.GetComponent<Card>().getCardType();
        string aiCard = AIShowdownCard.gameObject.GetComponent<Card>().getCardType();

        AIShowdownCard.gameObject.GetComponent<Card>().setFlipped(true);

        Debug.Log("Cards flipped");
        if (pCard == "Rock")
        {
            if (aiCard == "Rock")
            {
                Debug.Log(aiCard);
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
            else if (aiCard == "Paper")
            {
                Debug.Log(aiCard);
                AIPoints++;
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
            else if (aiCard == "Scissors")
            {
                Debug.Log(aiCard);
                playerPoints++;
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }

        }
        else if (pCard == "Paper")
        {
            if (aiCard == "Rock")
            {
                Debug.Log(aiCard);
                AIPoints++;
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
            else if (aiCard == "Paper")
            {
                Debug.Log(aiCard);
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
            else if (aiCard == "Scissors")
            {
                Debug.Log(aiCard);
                playerPoints++;
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
        }
        else if (pCard == "Scissors")
        {
            if (aiCard == "Rock")
            {
                Debug.Log(aiCard);
                AIPoints++;
                //resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
            else if (aiCard == "Paper")
            {
                Debug.Log(aiCard);
                playerPoints++;
                //resetPhases();

                StartCoroutine(dramaticPauseReset());
            }
            else if (aiCard == "Scissors")
            {
                Debug.Log(aiCard);
                resetPhases();
                StartCoroutine(dramaticPauseReset());
            }
        }
    }

    IEnumerator dramaticPauseReset()
    {


        yield return new WaitForSeconds(1f);
        resetPhases();

        Debug.Log("dramatic pause reset");
    }

    void resetPhases()
    {
        //add points
        aiScoreText.text = AIPoints.ToString();
        pScoreText.text = playerPoints.ToString();


        //reset showdown cards 

        //Debug.Log(AIShowdownCard.transform.name  + " AI Showdown Card anme");
        //Debug.Log(PlayerShowdownCard.transform.name + " player Showdown Card anme");

        AIShowdownCard = null;
        PlayerShowdownCard = null;

        //Move cards to garbage deck at position similar to regular deck
        //if garbage list is empty, then its equal to null. if it is not, then get the thing off the top
        //Transform prevCard = null;


        if (garbageList.Count == 0)
        {
            prevCard = null;
            Debug.Log("Prev card is null");
        }
        else
        {
            //this may cause problems
            if (garbageList.Count > 1)
            {
                prevCard = garbageList[garbageList.Count - 1].transform;
                Debug.Log("Prev Card is " + prevCard.transform.name);
            }
        }


        for (int i = 1; i <= aiList.Count; i++)
        {
            int lastnum = aiList.Count;
            GameObject currentCard = aiList[lastnum - i];
            currentCard.GetComponent<Card>().setFlipped(true);

            currentCard.transform.parent = garbagePile;

            if (prevCard == null)
            {
                currentCard.transform.position = garbagePile.position;
                prevCard = currentCard.transform;
            }
            else
            {
                float newPosY = prevCard.position.y + padding;
                currentCard.transform.position = new Vector3(garbagePile.position.x, newPosY, garbagePile.position.z);
                prevCard = currentCard.transform;
            }
        }

        for (int i = 1; i <= playerList.Count; i++)
        {
            int lastnum = playerList.Count;

            GameObject currentCard = playerList[lastnum - i];

            currentCard.transform.parent = garbagePile;

            if (prevCard == null)
            {
                currentCard.transform.position = garbagePile.position;
                prevCard = currentCard.transform;
            }
            else
            {
                float newPosY = prevCard.position.y + padding;
                currentCard.transform.position = new Vector3(garbagePile.position.x, newPosY, garbagePile.position.z);
                prevCard = currentCard.transform;
            }

        }

        //remove cards in this for loop
        for (int i = 1; i <= playerList.Count; i++)
        {
            int lastnum = playerList.Count;
            GameObject currentCard = playerList[lastnum - i];
            playerList.Remove(currentCard);
            garbageList.Add(currentCard);
        }
        Debug.Log(aiList.Count + " AI LIST COUNT");

        for (int i = 1; i <= aiList.Count; i++)
        {
            int lastnum = aiList.Count;
            GameObject currentCard = aiList[lastnum - i];
            aiList.Remove(currentCard);
            garbageList.Add(currentCard);
            //Debug.Log("Current Card AI Removed" + (lastnum - i));
        }

        //check the amount of cards left in the deck
        //if there are less than 6 cards then move the cards from the garbage pile to the regular deck
        if (cardsList.Count < 6)
        {
            Transform prevDeckCard = null;
            for (int i = 0; i < garbageList.Count-1; i++)
            {
                GameObject cCard = garbageList[i];
                cardsList.Add(cCard);
                garbageList.Remove(cCard);
                cCard.transform.parent = deck;

                if (prevDeckCard == null)
                {
                    prevDeckCard = cCard.transform;
                    cCard.transform.position = deck.position;
                    
                }
                else
                {
                    float newPosY = prevDeckCard.position.y + padding;
                    cCard.transform.position = new Vector3(deck.position.x, newPosY, deck.position.z);
                    prevDeckCard = cCard.transform;
                }
            }
        }

        //serve cards
        serveCards();
    }

}
