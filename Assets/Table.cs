using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject cardObjectPrefab;
    public GameObject[] areas;
    public GameObject soundsPlayer;

    public List<List<GameObject>>[] cards = new List<List<GameObject>>[2];

    public int turn;
    public bool dragging = false;

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
            foreach (CardInfo cardInfo in newCards[i])
            {
                GameObject newCardObject = Instantiate(cardObjectPrefab, new Vector3(13, 0, Random.RandomRange(-10, 10)), new Quaternion(0, 0, 0, 0));
                
                cardInfo.card = newCardObject.GetComponent<Card>();
                cardInfo.card.table = this;
                cardInfo.card.cardInfo = cardInfo;
                cardInfo.card.originalPlayer = i;
                cardInfo.card.ResetStats();
                cardInfo.SetPossibleMoves();
                cardInfo.card.nameLabel.GetComponent<TextMeshPro>().text = cardInfo.name;
                ChangeArea(newCardObject, 0);
            }
        }

        CleanTable();

        turn = 0;
    }

    public void CleanTable()
    {
        foreach(GameObject area in areas)
            area.GetComponent<MeshRenderer>().enabled = false;

        foreach(var list in cards)
            foreach(var list2 in list)
                foreach(var card in list2)
                    card.GetComponent<Card>().cardObjectCover.SetActive(false);

        StopAllCoroutines();
        //Stos
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < cards[i][0].Count; j++)
            {
                GameObject cardObject = cards[i][0][j];
                Card card = cardObject.GetComponent<Card>();
                card.destinationPosition = areas[i].transform.position
                    + new Vector3(0, 1, 0) * j * 0.0001f
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                StartCoroutine(card.Move());
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
                float difference = 1.65f / 2 - ((float)cards[i][1].Count/40);
                Debug.Log(difference);
                float offset = -(cards[i][1].Count - 1) * difference;
                card.destinationPosition = areas[2 + i].transform.position
                    + new Vector3(1, 0, 0) * (offset + j * difference * 2)
                    + new Vector3(0, 1, 0) * (2f + j * 0.0001f)
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                StartCoroutine(card.Move());
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
                float difference = 1.65f / 2 - ((float)cards[i][2].Count / 40);
                float offset = -(cards[i][2].Count - 1) * difference;
                card.destinationPosition = areas[4 + i].transform.position
                    + new Vector3(1, 0, 0) * (offset + j * difference * 2)
                    + new Vector3(0, 1, 0) * j * 0.0001f
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);
                StartCoroutine(card.Move());
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
                card.destinationPosition = areas[6 + i].transform.position
                    + new Vector3(0, 1, 0) * j * 0.0001f
                    + new Vector3(card.positionXOffset, 0, card.positionZOffset);

                StartCoroutine(card.Move());
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
