using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LaserTower : Tower
{
    List<GameObject> tethered;
    LaserManager laserManager;

    void Start()
    {
        peaceful = true;
        laserManager = GameObject.Find("Grid").GetComponent<LaserManager>();
        tethered = new List<GameObject>(); 
        base.Start();   
    }

    void Update()
    {
        
    }

    public void TetherOtherLaserTowers(){
        GameObject[] towersInRange = GameObject.FindGameObjectsWithTag("Tower").Where(t => Vector3.Distance(_transform.position, t.transform.position) < range).ToArray();
        GameObject[] laserTowersInRange = towersInRange.Where(t => t.GetComponent<LaserTower>() != null).ToArray();

        foreach(GameObject tower in laserTowersInRange){
            if(gameObject != tower){
                laserManager.CreatePairing(gameObject, tower);
            }
        }
    }

    public override void ReRoll(){
        base.ReRoll();
        TetherOtherLaserTowers();
    }

    public override void Place(){
        base.Place();
        TetherOtherLaserTowers();
    }
}
