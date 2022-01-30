using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShockShot : Shot
{
    Targeting targeting;
    float range = 1.5f;
    int count = 3;

    void Start(){
        targeting = GetComponent<Targeting>();
        targeting.SetTargetingMode(TargetingMode.Closest);
    }

    new void FixedUpdate()
    {
        try{
            Vector3 distance = Target.transform.position - transform.position;

            if(Vector3.Distance(new Vector3(0,0,0), distance) < 0.15f){
                Target.GetComponent<Unit>().TakeDamage(Damage);
                count--;

                if(count <= 0){
                    Destroy(gameObject);
                    return;
                }
                
                targeting.Retarget(range);
                if(targeting.TargetIsSet()){
                    Target = targeting.GetTarget();
                }else{
                    Destroy(gameObject);
                }
            }

            Vector3 unitMovementVector = Vector3.Normalize(distance);
            transform.position += unitMovementVector * Speed;

        }catch(Exception e){
            Destroy(gameObject);
        }
    }
}
