using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject cardObjectPrefab;
    public GameObject[] areas;

    public List<List<GameObject>>[] cards = new List<List<GameObject>>[2];

    public int turn;

    void Start()
    {
        //Przygotowanie listy kart.
        for (int i = 0; i < 2; i++)
        {
            cards[i] = new List<List<GameObject>>();
            for (int j = 0; j < 4; j++)
            {
                cards[i].Add(new List<GameObject>());
            }
        }

        //Przyk³adowe karty.
        List<CardInfo>[] newCards = { new List<CardInfo>(), new List<CardInfo>() };
        for(int i = 0; i < 20; i++)
        {
            newCards[0].Add(new Warrior());
            newCards[1].Add(new Dragon());
        }

        //Wczytanie kart na stó³ (taka minifabryka)
        for(int i = 0; i < 2; i++)
        {
            foreach (CardInfo card in newCards[i])
            {
                GameObject newCardObject = Instantiate(cardObjectPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                card.cardObject = newCardObject.GetComponent<Card>();
                newCardObject.GetComponent<Card>().table = this;
                newCardObject.GetComponent<Card>().cardInfo = card;
                newCardObject.GetComponent<Card>().originalPlayer = i;
                newCardObject.GetComponent<Card>().ResetStats();
                newCardObject.GetComponent<Card>().cardInfo.SetPossibleMoves();
                ChangeArea(newCardObject, 0);
                //i - gracz (0-1), 0 - stos
            }
        }

        //Kilka przyk³adowych zmian
        ChangeArea(cards[0][0][0], 1);
        ChangeArea(cards[0][0][0], 2);
        for(int i = 0; i < 10; i++)
        {
            ChangeArea(cards[0][0][0], 3);
            ChangeArea(cards[1][0][0], 3);
        }


        CleanTable();

        turn = 0;
    }

    public void CleanTable()
    {

        //Stos
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < cards[i][0].Count; j++)
            {
                GameObject cardObject = cards[i][0][j];
                Card card = cardObject.GetComponent<Card>();
                cardObject.transform.position = areas[i].transform.position
                    + new Vector3(0, 1, 0) * j * 0.0001f
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                cardObject.transform.rotation = Quaternion.Euler(180, card.rotationOffset, 0);
            }
        }

        //Rêka
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < cards[i][1].Count; j++)
            {
                GameObject cardObject = cards[i][1][j];
                Card card = cardObject.GetComponent<Card>();
                float difference = cardObject.transform.lossyScale.x / 2 - ((float)cards[i][1].Count/40);
                Debug.Log(difference);
                float offset = -(cards[i][1].Count - 1) * difference;
                cardObject.transform.position = areas[2 + i].transform.position
                    + new Vector3(1, 0, 0) * (offset + j * difference * 2)
                    + new Vector3(0, 1, 0) * (0.1f + j * 0.0001f)
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                cardObject.transform.rotation = Quaternion.Euler(0, card.rotationOffset, 0);
            }
        }

        //Walka
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < cards[i][2].Count; j++)
            {
                GameObject cardObject = cards[i][2][j];
                Card card = cardObject.GetComponent<Card>();
                float difference = cardObject.transform.lossyScale.x / 2 - ((float)cards[i][2].Count / 40);
                float offset = -(cards[i][2].Count - 1) * difference;
                cardObject.transform.position = areas[4 + i].transform.position
                    + new Vector3(1, 0, 0) * (offset + j * difference * 2)
                    + new Vector3(0, 1, 0) * j * 0.01f
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                cardObject.transform.rotation = Quaternion.Euler(0, card.rotationOffset, 0);
            }
        }

        //Cmentarz
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < cards[i][3].Count; j++)
            {
                GameObject cardObject = cards[i][3][j];
                Card card = cardObject.GetComponent<Card>();
                cardObject.transform.position = areas[6 + i].transform.position
                    + new Vector3(0, 1, 0) * j * 0.01f
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                cardObject.transform.rotation = Quaternion.Euler(0, card.rotationOffset, 0);
            }
        }

        //Napisy na kartach
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                foreach (GameObject cardObject in cards[i][j])
                {
                    Card card = cardObject.GetComponent<Card>();
                    card.healthLabel.GetComponent<TextMeshPro>().text = card.actualHealth + "";
                    card.strengthLabel.GetComponent<TextMeshPro>().text = card.actualStrength + "";
                }
            }
        }

    }

    public void ChangeTurn()
    {
        if (turn == 0)
            turn = 1;
        else if (turn == 1)
            turn = 0;

        for (int i = 0; i < 2; i++)
        {
            foreach (GameObject cardObject in cards[i][2])
            {
                cardObject.GetComponent<Card>().cardInfo.OnReceivingTurn();
            }
        }
    }

    public void ChangeArea(GameObject cardObject, int destination)
    {
        Card card = cardObject.GetComponent<Card>();
        cards[card.actualPlayer][card.area].Remove(cardObject);
        card.area = destination;
        cards[card.actualPlayer][card.area].Add(cardObject);
        card.cardInfo.OnChangingArea();

        switch (destination)
        {
            case 0:
                card.rotationOffset = Random.RandomRange(-5f,5f);
                card.positionXOffset = Random.RandomRange(-0.1f, 0.1f);
                card.positionZOffset = Random.RandomRange(-0.1f, 0.1f);
                break;
            case 1:
                card.rotationOffset = Random.RandomRange(-4f, 4f);
                card.positionXOffset = Random.RandomRange(-0.1f, 0.1f);
                card.positionZOffset = Random.RandomRange(-0.05f, 0.05f);
                break;
            case 2:
                card.rotationOffset = Random.RandomRange(-3f, 3f);
                card.positionXOffset = Random.RandomRange(-0.1f, 0.1f);
                card.positionZOffset = Random.RandomRange(-0.05f, 0.05f);
                break;
            case 3:
                card.rotationOffset = Random.RandomRange(-15f, 15f);
                card.positionXOffset = Random.RandomRange(-0.8f, 0.8f);
                card.positionZOffset = Random.RandomRange(-0.8f, 0.8f);
                break;
        }
    }

}
