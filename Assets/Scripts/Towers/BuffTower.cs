using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffTower : Tower
{
    new void Start()
    {
        peaceful = true;
        base.Start();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void UsePower(){
        if(diceNumber < 1){
            return;
        }

        GameObject[] towersInRange = GameObject.FindGameObjectsWithTag("Tower").Where(t => Vector3.Distance(_transform.position, t.transform.position) < range).ToArray();
        List<GameObject> buffedTowers = new List<GameObject>();
        int whileCheck = 0;

        while(whileCheck < 1000 && diceNumber > 0){
            int random = Random.Range(0, towersInRange.Length);
            if(buffedTowers.Count == towersInRange.Length){
                UpdateNumberSprite();
                return;
            }
            if(!buffedTowers.Contains(towersInRange[random])){
                buffedTowers.Add(towersInRange[random]);
                if(towersInRange[random] != gameObject && towersInRange[random].GetComponent<Tower>().Buff()){
                    diceNumber--;
                }
                
            }

            whileCheck++;
            if(whileCheck == 1000){
                Debug.Log("Stopped a while loop running away");
            }
        }

        UpdateNumberSprite();
    }

    public override void DePower(){
        _transform.GetChild(1).gameObject.SetActive(true);
    }
}
