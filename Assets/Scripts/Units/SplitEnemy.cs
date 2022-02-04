using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : Unit
{
    RoundManager _roundManager;

    new void Awake(){
        _roundManager = GameObject.Find("Grid").GetComponent<RoundManager>();
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Die(){
        _roundManager.SpawnUnitAt(transform.position + new Vector3(-0.2f, 0f, 0f), 0, PathPosition, distanceTraveled);
        _roundManager.SpawnUnitAt(transform.position + new Vector3(0.2f, 0f, 0f), 0, PathPosition, distanceTraveled);
        base.Die();
    }
}
