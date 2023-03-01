using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject card;
    private Transform deck, playerCards, AICards;


    private List<GameObject> cardsList;
    private List<GameObject> playerList;
    private List<GameObject> aiList;

    private int totalCards = 60;

    int handSize = 3;

    // Start is called before the first frame update
    void Start()
    {
        deck = GameObject.Find("spawnsite").transform;
        playerCards = GameObject.Find("Player One").transform;
        AICards = GameObject.Find("AI player").transform;

        

        CreateCardsSequence();//card gen sequence

        //organize the cards into their right spots
        serveCards();
    }

    void CreateCardsSequence()
    {
        int rockRemaining = 20;
        int paperRemaining = 20;
        int scissorsRemaining = 20;


        //spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            //ideally choose random card to pick
            //rock = 1, scissors = 2, paper = 3
            

            int rNum = Random.Range(1, 3);
            Debug.Log(i);
            if (rNum == 1)
            {
                if (rockRemaining > 0)
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("rock");
                    cardsList.Add(cardObject);
                    rockRemaining--;
                }
                else if (scissorsRemaining > 0)
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("scissors");
                    cardsList.Add(cardObject);
                    scissorsRemaining--;
                }
                else
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("paper");
                    cardsList.Add(cardObject);
                    paperRemaining--;
                }


            }
            else if (rNum == 2)
            {
                if (scissorsRemaining > 0)
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("scissors");
                    cardsList.Add(cardObject);
                    scissorsRemaining--;
                }
                else if(paperRemaining > 0)
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("paper");
                    cardsList.Add(cardObject);
                    paperRemaining--;
                }               
                else
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("rock");
                    cardsList.Add(cardObject);
                    rockRemaining--;
                }
            }
            else
            {
                if (paperRemaining > 0)
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("paper");
                    cardsList.Add(cardObject);
                    paperRemaining--;
                }
                else if (rockRemaining > 0)
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("rock");
                    cardsList.Add(cardObject);
                    rockRemaining--;
                }
                else
                {
                    GameObject cardObject = Instantiate(card, deck.position, Quaternion.identity);
                    Card cScript = cardObject.GetComponent<Card>();
                    cScript.setType("scissors");
                    cardsList.Add(cardObject);
                    scissorsRemaining--;
                }
            }


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
            cardObject.transform.position = new Vector3(AICards.position.x - cScript.getWidth() * i + 20, AICards.position.y,AICards.position.z);

        }


    }

}
