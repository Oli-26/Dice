using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shot : MonoBehaviour
{
    public GameObject Target;
    public float Damage;
    public float Speed;
    void Start()
    {
        
    }

    protected void FixedUpdate()
    {
        try{
            Vector3 distance = Target.transform.position - transform.position;

            if(Vector3.Distance(new Vector3(0,0,0), distance) < 0.15f){
                Target.GetComponent<Unit>().TakeDamage(Damage);
                Destroy(gameObject);
                return;
            }

            Vector3 unitMovementVector = Vector3.Normalize(distance);
            transform.position += unitMovementVector * Speed;

        }catch(Exception e){
            Destroy(gameObject);
        }
    }

    public void Init(GameObject target, float damage, float speed){
        Target = target;
        Damage = damage;
        Speed = speed;
    }
}
