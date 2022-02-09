using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    GameObject towerBeingPlaced;
    Transform towerBeingPlacedTransform;
    bool placementActive = false;
    bool guardAgainstInstantPlacement = false;
    public GameObject[] towers;
    public List<GameObject> placedTowers;
    MouseTracker tracker;
    GridSystem grid;
    MoneyManager moneyManager;

    void Awake(){
        tracker = GetComponent<MouseTracker>();
        grid = GetComponent<GridSystem>();
        moneyManager = GetComponent<MoneyManager>();
    }

    void Start()
    {
        placedTowers = new List<GameObject>();
    }
    
    void Update()
    {
        if(placementActive){
            towerBeingPlacedTransform.position = SnapToGrid(tracker.GetMousePosition());
            if(Input.GetMouseButtonDown(0) && !guardAgainstInstantPlacement){
                PlaceTower();
            }

            if(guardAgainstInstantPlacement){
                guardAgainstInstantPlacement = false;
            }
        }
    }

    public void PlaceTower(){
        placementActive = false;
        guardAgainstInstantPlacement = true;
        placedTowers.Add(towerBeingPlaced);
        tracker.AddSelectableObject(towerBeingPlaced);
        towerBeingPlaced.GetComponent<Tower>().Place();
    }

    public void RemoveTower(GameObject tower){
        placedTowers.Remove(tower);
    }

    public void PurchaseTower(int id){
        GameObject tempTower = towers[id];
        int cost = tempTower.GetComponent<Tower>().cost;
        if(moneyManager.CheckFunds(cost)){
            towerBeingPlaced = Instantiate(tempTower, GetComponent<MouseTracker>().GetMousePosition(), Quaternion.identity);
            towerBeingPlacedTransform = towerBeingPlaced.transform;
            placementActive = true;
            guardAgainstInstantPlacement= true;
            moneyManager.SpendMoney(cost);
        }
    }

    public void SellTower(GameObject tower){
        int cost = tower.GetComponent<Tower>().cost;
        moneyManager.GainMoney(cost);
        
        tracker.RemoveSelectableObject(tower);
        RemoveTower(tower);
        Destroy(tower);

    }

    public Vector3 SnapToGrid(Vector3 pos){
        return grid.SnapToGrid(pos);
    }
}
