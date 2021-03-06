using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SlowTower : Tower
{
    new void Start()
    {
        base.Start();
        _targeting.SetTargetingMode(TargetingMode.Random);
    }

    new void FixedUpdate()
    {
        if(!peaceful && nextShot < 0 && _roundManager.IsRoundActive()){
            Attack();
        }else{
            nextShot -= Time.deltaTime;
        }
    }

    protected override void Attack(){
        
        for(int i = 0; i < diceNumber; i++){
            _targeting.Retarget(range);
            if(_targeting.TargetIsSet()){
                nextShot = shotCoolDown;
                _targeting.GetTarget().GetComponent<Unit>().TargetedForDamage(damage);
                SlowShot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<SlowShot>();
                shot.Init(_targeting.GetTarget(), damage, shotSpeed, diceNumber >= 4 ? 70 + 30*diceNumber : 70);
            }
        }
        

    }
}
