using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int PathPosition = 1;
    Vector3 TargetPosition;
    GridSystem Grid;
    RoundManager Manager;
    bool Moving = true;
    public float health = 1f;
    public int worth = 1;
    public float distanceTraveled = 0;
    public int Tier = 1;
    public float speed = 0.035f;
    void Start()
    {
        Grid = GameObject.FindWithTag("Grid").GetComponent<GridSystem>();
        Manager = GameObject.FindWithTag("Grid").GetComponent<RoundManager>();
        TargetPosition = Grid.GetNthSquareOnPath(PathPosition);
    }

    void Update()
    {
        if(Moving){
            Move();
        } 
    }

    void Move(){
        FindNextPathPoint();
        MoveTowardsNextPathPoint();
    }

    void MoveTowardsNextPathPoint(){
        Vector3 unitMovementVector = Vector3.Normalize(TargetPosition - transform.position);
        Vector3 changeVector = unitMovementVector * speed;
        transform.position += changeVector;
        distanceTraveled += Vector3.Distance(new Vector3(0f, 0f, 0f), changeVector);
    }

    public float GetDistanceTraveled(){
        return distanceTraveled;
    }
    
    void FindNextPathPoint(){
        if(Vector3.Distance(transform.position, TargetPosition) < 0.1f){
            if(Grid.GetAmountOfPoints() <= PathPosition + 1){
                Moving = false;
                FinishRun();
            }else{
                PathPosition++;
                TargetPosition = Grid.GetNthSquareOnPath(PathPosition);
            }
        }
    }

    void FinishRun(){
        GameObject.FindWithTag("Grid").GetComponent<Effects>().Flash();
        Manager.KillUnit(gameObject);
    }

    public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0){
            Manager.KillUnit(gameObject);
        }
    }
}
