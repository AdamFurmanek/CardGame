using System;
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

    public GameObject cardObjectTop;
    public GameObject cardObjectBottom;

    public GameObject healthLabel;
    public GameObject strengthLabel;
    public GameObject nameLabel;

    public ParticleSystem hitParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem areaParticles;

    public float positionXOffset, positionZOffset, rotationOffset;
    public Vector3 destinationPosition;

    private void Start()
    {
        table = FindObjectOfType<Table>();
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
        float deltaXZ = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);

        for(int i = 0; i < 10; i++)
        {
            gameObject.transform.position += new Vector3(1, 0, 0) * deltaX / 10;
            gameObject.transform.position += new Vector3(0, 1, 0) * deltaY / 10;
            gameObject.transform.position += new Vector3(0, 0, 1) * deltaZ / 10;

            yield return null;
        }
    }

}
