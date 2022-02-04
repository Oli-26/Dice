using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockTower : Tower
{
    new void Start()
    {
        base.Start();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack(){
        _targeting.Retarget(range);

        if(_targeting.TargetIsSet()){
            nextShot = shotCoolDown;
            _targeting.GetTarget().GetComponent<Unit>().TargetedForDamage(damage);
            ShockShot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<ShockShot>();
            shot.Init(_targeting.GetTarget(), damage, shotSpeed, diceNumber);
        }
    }
}
