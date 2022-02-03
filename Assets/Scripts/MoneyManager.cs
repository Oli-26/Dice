using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int money;
    public UIManager UIManager;

    void Awake(){
        UIManager = GetComponent<UIManager>();
    }

    void Start()
    {
        money= 50;
    }


    void Update()
    {
        
    }

    public void GainMoney(int amount){
        money += amount;
        UpdateUI();
    }

    public void SpendMoney(int amount){
        money -= amount;
        UpdateUI();
    }

    public bool CheckFunds(int amount){
        return money >= amount;
    }

    public void UpdateUI(){
        UIManager.UpdateMoneyUI(money);
    }
}
