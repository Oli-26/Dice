using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    void Start()
    {
        
    }

    public void SpawnCoinAt(Vector3 position, int value){
        GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
        coin.GetComponent<Coin>().value = value;
    }
}
