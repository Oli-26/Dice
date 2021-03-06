using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TargetingMode {First, Strong, Weak, Last, Closest, ClosestNew, FirstNew, Random};
public class Targeting : MonoBehaviour
{
    public TargetingMode Mode = TargetingMode.First;
    GameObject Target;
    bool TargetSet = false;
    RoundManager Manager;
    List<GameObject> alreadyTargeted;
    Transform _transform;

    void Awake(){
        alreadyTargeted = new List<GameObject>();
        _transform = transform;
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

    public void SetTargetingMode(int id){
        switch(id){
            case 0:
                Mode = TargetingMode.First;
                break;
            case 1:
                Mode = TargetingMode.Last;
                break;
            case 2:
                Mode = TargetingMode.Strong;
                break;
            default:
                Mode = TargetingMode.First;
                break;
        }
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
                case TargetingMode.Random:
                    RandomTargetingHelper(range);
                    break;
                default:
                    TargetingHelper(range);
                    break;
        }
    }

    private void RandomTargetingHelper(float range){
        TargetSet = false;
        GameObject[] targetableEnemies = GetTargetableEnemies(range);
        if(targetableEnemies.Length == 0){
            return;
        }

        int randomTarget = Random.Range(0, targetableEnemies.Length);
        SetTarget(targetableEnemies[randomTarget]);

    }

    private void TargetingHelper(float range){
        float savedValue = getInitialSavedValue();
        TargetSet = false;
        GameObject tempTarget = null;
        GameObject[] targetableEnemies = GetTargetableEnemies(range);

        for(int i = 0; i < targetableEnemies.Length; i++){
            GameObject enemyObject = targetableEnemies[i];
            if(enemyObject && !enemyObject.GetComponent<Unit>().IsOverKilled()){
                float compareValue = getCompareValue(enemyObject);
                
                if(compareValues(compareValue, savedValue)){
                    savedValue = compareValue;
                    TargetSet = true;
                    tempTarget = enemyObject;
                }
            }
        }  
        if(TargetSet){
            SetTarget(tempTarget);
        }
    }


    private GameObject[] GetTargetableEnemies(float range){
        GameObject[] enemies = Manager.GetAliveUnits();
        GameObject[] enemiesInRange = enemies.Where(e => e != null && Vector3.Distance(e.transform.position, _transform.position) < range).ToArray();
        GameObject[] targetableEnemies;
        if(excludePreviouslyTargeted()){
            targetableEnemies = enemiesInRange.Where(e => !alreadyTargeted.Any(e2 => e == e2)).ToArray();
        }else{
            targetableEnemies = enemiesInRange;
        }

        return targetableEnemies;
    }

    private bool excludePreviouslyTargeted(){
        switch(Mode){
                case TargetingMode.FirstNew:
                case TargetingMode.ClosestNew:
                    return true;
                case TargetingMode.First:
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
                    return 0f;
                case TargetingMode.Last:
                case TargetingMode.Closest:
                case TargetingMode.ClosestNew:
                    return 100000f;
                default:
                    return 0f;
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
                    return enemy.GetComponent<Unit>().Tier * 1000f + enemy.GetComponent<Unit>().GetDistanceTraveled();
                case TargetingMode.Closest:
                case TargetingMode.ClosestNew:
                    return Vector3.Distance(enemy.transform.position, transform.position);
                default:
                    return 0f;
          }
    }
}
