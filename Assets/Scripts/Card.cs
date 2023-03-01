using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //QOL handles
    private string s = "Scissors";
    private string p = "Paper";
    private string r = "Rock";


    string cardType = "";

    bool flipped = false;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
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

}
