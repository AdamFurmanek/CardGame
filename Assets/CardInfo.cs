using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo
{
    public Card card;
    public int strength, health;
    public string name;

    public List<string> possibleMoves = new List<string>(); //ABCD:  A - from? (0-2) B - where? (0-3) C - whose? (m/o) D - card/area(c/a)

    public virtual void SetPossibleMoves()
    {
        possibleMoves.Add("01ma");
        possibleMoves.Add("12ma");
        possibleMoves.Add("22oc");
    }

    public virtual void OnReceivingTurn()
    {

    }

    public virtual void OnChangingArea()
    {
        card.areaParticles.Play();
    }

    public virtual void OnHittingOtherCard(GameObject otherCardObject)
    {
        Card otherCard = otherCardObject.GetComponent<Card>();
        otherCard.hitParticles.Play();
        otherCard.actualHealth -= card.actualStrength;
        otherCard.hitParticles.Play();
        if (otherCard.actualHealth <= 0)
        {
            otherCard.cardInfo.OnDie();
            otherCard.deathParticles.Play();
        }
    }

    public virtual void OnDie()
    {
        card.table.ChangeArea(card.gameObject, 3);
    }

}
