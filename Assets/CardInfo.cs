using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo
{
    public Card cardObject;
    public int strength, health;

    public virtual void OnReceivingTurn()
    {
        Debug.Log("Podstawowa metoda tury");
    }
}
