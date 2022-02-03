using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Vector3 target;
    Transform _transform;
    MoneyManager moneyManager;
    
    public int value;
    public float speed = 1f;

    void Start()
    {
        target = GameObject.Find("MoneyIndicator").transform.position;
        moneyManager = GameObject.Find("Grid").GetComponent<MoneyManager>();
        _transform = transform;
    }


    void FixedUpdate()
    {
        float distance = Vector3.Distance(_transform.position, target);
        _transform.position = Vector3.MoveTowards(_transform.position, target, 0.07f * distance);

        if(Vector3.Distance(_transform.position, target) < 0.5f){
            moneyManager.GainMoney(value);
            Destroy(gameObject);
        }
    }
}
