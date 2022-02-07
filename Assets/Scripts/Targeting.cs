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
        TargetingHelper();
    }

    private void TargetingHelper(){
        float savedValue = getInitialSavedValue();
        TargetSet = false;
        GameObject tempTarget = null;
        GameObject[] targetableEnemies = GetTargetableEnemies();

        for(int i = 0; i < targetableEnemies.Length; i++){
            Unit enemy = targetableEnemies[i].GetComponent<Unit>();
            if(Vector3.Distance(targetableEnemies[i].transform.position, transform.position) < range && !enemy.IsOverKilled()){
                float compareValue = getCompareValue(targetableEnemies[i]);
                
                if(compareValues(compareValue, savedValue)){
                    savedValue = compareValue;
                    TargetSet = true;
                    tempTarget = targetableEnemies[i];
                }
            }
        }  
        if(TargetSet){
            SetTarget(tempTarget);
        }
    }


    private GameObject[] GetTargetableEnemies(){
        GameObject[] enemies = Manager.GetAliveUnits();
        GameObject[] targetableEnemies;
        if(excludePreviouslyTargeted()){
            targetableEnemies = enemies.Where(e => !alreadyTargeted.Any(e2 => e == e2)).ToArray();
        }else{
            targetableEnemies = enemies;
        }

        return targetableEnemies;
    }

    private bool excludePreviouslyTargeted(){
        switch(Mode){
            case TargetingMode.First:
                case TargetingMode.FirstNew:
                case TargetingMode.ClosestNew:
                    return true;
                case TargetingMode.Last:
                case TargetingMode.Closest:
                case TargetingMode.Strong:
                    return false;
                default:
                    return false;
        }
    }
    private float getInitialSavedValue(){
        switch(Mode){
            case TargetingMode.First:
                case TargetingMode.FirstNew:
                case TargetingMode.Strong:
                    return 0;
                case TargetingMode.Last:
                case TargetingMode.Closest:
                case TargetingMode.ClosestNew:
                    return 100000;
                default:
                    return 0;
        }
    }

    private bool compareValues(float newValue, float oldValue){
        switch(Mode){
            case TargetingMode.First:
                case TargetingMode.FirstNew:
                case TargetingMode.Strong:
                    return newValue > oldValue;
                case TargetingMode.Last:
                case TargetingMode.Closest:
                case TargetingMode.ClosestNew:
                    return newValue < oldValue;
                default:
                    return false;
        }
    }

    private float getCompareValue(GameObject enemy){
        switch(Mode){
                case TargetingMode.First:
                case TargetingMode.FirstNew:
                case TargetingMode.Last:
                    return enemy.GetComponent<Unit>().GetDistanceTraveled();
                case TargetingMode.Strong:
                    return enemy.GetComponent<Unit>().Tier * 1000 + enemy.GetComponent<Unit>().GetDistanceTraveled();
                case TargetingMode.Closest:
                case TargetingMode.ClosestNew:
                    return Vector3.Distance(enemy.transform.position, transform.position);
                default:
                    return 0;
          }
    }
}
