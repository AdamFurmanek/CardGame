using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : CardInfo
{
    public Warrior()
    {
        health = 5;
        strength = 2;
    }

    //Przyk³adowy override metody: regeneracja 1pkt zdrowia przy odzyskaniu tury.
    public override void OnReceivingTurn()
    {
        Debug.Log("Nowa metoda tury");
        if (cardObject.actualHealth < health)
            cardObject.actualHealth++;
    }
}
