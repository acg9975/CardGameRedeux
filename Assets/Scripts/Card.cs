using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //QOL handles
    private string s = "Scissors";
    private string p = "Paper";
    private string r = "Rock";

    public Sprite s_Scissors, s_Paper, s_Rock;


    string cardType = "";

    bool flipped = false;

    SpriteRenderer sr;
    GameManager gm;


    bool chooseable = false;
    


    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void setType(string type)
    {
        cardType = type;
        transform.name = type;

        if (type == s)
        {
            sr.color = Color.blue;
        }
        else if(type == r)
        {
            sr.color = Color.red;
        }
        else if(type == p)
        {
            sr.color = Color.green;
        }

    }

    public void setFlipped(bool val)
    {
        flipped = val;
    }


    public int getWidth()
    {
        return (int) sr.size[0];
    }

    public void setChooseable(bool val)
    {
        chooseable = val;
    }

    public string getCardType()
    {
        return cardType;
    }

    private void OnMouseDown()
    {
        if (chooseable)
        {
            gameObject.transform.position = GameObject.Find("Player Standoff").transform.position;
            //remove ability to choose from player
            gm.getPlayerCardChosen(gameObject);

            Debug.Log("Player chose a card of type: " + cardType);

        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Entered card:" + cardType);
    }



}
