using System.Collections;
using System.Collections.Generic;
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
        for(int i = 0; i < 10; i++)
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
                newCardObject.GetComponent<Card>().area = 0;
                newCardObject.GetComponent<Card>().ResetStats();
                newCardObject.GetComponent<Card>().cardInfo.SetPossibleMoves();
                //i - gracz (0-1), 0 - stos
                cards[i][0].Add(newCardObject);
            }
        }

        //Kilka przyk³adowych zmian
        ChangeArea(cards[0][0][0], 1);
        ChangeArea(cards[0][0][0], 2);
        ChangeArea(cards[0][0][0], 3);

        CleanTable();

        turn = 0;
    }

    public void CleanTable()
    {
        for(int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                foreach (GameObject cardObject in cards[i][j])
                {
                    cardObject.transform.position = areas[i + j * 2].transform.position;
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
    }

}
