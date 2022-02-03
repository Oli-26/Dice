using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Transform _transform;
    public int PathPosition = 1;
    Vector3 TargetPosition;
    GridSystem Grid;
    RoundManager Manager;
    ItemSpawner Spawner;
    bool Moving = true;
    public float health = 1f;
    public int worth = 1;
    public float distanceTraveled = 0;
    public int Tier = 1;
    public float speed = 0.035f;
    public bool isShielded = false;
    float incomingDamage = 0f;
    protected void Awake(){
        _transform = transform;
    }

    protected void Start()
    {
        GameObject grid = GameObject.FindWithTag("Grid");
        Grid = grid.GetComponent<GridSystem>();
        Manager = grid.GetComponent<RoundManager>();
        Spawner = grid.GetComponent<ItemSpawner>();
        TargetPosition = Grid.GetNthSquareOnPath(PathPosition);
    }

    protected void FixedUpdate()
    {
        if(Moving){
            Move();
        } 
    }

    protected void Move(){
        FindNextPathPoint();
        MoveTowardsNextPathPoint();
    }

    protected void MoveTowardsNextPathPoint(){
        Vector3 unitMovementVector = Vector3.Normalize(TargetPosition - _transform.position);
        Vector3 changeVector = unitMovementVector * speed;
        _transform.position += changeVector;
        distanceTraveled += Vector3.Distance(new Vector3(0f, 0f, 0f), changeVector);
    }

    public float GetDistanceTraveled(){
        return distanceTraveled;
    }
    
    protected void FindNextPathPoint(){
        if(Vector3.Distance(_transform.position, TargetPosition) < 0.1f){
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
        health -= ShieldReduction(damage);
        incomingDamage -= ShieldReduction(damage);

        if(health <= 0){
            Spawner.SpawnCoinAt(_transform.position, worth);
            Manager.KillUnit(gameObject);
        }
    }

    public void TargetedForDamage(float damage){
        incomingDamage += ShieldReduction(damage);      
    }

    public bool IsOverKilled(){
        if(incomingDamage >= health){
            return true;
        }
        
        return false;
    }

    float ShieldReduction(float damage){
        if(isShielded){
            return damage * 0.75f;
        }
        return damage;
    }

    public void Shield(){
        _transform.GetChild(0).gameObject.SetActive(true);
        incomingDamage = 0.75f * incomingDamage;
        isShielded = true;
    }

    public void RemoveShield(){
        _transform.GetChild(0).gameObject.SetActive(false);
        incomingDamage = 1.33f * incomingDamage;
        isShielded = false;

    }
    public bool IsShielded(){
        return isShielded;
    }
}
