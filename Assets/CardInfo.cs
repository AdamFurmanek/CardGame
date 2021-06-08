using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo
{
    public Card card;
    public int strength, health;
    public string name;

    public Dictionary<string, Action> areaMoves = new Dictionary<string, Action>();
    public Dictionary<string, Action<GameObject>> cardMoves = new Dictionary<string, Action<GameObject>>(); //FROM, TO, WHOSE

    public CardInfo()
    {
        SetPossibleMoves();
    }

    public virtual void SetPossibleMoves()
    {
        areaMoves.Add("01m", FromStackToHand);
        areaMoves.Add("12m", FromHandToTable);
        cardMoves.Add("22o", HitOtherCard);
    }

    public virtual void OnReceivingTurn()
    {

    }

    public void FromStackToHand()
    {
        card.areaParticles.Play();
        card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card effect 1");
        card.cardObjectTop.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
        card.cardObjectBottom.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void FromHandToTable()
    {
        card.areaParticles.Play();
        card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card put effect");
        card.cardObjectTop.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
        card.cardObjectBottom.GetComponent<Renderer>().material.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void HitOtherCard(GameObject otherCardObject)
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
        card.deathParticles.Play();
        card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("card effect 3");
        card.cardObjectTop.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        card.cardObjectBottom.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
    }
}
