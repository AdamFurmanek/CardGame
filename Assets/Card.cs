using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Table table;
    public CardInfo cardInfo;
    public int actualHealth, actualStrength;
    public int originalPlayer, actualPlayer;
    public int area;

    public GameObject cardObjectTop;
    public GameObject cardObjectBottom;
    public GameObject cardObjectCover;

    public GameObject image;
    public GameObject healthLabel;
    public GameObject strengthLabel;
    public GameObject nameLabel;

    public ParticleSystem hitParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem areaParticles;

    public float positionXOffset, positionZOffset, rotationOffset;
    public Vector3 destinationPosition;
    private Vector3 destinationScale = new Vector3(1.65f, 0.0001f, 2.55f);
    public Color destinationColor;
    public Color actualColor;

    private void Start()
    {
        table = FindObjectOfType<Table>();
        healthLabel.GetComponent<TextMeshPro>().enableCulling = true;
        strengthLabel.GetComponent<TextMeshPro>().enableCulling = true;
        nameLabel.GetComponent<TextMeshPro>().enableCulling = true;
    }

    public void Prepare(CardInfo cardInfo, int originalPlayer)
    {
        this.cardInfo = cardInfo;
        this.originalPlayer = originalPlayer;
        ResetStats();
        nameLabel.GetComponent<TextMeshPro>().text = cardInfo.name;
        destinationColor = new Color(0.8f, 0.8f, 0.8f);
        actualColor = new Color(0.8f, 0.8f, 0.8f);

        Material material = Resources.Load<Material>("Materials/" + cardInfo.name);
        image.GetComponent<Renderer>().material = material;
    }

    public void ResetStats()
    {
        actualHealth = cardInfo.health;
        actualStrength = cardInfo.strength;
        actualPlayer = originalPlayer;
    }

    public IEnumerator Move()
    {
        float deltaX = destinationPosition.x - gameObject.transform.position.x;
        float deltaY = destinationPosition.y - gameObject.transform.position.y;
        float deltaZ = destinationPosition.z - gameObject.transform.position.z;

        float deltaScaleX = destinationScale.x - gameObject.transform.localScale.x;
        float deltaScaleY = destinationScale.y - gameObject.transform.localScale.y;
        float deltaScaleZ = destinationScale.z - gameObject.transform.localScale.z;

        float deltaColorR = destinationColor.r - actualColor.r;
        float deltaColorG = destinationColor.g - actualColor.g;
        float deltaColorB = destinationColor.b - actualColor.b;

        for (int i = 0; i < 10; i++)
        {
            gameObject.transform.position += new Vector3(deltaX, deltaY, deltaZ) / 10;
            gameObject.transform.localScale += new Vector3(deltaScaleX, deltaScaleY, deltaScaleZ) / 10;
            actualColor.r += deltaColorR / 10;
            actualColor.g += deltaColorG / 10;
            actualColor.b += deltaColorB / 10;
            cardObjectTop.GetComponent<Renderer>().material.color = actualColor;
            cardObjectBottom.GetComponent<Renderer>().material.color = actualColor;

            yield return null;
        }
    }

}
