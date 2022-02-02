using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shot : MonoBehaviour
{
    public GameObject Target;
    public float Damage;
    public float Speed;

    protected Transform _transform;

    protected void Awake(){
        _transform = transform;
    }

    void Start()
    {
        
    }

    protected void FixedUpdate()
    {
        try{
            Vector3 distance = Target.transform.position - _transform.position;

            if(Vector3.Distance(new Vector3(0,0,0), distance) < 0.15f){
                Target.GetComponent<Unit>().TakeDamage(Damage);
                Destroy(gameObject);
                return;
            }

            Vector3 unitMovementVector = Vector3.Normalize(distance);
            _transform.position += unitMovementVector * Speed;

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
