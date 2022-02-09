using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    List<PairOfTowers> pairs;
    public GameObject laserPrefab;

    float laserCooldown = 1f;
    RoundManager _roundManager;

    void Start()
    {
        _roundManager = GetComponent<RoundManager>();
        pairs = new List<PairOfTowers>();
    }

    void FixedUpdate()
    {
        if(_roundManager.IsRoundActive()){
            if(laserCooldown <= 0){
                CreateLasers();
                laserCooldown = 2f;
            }else{
                laserCooldown -= Time.deltaTime;
            }
        }
    }

    public void ResetPairings(){
        pairs = new List<PairOfTowers>();
    }

    public void CreatePairing(GameObject t1, GameObject t2){
        foreach(PairOfTowers pair in pairs){
            if(pair.containsTowers(t1, t2)){
                return;
            }
            
        }
        pairs.Add(new PairOfTowers(t1, t2));
    }

    public void CreateLasers(){
        foreach(PairOfTowers pair in pairs){
            GameObject laser = Instantiate(laserPrefab, pair.GetMidPoint(), Quaternion.identity);
            laser.transform.localScale = new Vector3(laser.transform.localScale.x, pair.GetDistance(), laser.transform.localScale.z);
            laser.transform.rotation = Quaternion.FromToRotation(Vector3.up, pair.GetDirection());
            laser.GetComponent<Laser>().Init(pair.GetDiceNumberDifference());
        }
    }
}

class PairOfTowers{
    GameObject tower1;
    GameObject tower2;

    public PairOfTowers(GameObject t1, GameObject t2){
        tower1 = t1;
        tower2 = t2;
    }

    public bool containsTowers(GameObject t1, GameObject t2){
        bool condition1 = tower1 == t1 && tower2 == t2;
        bool condition2 = tower2 == t1 && tower1 == t2;
        return condition1 || condition2;
    }

    public Vector3 GetMidPoint(){
        return (tower1.transform.position + tower2.transform.position) / 2;
    }

    public float GetDistance(){
        return Vector3.Distance(tower1.transform.position, tower2.transform.position);
    }

    public Vector3 GetDirection(){
        return tower1.transform.position - tower2.transform.position;
    }

    public int GetDiceNumberDifference(){
        return Mathf.Abs(tower1.GetComponent<LaserTower>().diceNumber - tower2.GetComponent<LaserTower>().diceNumber);
    }
}
