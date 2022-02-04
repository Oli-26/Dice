using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Selectable
{
    public ButtonType type;
    public int value = 0;
    public GameObject attatchedTo;
    public GameObject Grid;
    void Start()
    {
        Grid = GameObject.FindWithTag("Grid");
    }

    void Update()
    {
        
    }

    public override void OnClick(){
        if(!IsActive){
            return;
        }

        switch(type){
            case ButtonType.BuyTower:
                BuyTower();
                break;
            case ButtonType.StartRound:
                StartRound();
                break;
            case ButtonType.TowerPower:
                UsePower();
                break;
            default:
                break;
        }   
    }

    void BuyTower(){
        Grid.GetComponent<BuildManager>().PurchaseTower(value);
    }

    void StartRoundWrapper(){
        Invoke("StartRound", 4f);
    }

    void StartRound(){
       Grid.GetComponent<RoundManager>().StartRound(); 
    }

    void UsePower(){
        if(attatchedTo.GetComponent<Tower>() != null){
            attatchedTo.GetComponent<Tower>().UsePower();
        }
    }

    public void Attatch(GameObject attatch){
        attatchedTo = attatch;
    }

}
