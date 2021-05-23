using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : CardInfo
{
    public Warrior() : base()
    {
        health = 5;
        strength = 2;
        name = "Warrior";
    }

    //Przyk³adowy override metody: regeneracja 1pkt zdrowia przy odzyskaniu tury.
    public override void OnReceivingTurn()
    {
        //Debug.Log("Nowa metoda tury");
        if (card.actualHealth < health)
            card.actualHealth++;
    }
}
