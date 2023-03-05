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

    public Sprite faceSprite, backSprite;

    string cardType = "";

    bool flipped = false;

    SpriteRenderer sr;
    GameManager gm;


    bool chooseable = false;
    


    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        sr.sprite = backSprite;
    }

    public void setType(string type)
    {
        cardType = type;
        transform.name = type;

        if (type == s)
        {
            faceSprite = s_Scissors;

        }
        else if(type == r)
        {
            faceSprite = s_Rock;

        }
        else if(type == p)
        {
            faceSprite = s_Paper;
        }


    }

    public void setFlipped(bool val)
    {
        flipped = val;

        //if flipped, turn to face card, if not then turn to backside
        if (val)
        {
            sr.sprite = faceSprite;
        }
        else
        {
            sr.sprite = backSprite;
        }
    }


    public int getWidth()
    {
        return (int) sr.size[0];
    }

    public void setChooseable(bool val)
    {
        chooseable = val;
        //Debug.Log("Set as chooseable");
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

}
