using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardControl : MonoBehaviour
{
    Camera mainCamera;
    Card card;
    bool debugBothPlayers = true;
    bool dragging = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        card = gameObject.GetComponent<Card>();
    }

    private void Update()
    {
        if (dragging)
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
                Ray r = mainCamera.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
                Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
                Vector3 PN = -mainCamera.transform.forward; // Take current negative camera's forward as Plane's Normal
                float t = Vector3.Dot(PO - r.origin, PN) / Vector3.Dot(r.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
                Vector3 P = r.origin + r.direction * t; // Find the new point.

                transform.position = P;

                transform.position = new Vector3(transform.position.x, 3, transform.position.z);

                //TODO: Dymki mówi¹ce nad czym jest karta

                GameObject otherObject = CheckActionPossibility(r, true);
                if (otherObject != null)
                {
                    //Debug.Log("Karta");
                    otherObject.GetComponent<Card>().cardObjectCover.SetActive(true);
                }
                else
                {
                    otherObject = CheckActionPossibility(r, false);
                    if (otherObject != null)
                    {
                        //Debug.Log("Obszar");
                        otherObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    public void OnMouseEnter()
    {
        if (!card.table.dragging)
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
            dragging = true;
            card.table.dragging = true;
        }
    }

    private void OnMouseUp()
    {
        if (card.actualPlayer == card.table.turn || debugBothPlayers)
        {
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
            GameObject otherObject = CheckActionPossibility(r, true);
            if (otherObject != null)
            {
                card.cardInfo.OnHittingOtherCard(otherObject);
            }
            else
            {
                otherObject = CheckActionPossibility(r, false);
                if (otherObject != null)
                {
                    this.gameObject.GetComponent<Card>().table.ChangeArea(this.gameObject, otherObject.GetComponent<Area>().area);
                }

            }
            dragging = false;
            card.table.dragging = false;
            this.gameObject.GetComponent<Card>().table.CleanTable();
        }
    }

    private void OnMouseExit()
    {
        if (!card.table.dragging)
        {
            card.table.CleanTable();
        }
    }

    public GameObject CheckActionPossibility(Ray r, bool cardOrArea)
    {
        List<RaycastHit> otherObjects = new List<RaycastHit>();
        otherObjects.AddRange(Physics.RaycastAll(r, Mathf.Infinity));

        for (int i = 0; i < otherObjects.Count; i++)
        {
            if (otherObjects[i].collider.gameObject == this.gameObject)
            {
                otherObjects.RemoveAt(i);
                break;
            }
        }

        if (otherObjects.Count > 0)
        {
            GameObject otherObject = null;
            for (int i = 0; i < otherObjects.Count; i++)
            {
                if (otherObjects[i].transform.gameObject.tag.CompareTo(cardOrArea ? "Card" : "Area") == 0)
                {
                    otherObject = otherObjects[i].transform.gameObject;
                    break;
                }
            }

            if (otherObject != null)
            {
                if (cardOrArea) {
                    if (CheckCardActionPossibility(this.gameObject, otherObject))
                        return otherObject;
                }
                else
                {
                    if (CheckAreaActionPossibility(this.gameObject, otherObject))
                        return otherObject;
                }
            }
        }

        return null;
    }

    public bool CheckCardActionPossibility(GameObject cardObject, GameObject otherObject)
    {
        Card otherCard = otherObject.GetComponent<Card>();
        string move = "";
        move += card.area;
        move += otherCard.area;
        move += card.actualPlayer == otherCard.actualPlayer ? "m" : "o";
        move += "c";

        //Debug.Log(move);

        return (card.cardInfo.possibleMoves.Contains(move));
    }

    public bool CheckAreaActionPossibility(GameObject cardObject, GameObject otherObject)
    {
        Area otherArea = otherObject.GetComponent<Area>();
        string move = "";
        move += card.area;
        move += otherArea.area;
        move += card.actualPlayer == otherArea.player ? "m" : "o";
        move += "a";

        //Debug.Log(move);

        return (card.cardInfo.possibleMoves.Contains(move));
    }

}
