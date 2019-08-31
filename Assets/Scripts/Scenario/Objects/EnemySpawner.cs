using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private GameObject enemyInstantiated;

    private void OnBecameVisible()
    {
        if (!enemyInstantiated)
        {
            enemyInstantiated = Instantiate(enemy, transform.position, transform.rotation);
            enemyInstantiated.transform.parent = gameObject.transform;
        }
    }  
}
