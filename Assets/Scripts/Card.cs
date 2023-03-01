using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

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
