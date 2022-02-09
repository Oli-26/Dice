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
            case ButtonType.ChangeTargeting:
                ChangeTargeting();
                break;
            case ButtonType.SellTower:
                SellTower();
                break;
            default:
                break;
        }   
    }

    void SellTower(){
        if(attatchedTo.GetComponent<Tower>() != null){
            Grid.GetComponent<BuildManager>().SellTower(attatchedTo);
            Grid.GetComponent<UIManager>().CloseSelectedMenu();
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

    void ChangeTargeting(){
        if(attatchedTo.GetComponent<Targeting>() != null){
            value++;
            if(value > 2){
                value = 0;
            }
            
            attatchedTo.GetComponent<Targeting>().SetTargetingMode(value);
            Object[] sprites = Resources.LoadAll("ButtonSprites/arrow");
            GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[value+1];
        }
    }

    public void Attatch(GameObject attatch){
        attatchedTo = attatch;
    }
}
