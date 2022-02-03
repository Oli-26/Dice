using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Selectable
{
    public ButtonType type;
    public int value = 0;
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
       Grid.GetComponent<Effects>().Roll();
    }

}
