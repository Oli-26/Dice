using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Selectable
{
    public float damage = 1f;
    public float shotSpeed = 0.1f;
    float nextShot = 0;
    public float shotCoolDown = 1f;
    float range = 5f;
    public GameObject shotPrefab;
    void Start()
    {
        
    }

    protected void FixedUpdate()
    {
        if(nextShot < 0){
            Attack();
        }else{
            nextShot -= Time.deltaTime;
        }
    }

    void Attack(){
        Targeting targeting = GetComponent<Targeting>();
        targeting.Retarget(range);
        if(targeting.TargetIsSet()){
            nextShot = shotCoolDown;
            Shot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<Shot>();
            shot.Init(targeting.GetTarget(), damage, shotSpeed);
        }
    }
}
