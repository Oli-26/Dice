using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShockShot : Shot
{
    Targeting _targeting;
    float range = 1.5f;
    int count = 1;

    new void Awake(){
        base.Awake();
        _targeting = GetComponent<Targeting>();
    }

    void Start(){
        _targeting.SetTargetingMode(TargetingMode.ClosestNew);
    }

    public void Init(GameObject target, float damage, float speed, int bounceCount){
        this.count = bounceCount;
        base.Init(target, damage, speed);
        _targeting.SetTarget(target);
    }

    new void FixedUpdate()
    {
        try{
            Vector3 distance = Target.transform.position - _transform.position;

            if(Vector3.Distance(new Vector3(0,0,0), distance) < 0.15f){
                Unit unit = Target.GetComponent<Unit>();
                unit.RemoveShield();
                unit.TakeDamage(Damage);
                count--;

                if(count <= 0){
                    Destroy(gameObject);
                    return;
                }
                
                _targeting.Retarget(range);
                if(_targeting.TargetIsSet()){
                    Target = _targeting.GetTarget();
                    Target.GetComponent<Unit>().TargetedForDamage(Damage);
                }else{
                    Destroy(gameObject);
                }
            }

            Vector3 unitMovementVector = Vector3.Normalize(distance);
            _transform.position += unitMovementVector * Speed;

        }catch(Exception e){
            Debug.Log(e);
            Destroy(gameObject);
        }
    }
}
