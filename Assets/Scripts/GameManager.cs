using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject card;

    [SerializeField]
    private Transform deck, playerCards, AICards;

    //List has to be public for interaction
    public List<GameObject> cardsList;
    public List<GameObject> playerList;
    public List<GameObject> aiList;

    [SerializeField][Tooltip("This has to be a multiple of 3")]
    private int totalCards = 60;
    private int rockRemaining = 20;
    private int paperRemaining = 20;
    private int scissorsRemaining = 20;

    private string s = "Scissors";
    private string p = "Paper";
    private string r = "Rock";

    int handSize = 3;

    private Transform prevCard;
    [SerializeField]
    float padding = 5;

    // Start is called before the first frame update
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
        //serveCards();
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
        if (prevCard == null)
        {
            GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
            Card cScript = cardObject.GetComponent<Card>();
            cScript.setType(type);
            cardsList.Add(cardObject);
            
            cardObject.transform.parent = deck;

            prevCard = cardObject.transform;
        }
        else
        {
            float newY = prevCard.position.y + padding;
            Vector3 cardPos = new Vector3(deck.position.x,newY, deck.position.z);

            GameObject cardObject = Instantiate(card, cardPos, Quaternion.identity);
            Card cScript = cardObject.GetComponent<Card>();
            cScript.setType(type);
            cardsList.Add(cardObject);

            cardObject.transform.parent = deck;
            
            prevCard = cardObject.transform;
        }


        //handles the object counting
        if (type == r)
        {
            rockRemaining--;
            Debug.Log("rock remaining: " + rockRemaining);

        }
        else if (type == p)
        {
            paperRemaining--;
            Debug.Log("Papers remaining: " + paperRemaining);

        }
        else
        {
            scissorsRemaining--;
            Debug.Log("Scissors remaining: " + scissorsRemaining);
        }

    }

    private void serveCards()
    {
        //take top 3 cards, give to AI, take next 3 and give to player

        for (int i = 0; i < handSize; i++)
        {
            int lastnum = cardsList.Count;
            GameObject cardObject = cardsList[lastnum - 1];
            aiList.Add(cardObject);
            Card cScript = cardObject.GetComponent<Card>();
            float cardPosX = AICards.position.x + (cScript.getWidth() * i) + 20;
            cardObject.transform.position = new Vector3(cardPosX, AICards.position.y,AICards.position.z);

        }


    }

}
