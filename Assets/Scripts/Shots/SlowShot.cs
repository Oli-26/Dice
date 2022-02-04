using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlowShot : Shot
{
    int slowAmount = 50;
    void Start()
    {
        
    }

    new void FixedUpdate()
    {
        try{
            Vector3 distance = Target.transform.position - _transform.position;

            if(Vector3.Distance(new Vector3(0,0,0), distance) < 0.15f){
                Target.GetComponent<Unit>().TakeDamage(Damage);
                Target.GetComponent<Unit>().Slow(slowAmount);
                Destroy(gameObject);
                return;
            }

            Vector3 unitMovementVector = Vector3.Normalize(distance);
            _transform.position += unitMovementVector * Speed;

        }catch(Exception e){
            Debug.Log(e);
            Destroy(gameObject);
        }
    }

    public new void Init(GameObject target, float damage, float speed, int slowAmount){
        this.slowAmount = slowAmount;
        base.Init(target, damage, speed);
    }
}
