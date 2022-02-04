using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TargetingMode {First, Strong, Weak, Last, Closest, ClosestNew, FirstNew};
public class Targeting : MonoBehaviour
{
    protected TargetingMode Mode = TargetingMode.First;
    GameObject Target;
    bool TargetSet = false;
    RoundManager Manager;
    List<GameObject> alreadyTargeted;

    void Awake(){
        alreadyTargeted = new List<GameObject>();
    }

    void Start()
    {
        Manager = GameObject.FindWithTag("Grid").GetComponent<RoundManager>();

    }

    public GameObject GetTarget(){
        return Target;
    }

    public bool TargetIsSet(){
        return TargetSet;
    }

    public TargetingMode GetTargetingMode(){
        return Mode;
    }

    public void SetTargetingMode(TargetingMode m){
        Mode = m;
    }

    public void ForgetPreviousTargets(){
        alreadyTargeted = new List<GameObject>();
    }

    public void SetTarget(GameObject target){
        Target = target;
        alreadyTargeted.Add(target);
        TargetSet = true;
    }

    public virtual void Retarget(float range){
          switch(Mode){
                case TargetingMode.First:
                    TargetFirst(range);
                    break;
                case TargetingMode.Strong:
                    TargetStrongest(range);
                    break;
                case TargetingMode.Weak:
                    TargetWeakest(range);
                    break;
                case TargetingMode.Last:
                    TargetLast(range);
                    break;
                case TargetingMode.Closest:
                    TargetClosest(range, Target);
                    break;
                case TargetingMode.ClosestNew:
                    TargetClosestNew(range);
                    break;
                case TargetingMode.FirstNew:
                    TargetFirstNew(range);
                    break;
                default:
                    TargetFirst(range);
                    break;
          }
    }

    protected virtual void TargetClosestNew(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float leastDistance = range + 1f;
        TargetSet = false;
        GameObject tempTarget = null;
        GameObject[] unTargetedEnemies = enemies.Where(e => !alreadyTargeted.Any(e2 => e == e2)).ToArray();

        for(int i = 0; i < unTargetedEnemies.Length; i++){
            Unit enemy = unTargetedEnemies[i].GetComponent<Unit>();
            if(Vector3.Distance(unTargetedEnemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float distance = Vector3.Distance(unTargetedEnemies[i].transform.position, transform.position);
                if(distance < leastDistance){
                    TargetSet = true;
                    tempTarget = unTargetedEnemies[i];
                    
                    leastDistance = distance;
                }
            }
        }  
        if(TargetSet){
            SetTarget(tempTarget);
        }
        
    }

    protected virtual void TargetClosest(float range, GameObject previousTarget = null){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float leastDistance = range + 1f;
        TargetSet = false;
        GameObject tempTarget = null;

        for(int i = 0; i < enemies.Length; i++){
            Unit enemy = enemies[i].GetComponent<Unit>();
            if(previousTarget != enemies[i] && Vector3.Distance(enemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float distance = Vector3.Distance(enemies[i].transform.position, transform.position);
                if(distance < leastDistance){
                    TargetSet = true;
                    tempTarget = enemies[i];
                    leastDistance = distance;
                }
            }
        }  

        if(TargetSet){
            SetTarget(tempTarget);
        }
    }

    protected virtual void TargetFirst(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float maxDistanceTraveled = 0f;
        TargetSet = false;
        GameObject tempTarget = null;
        

        for(int i = 0; i < enemies.Length; i++){
            Unit enemy = enemies[i].GetComponent<Unit>();
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(lengthTraveled > maxDistanceTraveled){
                    TargetSet = true;
                    tempTarget = enemies[i];
                    maxDistanceTraveled = lengthTraveled;
                }
            }
        }  

        if(TargetSet){
            SetTarget(tempTarget);
        } 
    }

    protected virtual void TargetFirstNew(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float maxDistanceTraveled = 0f;
        TargetSet = false;
        GameObject tempTarget = null;
        GameObject[] unTargetedEnemies = enemies.Where(e => !alreadyTargeted.Any(e2 => e == e2)).ToArray();
        enemies = unTargetedEnemies;

        for(int i = 0; i < enemies.Length; i++){
            Unit enemy = enemies[i].GetComponent<Unit>();
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(lengthTraveled > maxDistanceTraveled){
                    TargetSet = true;
                    tempTarget = enemies[i];
                    maxDistanceTraveled = lengthTraveled;
                }
            }
        }  

        if(TargetSet){
            SetTarget(tempTarget);
        } 
    }

    protected virtual void TargetStrongest(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float highestTier = 0f;
        float maxDistanceTraveled = 0f;
        TargetSet = false;
        GameObject tempTarget = null;

        for(int i = 0; i < enemies.Length; i++){
            Unit enemy = enemies[i].GetComponent<Unit>();
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float enemyTier = enemies[i].GetComponent<Unit>().Tier;
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(enemyTier > highestTier && maxDistanceTraveled < lengthTraveled){
                    TargetSet = true;
                    tempTarget = enemies[i];
                    highestTier = enemyTier;
                    maxDistanceTraveled = lengthTraveled;
                }
            }
        }  

        if(TargetSet){
            SetTarget(tempTarget);
        }
    }
    
    protected virtual void TargetWeakest(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float lowestTier = 100f;
        float maxDistanceTraveled = 0f;
        TargetSet = false;
        GameObject tempTarget = null;

        for(int i = 0; i < enemies.Length; i++){
            Unit enemy = enemies[i].GetComponent<Unit>();
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float enemyTier = enemies[i].GetComponent<Unit>().Tier;
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(enemyTier < lowestTier && maxDistanceTraveled < lengthTraveled){
                    TargetSet = true;
                    tempTarget = enemies[i];
                    lowestTier = enemyTier;
                    maxDistanceTraveled = lengthTraveled;
                }
            }
        }  

        if(TargetSet){
            SetTarget(tempTarget);
        }
    }

    protected virtual void TargetLast(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float leastDistanceTraveled = 1000f;
        TargetSet = false;
        GameObject tempTarget = null;

        for(int i = 0; i<enemies.Length; i++){
            Unit enemy = enemies[i].GetComponent<Unit>();
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(lengthTraveled < leastDistanceTraveled){
                    TargetSet = true;
                    tempTarget = enemies[i];
                    leastDistanceTraveled = lengthTraveled;
                }
            }
        }

        if(TargetSet){
            SetTarget(tempTarget);
        }   
    }
}
