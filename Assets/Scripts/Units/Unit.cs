using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Transform _transform;
    public int PathPosition = 1;
    protected Vector3 TargetPosition;
    protected GridSystem Grid;
    protected RoundManager Manager;
    protected ItemSpawner Spawner;
    protected bool Moving = true;
    public float health = 1f;
    public int worth = 1;
    public float distanceTraveled = 0;
    public int Tier = 1;
    public float speed = 0.035f;
    public bool isShielded = false;
    public float incomingDamage = 0f;
    protected int slow = 0;

    protected void Awake(){
        _transform = transform;
        GameObject grid = GameObject.FindWithTag("Grid");
        Grid = grid.GetComponent<GridSystem>();
        Manager = grid.GetComponent<RoundManager>();
        Spawner = grid.GetComponent<ItemSpawner>();
    }

    protected void Start()
    {
        TargetPosition = Grid.GetNthSquareOnPath(PathPosition);
    }

    protected void FixedUpdate()
    {
        if(Moving){
            Move();
            if(slow > 0){
                slow--;
            }
            
        } 
    }

    protected void Move(){
        FindNextPathPoint();
        MoveTowardsNextPathPoint();
    }

    public void SetPathPoint(int point){
        PathPosition = point;
        TargetPosition = Grid.GetNthSquareOnPath(PathPosition);
    }

    protected void MoveTowardsNextPathPoint(){
        Vector3 unitMovementVector = Vector3.Normalize(TargetPosition - _transform.position);
        float magnitude = speed * (1f - slow * 0.001f);
        Vector3 changeVector = unitMovementVector * magnitude;
        _transform.position += changeVector;
        distanceTraveled += magnitude;
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
        float reducedDamage = ShieldReduction(damage);
        health -= reducedDamage;
        incomingDamage -= reducedDamage;

        if(health <= 0){
            Die();
        }
    }

    public void TakeLaserDamage(float damage){
        float reducedDamage = ShieldReduction(damage);
        health -= reducedDamage;

        if(health <= 0){
            Die();
        }
    }

    protected virtual void Die(){
        Spawner.SpawnCoinAt(_transform.position, worth);
        Manager.KillUnit(gameObject);
    }

    public void TargetedForDamage(float damage){
        incomingDamage += ShieldReduction(damage);      
    }

    public bool IsOverKilled(){
        if(incomingDamage >= health + 0.1f){
            return true;
        }
        
        return false;
    }

    protected float ShieldReduction(float damage){
        if(isShielded){
            return damage * 0.6f;
        }
        return damage;
    }

    public void Shield(){
        GetShieldObject().SetActive(true);
        incomingDamage = 0.6f * incomingDamage;
        isShielded = true;
    }

    public void RemoveShield(){
        if(!isShielded){
            return;
        }

        GetShieldObject().SetActive(false);
        incomingDamage =  incomingDamage/0.6f;
        isShielded = false;
    }

    private GameObject GetShieldObject(){
        return _transform.GetChild(0).gameObject;
    }

    public bool IsShielded(){
        return isShielded;
    }

    public void Slow(int amount){
        if(slow < 400){
            slow+=amount;
        }
        
    }
}
