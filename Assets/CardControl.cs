using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardControl : MonoBehaviour
{
    Camera mainCamera;
    Card card;
    bool debugBothPlayers;
    bool thisDragging;
    static bool anyDragging;

    private void Awake()
    {
        mainCamera = Camera.main;
        card = gameObject.GetComponent<Card>();
        debugBothPlayers = true;
        thisDragging = false;
        anyDragging = false;
    }

    public void OnMouseEnter()
    {
        if (!anyDragging)
        {
            gameObject.transform.localScale = new Vector3(1.65f * 1.1f, 0.0001f, 2.55f * 1.1f);
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
            card.table.soundsPlayer.GetComponent<SoundsPlayer>().PlaySound("hover effect");
        }
    }

    private void OnMouseDown()
    {
        if (card.actualPlayer == card.table.turn || debugBothPlayers)
        {
            gameObject.transform.localScale = new Vector3(1.65f, 0.0001f, 2.55f);
            thisDragging = true;
            anyDragging = true;
        }
    }

    private void Update()
    {
        if (thisDragging)
        {
            foreach (GameObject area in card.table.areas)
                area.GetComponent<MeshRenderer>().enabled = false;

            foreach (var list in card.table.cards)
                foreach (var list2 in list)
                    foreach (var card in list2)
                        card.GetComponent<Card>().cardObjectCover.SetActive(false);

            if (card.actualPlayer == card.table.turn || debugBothPlayers)
            {
                //https://gist.github.com/SimonDarksideJ/477f5674285b63cba8e752c43950ed7c
                Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
                Vector3 PO = transform.position;
                Vector3 PN = -mainCamera.transform.forward;
                float t = Vector3.Dot(PO - r.origin, PN) / Vector3.Dot(r.direction, PN);
                transform.position = r.origin + r.direction * t;
                transform.position = new Vector3(transform.position.x, 3, transform.position.z);

                List<RaycastHit> otherObjects = new List<RaycastHit>();
                otherObjects.AddRange(Physics.RaycastAll(r, Mathf.Infinity));
                var cards = otherObjects.Where(s => s.transform.gameObject.tag == "Card").ToList();
                var areas = otherObjects.Where(s => s.transform.gameObject.tag == "Area").ToList();

                bool usedOnCard = false;
                if (cards.Count > 1)
                {
                    Card otherCard = cards[1].transform.gameObject.GetComponent<Card>();
                    string move = "";
                    move += card.area;
                    move += otherCard.area;
                    move += card.actualPlayer == otherCard.actualPlayer ? "m" : "o";
                    if (card.cardInfo.cardMoves.ContainsKey(move))
                    {
                        usedOnCard = true;
                        otherCard.gameObject.GetComponent<Card>().cardObjectCover.SetActive(true);
                    }
                }
                if (!usedOnCard && areas.Count > 0)
                {
                    Area otherArea = areas[0].transform.gameObject.GetComponent<Area>();
                    string move = "";
                    move += card.area;
                    move += otherArea.area;
                    move += card.actualPlayer == otherArea.player ? "m" : "o";
                    if (card.cardInfo.areaMoves.ContainsKey(move))
                    {
                        otherArea.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if (card.actualPlayer == card.table.turn || debugBothPlayers)
        {
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
            List<RaycastHit> otherObjects = new List<RaycastHit>();
            otherObjects.AddRange(Physics.RaycastAll(r, Mathf.Infinity));
            var cards = otherObjects.Where(s => s.transform.gameObject.tag == "Card").ToList();
            var areas = otherObjects.Where(s => s.transform.gameObject.tag == "Area").ToList();

            bool usedOnCard = false;
            if (cards.Count > 1)
            {
                Card otherCard = cards[1].transform.gameObject.GetComponent<Card>();
                string move = "";
                move += card.area;
                move += otherCard.area;
                move += card.actualPlayer == otherCard.actualPlayer ? "m" : "o";
                if (card.cardInfo.cardMoves.ContainsKey(move))
                {
                    usedOnCard = true;
                    card.cardInfo.cardMoves[move].Invoke(otherCard.gameObject);
                }
            }
            if(!usedOnCard && areas.Count > 0)
            {
                Area otherArea = areas[0].transform.gameObject.GetComponent<Area>();
                string move = "";
                move += card.area;
                move += otherArea.area;
                move += card.actualPlayer == otherArea.player ? "m" : "o";
                if (card.cardInfo.areaMoves.ContainsKey(move))
                {
                    this.gameObject.GetComponent<Card>().table.ChangeArea(this.gameObject, otherArea.area);
                    card.cardInfo.areaMoves[move].Invoke();
                }
            }

            thisDragging = false;
            anyDragging = false;
            this.gameObject.GetComponent<Card>().table.CleanTable();
        }
    }

    private void OnMouseExit()
    {
        if (!anyDragging)
        {
            card.table.CleanTable();
        }
    }

}
