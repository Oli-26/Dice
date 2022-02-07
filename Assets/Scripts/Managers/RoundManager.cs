using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RoundManager : MonoBehaviour
{
    List<EnemySpawn> roundSpawns = new List<EnemySpawn>();
    public List<(bool, GameObject)> aliveEnemies = new List<(bool, GameObject)>();
    GridSystem Grid;

    public GameObject[] unitPrefabs;

    int roundTick = 0;
    bool roundActive = false;
    int round = 1;
    int SpawnsListPosition = 0;
    bool EnemyRecentlyDied = false;

    MoneyManager _moneyManager;

    void Awake(){
        _moneyManager = GetComponent<MoneyManager>();
    }

    void Start()
    {
        Grid = GetComponent<GridSystem>();
    }

    void FixedUpdate()
    {
        if(roundActive){
            if(SpawnsListPosition < roundSpawns.Count - 1){
                while(roundTick >= roundSpawns[SpawnsListPosition].getSpawnTime()){
                    SpawnUnit();
                    if(SpawnsListPosition >= roundSpawns.Count){
                        break;
                    }
                }
            }

            roundTick++;
        }
    }

    private void EndRound(){
        GainMoneyForRound();
        round++;
        roundActive = false;
    }

    private int GainMoneyForRound(){
        _moneyManager.GainMoney(10 * round*5);
    }

    public void StartRound(){
        roundTick = 0;
        SpawnsListPosition = 0;
        roundActive = true;

        LoadRound();
        ReRollAllTowers();
    }

    private void LoadRound(){
        roundSpawns = readRoundFromFile(round);
        roundSpawns.Sort((a, b) => a.getSpawnTime().CompareTo(b.getSpawnTime()));
    }

    private void ReRollAllTowers(){
        foreach(GameObject tower in GetComponent<BuildManager>().placedTowers){
            tower.GetComponent<Tower>().ReRoll();
        }
    }

    void SpawnUnit(){
        aliveEnemies.Add((true, Instantiate(roundSpawns[SpawnsListPosition].getUnit(), Grid.GetNthSquareOnPath(0), Quaternion.identity)));
        SpawnsListPosition++;
    }

    public void SpawnUnitAt(Vector3 position, int unitId, int targetPositionIndex, float distanceTraveled){
        GameObject unit = Instantiate(unitPrefabs[unitId], position, Quaternion.identity);
        aliveEnemies.Add((true, unit));
        unit.GetComponent<Unit>().SetPathPoint(targetPositionIndex);
        unit.GetComponent<Unit>().distanceTraveled = distanceTraveled;
    }

    public void KillUnit(GameObject unit){
        int index = aliveEnemies.IndexOf((true, unit));
        aliveEnemies[index] = (false, null);
        Destroy(unit);

        if(aliveEnemies.Count == 1){
            RemoveDeadEnemies();
            EndRound();
        }else{
            EnemyRecentlyDied = true;
        }
    }

    void RemoveDeadEnemies(){
        if(EnemyRecentlyDied){
            List<(bool, GameObject)> tempList =  aliveEnemies.Where(x => x.Item1 == true).ToList();
            aliveEnemies = tempList;
            EnemyRecentlyDied = false;
        }
    }

    public GameObject[] GetAliveUnits(){
        RemoveDeadEnemies();
        return aliveEnemies.Select(x => x.Item2).ToArray();
    }

    public bool IsRoundActive(){
        return GetAliveUnits().Length != 0;
    }

    List<EnemySpawn> readRoundFromFile(int round){
        try{
            TextAsset textasset = (TextAsset)Resources.Load("rounds/round" + round.ToString());
            string text = textasset.text;
            string[] lines = text.Split('\n');

            
            List<EnemySpawn> returnList = new List<EnemySpawn>();
            foreach(string line in lines){
                if(line == ""){
                    return returnList;
                }
                
                string[] values = line.Split(' ');
                int spawnAtTick = int.Parse(values[0]);
                GameObject enemyToSpawn = unitPrefabs[int.Parse(values[1])];
                if(values.Length > 2){
                    int amount = int.Parse(values[2]);
                    int spawnSeperator = 1;
                    if(values.Length > 3){
                        spawnSeperator = int.Parse(values[3]);
                    }
                        for(int i = 0; i < amount; i++){
                            int addedTickTime = i*spawnSeperator;
                            EnemySpawn nextSpawn = new EnemySpawn(spawnAtTick+addedTickTime, enemyToSpawn);
                            returnList.Add(nextSpawn);
                        }
                }else{
                    EnemySpawn nextSpawn = new EnemySpawn(spawnAtTick, enemyToSpawn);
                    returnList.Add(nextSpawn);
                }
            }
            return returnList;
            
        }catch(Exception e){
            Debug.Log(e);
            return readRoundFromFile(1);
        }
        
       
    }
}

class EnemySpawn{
    int tickTime = 0;
    GameObject prefab;

    public EnemySpawn(int time, GameObject unit){
        tickTime = time;
        prefab = unit;
    }

    public int getSpawnTime(){
        return tickTime;
    }

    public GameObject getUnit(){
        return prefab;
    }
}
