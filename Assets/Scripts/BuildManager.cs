using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    GameObject towerBeingPlaced;
    Transform towerBeingPlacedTransform;
    bool placementActive = false;

    public GameObject[] towers;

    public List<GameObject> placedTowers;

    int moneyTEMP = 10000;

    MouseTracker tracker;

    void Awake(){
        tracker = GetComponent<MouseTracker>();
    }

    void Start()
    {
        placedTowers = new List<GameObject>();
    }

    
    void Update()
    {
        if(placementActive){
            towerBeingPlacedTransform.position = tracker.GetMousePosition();
        }

        if(placementActive && Input.GetMouseButtonDown(0)){
            placementActive = false;
            placedTowers.Add(towerBeingPlaced);
        }
    }

    public void CreateTower(){

    }

    public void PurchaseTower(int id){
        GameObject tempTower = towers[id];
        if(tempTower.GetComponent<Tower>().cost < moneyTEMP){
            towerBeingPlaced = Instantiate(tempTower, GetComponent<MouseTracker>().GetMousePosition(), Quaternion.identity);
            towerBeingPlacedTransform = towerBeingPlaced.transform;
            placementActive = true;
        }
    }
}
