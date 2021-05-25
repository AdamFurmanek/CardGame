using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo
{
    private Table table;
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
        card.cardObjectTop.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
        card.cardObjectBottom.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);

        switch (card.area)
        {
            case 0:
                break;
            case 1:
                card.areaParticles.Play();
                card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card effect 1");
                break;
            case 2:
                card.areaParticles.Play();
                card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card put effect");
                break;
            case 3:
                card.deathParticles.Play();
                card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card effect 3");
                card.cardObjectTop.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                card.cardObjectBottom.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                break;
        }
    }

    public virtual void OnHittingOtherCard(GameObject otherCardObject)
    {
        card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card hit");
        Card otherCard = otherCardObject.GetComponent<Card>();

        otherCard.actualHealth -= card.actualStrength;
        if (otherCard.actualHealth <= 0)
            otherCard.cardInfo.OnDie();
        else
            otherCard.hitParticles.Play();
    }

    public virtual void OnDie()
    {
        card.table.ChangeArea(card.gameObject, 3);
    }

}
