using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Unit
{

    float shieldingCooldown = 1f;
    int numberToShield = 5;
    float range = 2.5f;
    
    new void Awake(){
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if(shieldingCooldown <= 0){
            ShieldAllies();
            shieldingCooldown = 4f;
        }else{
            shieldingCooldown -= Time.deltaTime;
        }
    }

    void ShieldAllies(){ 
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        int unitsShielded = 0;

        foreach(GameObject unit in units){
            if(unitsShielded >= numberToShield){
                return;
            }

            if(unit != gameObject && Vector3.Distance(unit.transform.position, _transform.position) < range){
                Unit script = unit.GetComponent<Unit>();
                if(!script.IsShielded()){
                    script.Shield();
                    unitsShielded++;
                }
            }
        }
    }
}
