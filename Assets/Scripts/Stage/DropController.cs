using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private float _rateLife = 5;
    private float _rateBigEnergy = 10;
    private float _rateSmallEnergy = 20;
    private float _ratescoreBall = 70;

    public GameObject lifePrefab;
    public GameObject largeEnergyPrefab;
    public GameObject smallEnergyPrefab;
    public GameObject scoreBallPrefab;

    public void dropTry(Transform dropPosition)
    {
        float chance = Random.Range(0.0f, 100.0f);
        if (chance <= _rateLife)
        {
            Instantiate(lifePrefab, dropPosition.position, dropPosition.rotation);
        } else if (chance <= _rateBigEnergy)
        {
            Instantiate(largeEnergyPrefab, dropPosition.position, dropPosition.rotation);
        } else if (chance <= _rateSmallEnergy)
        {
            Instantiate(smallEnergyPrefab, dropPosition.position, dropPosition.rotation);
        } else if (chance <= _ratescoreBall)
        {
            Instantiate(scoreBallPrefab, dropPosition.position, dropPosition.rotation);
        }
    }
}
