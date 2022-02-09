using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducerEnemy : Unit
{
    GameObject targetTower;
    Transform targetTowerTransform;
    RoundManager _roundManager;

    new void Awake(){
        base.Awake();
        _roundManager = GameObject.Find("Grid").GetComponent<RoundManager>();
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        SetTarget(towers[Random.Range(0, towers.Length)]);
    }

    new void Start()
    {
        base.Start();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if(Moving){
            float distance = Vector3.Distance(_transform.position, targetTowerTransform.position);
            if(distance < 2f){
                if(distance < 0.8f){
                    ReduceDiceNumberOfTarget();
                    return;
                }

                MoveTowardTargetTower();
                
            }else{
                Move();
                if(slow > 0){
                    slow--;
                }
            } 
        }
    }

    protected void MoveTowardTargetTower(){
        Vector3 unitMovementVector = Vector3.Normalize(targetTowerTransform.position - _transform.position);
        float magnitude = speed * (1f - slow * 0.0014f);
        Vector3 changeVector = unitMovementVector * magnitude;
        _transform.position += changeVector;
        distanceTraveled += magnitude;
    }

    public void SetTarget(GameObject tower){
        targetTower = tower;
        targetTowerTransform = targetTower.transform;
    }

    public void ReduceDiceNumberOfTarget(){
        targetTower.GetComponent<Tower>().ReduceDiceNumber();
        _roundManager.KillUnit(gameObject);
    }
}
