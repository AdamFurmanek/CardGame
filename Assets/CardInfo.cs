using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo
{
    public Card cardObject;
    public int strength, health;

    public List<string> possibleMoves = new List<string>(); //ABCD:  A - from? (0-2) B - where? (0-3) C - whose? (m/o) D - card/area(c/a)

    public virtual void SetPossibleMoves()
    {
        possibleMoves.Add("01ma");
        possibleMoves.Add("12ma");
        possibleMoves.Add("22oc");
    }

    public virtual void OnReceivingTurn()
    {
        //Debug.Log("Podstawowa metoda tury");
    }

    public virtual void OnChangingArea()
    {

    }

    public virtual void OnHittingOtherCard(GameObject otherCard)
    {

    }

}
