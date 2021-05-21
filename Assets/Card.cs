using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Table table;
    public CardInfo cardInfo;
    public int actualHealth, actualStrength;
    public int originalPlayer, actualPlayer;
    public int area;


    public void ResetStats()
    {
        actualHealth = cardInfo.health;
        actualStrength = cardInfo.strength;
        actualPlayer = originalPlayer;
    }

}
