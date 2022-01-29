using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    void Start()
    {
        Grid = GameObject.FindWithTag("Grid").GetComponent<GridSystem>();
        SetUpRound1();
    }

    void Update()
    {
        if(Input.GetKeyDown("s")){
            roundTick = 0;
            SpawnsListPosition = 0;
            roundActive = true;
        }
        if(roundActive){
            if(SpawnsListPosition < roundSpawns.Count - 1){
                while(roundTick >= roundSpawns[SpawnsListPosition].getSpawnTime()){
                    SpawnUnit();
                    if(SpawnsListPosition >= roundSpawns.Count){
                        break;
                    }
                }
            }else{
                roundActive = false;
            }
            roundTick++;
        }

    }

    void SpawnUnit(){
        aliveEnemies.Add((true, Instantiate(roundSpawns[SpawnsListPosition].getUnit(), Grid.GetNthSquareOnPath(0), Quaternion.identity)));
        SpawnsListPosition++;
    }

    public void KillUnit(GameObject unit){
        EnemyRecentlyDied = true;
        int index = aliveEnemies.IndexOf((true, unit));
        aliveEnemies[index] = (false, null);
        Destroy(unit);
    }

    void SetUpRound1(){
        roundSpawns = new List<EnemySpawn>();
        for(int i = 0; i < 10; i++){
            roundSpawns.Add(new EnemySpawn(i*30, unitPrefabs[1]));
            
        }
        roundSpawns.Add(new EnemySpawn(45, unitPrefabs[0]));
        roundSpawns.Add(new EnemySpawn(75, unitPrefabs[0]));
        roundSpawns.Sort((a, b) => a.getSpawnTime().CompareTo(b.getSpawnTime()));
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
