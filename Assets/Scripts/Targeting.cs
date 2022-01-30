using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TargetingMode {First, Strong, Weak, Last, Closest};
public class Targeting : MonoBehaviour
{
    protected TargetingMode Mode = TargetingMode.First;
    GameObject Target;
    bool TargetSet = false;
    RoundManager Manager;
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
                    TargetClosest(range);
                    break;
                default:
                    TargetFirst(range);
                    break;
          }
    }

    protected virtual void TargetClosest(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float leastDistance = range + 1f;
        TargetSet = false;

        for(int i = 0; i < enemies.Length; i++){
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range){
                float distance = Vector3.Distance(enemies[i].transform.position, transform.position);
                if(distance < leastDistance){
                    Target = enemies[i];
                    leastDistance = distance;
                    TargetSet = true;
                }
            }
        }  
    }

    protected virtual void TargetFirst(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float maxDistanceTraveled = 0f;
        TargetSet = false;

        for(int i = 0; i < enemies.Length; i++){
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range){
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(lengthTraveled > maxDistanceTraveled){
                    Target = enemies[i];
                    maxDistanceTraveled = lengthTraveled;
                    TargetSet = true;
                }
            }
        }   
    }

    protected virtual void TargetStrongest(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float highestTier = 0f;
        float maxDistanceTraveled = 0f;
        TargetSet = false;

        for(int i = 0; i < enemies.Length; i++){
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range){
                float enemyTier = enemies[i].GetComponent<Unit>().Tier;
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(enemyTier > highestTier && maxDistanceTraveled < lengthTraveled){
                    Target = enemies[i];
                    highestTier = enemyTier;
                    maxDistanceTraveled = lengthTraveled;
                    TargetSet = true;
                }
            }
        }  
    }
    
    protected virtual void TargetWeakest(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float lowestTier = 100f;
        float maxDistanceTraveled = 0f;
        TargetSet = false;

        for(int i = 0; i < enemies.Length; i++){
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range){
                float enemyTier = enemies[i].GetComponent<Unit>().Tier;
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(enemyTier < lowestTier && maxDistanceTraveled < lengthTraveled){
                    Target = enemies[i];
                    lowestTier = enemyTier;
                    maxDistanceTraveled = lengthTraveled;
                    TargetSet = true;
                }
            }
        }  
    }

    protected virtual void TargetLast(float range){
        GameObject[] enemies = Manager.GetComponent<RoundManager>().GetAliveUnits();
        float leastDistanceTraveled = 1000f;
        TargetSet = false;

        for(int i = 0; i<enemies.Length; i++){
            if(Vector3.Distance(enemies[i].transform.position, transform.position) < range){
                float lengthTraveled = enemies[i].GetComponent<Unit>().GetDistanceTraveled();
                if(lengthTraveled < leastDistanceTraveled){
                    Target = enemies[i];
                    leastDistanceTraveled = lengthTraveled;
                    TargetSet = true;
                }
            }
        }   
    }
}
