using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    // Selected menu stuff
    public GameObject selectedMenu;
    bool selectedMenuOpen = false;
    Vector3 selectedMenuPosition;
    bool selectedMenuMoving;

    // Money indicator
    public GameObject moneyIndicator;
    TMP_Text moneyIndicatorText;
    SpriteRenderer moneyIndicatorRenderer;
    int moneyFlash;

    void Start()
    {
        selectedMenuPosition = selectedMenu.transform.position;
        moneyIndicatorText = moneyIndicator.GetComponent<TextMeshPro>();
        moneyIndicatorRenderer = moneyIndicator.transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(selectedMenuMoving){
            if(Vector3.Distance(selectedMenu.transform.position, selectedMenuPosition) <= 0.01f){
                selectedMenuMoving = false;
            }else{
                selectedMenu.transform.position = Vector3.MoveTowards(selectedMenu.transform.position, selectedMenuPosition, 0.14f);
            }
        }

        if(moneyFlash > 0){
            moneyFlash--;
        }else if(moneyFlash == 0){
            moneyIndicatorRenderer.color = new Color (1f, 1f, 1f, 1f);
            moneyIndicatorText.color = new Color (1f, 1f, 1f, 1f);
            moneyFlash = -1;
        }
    }

    public void OpenSelectedMenu(){
        if(!selectedMenuOpen){
            selectedMenuPosition = selectedMenuPosition + new Vector3(-4.35f, 0f, 0f);
            selectedMenuOpen = !selectedMenuOpen;
            selectedMenuMoving = true;
        }
    }

    public void CloseSelectedMenu(){
        if(selectedMenuOpen){
            selectedMenuPosition = selectedMenuPosition + new Vector3(4.35f, 0f, 0f);
            selectedMenuOpen = !selectedMenuOpen;
            selectedMenuMoving = true;
        }
    }
    
    public void UpdateSelectedMenu(GameObject tower, Sprite towerSprite){
        GameObject.Find("UsePower").GetComponent<Button>().Attatch(tower);
        selectedMenu.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = towerSprite;
    }

    public void UpdateMoneyUI(int amount){
        if(amount > 0){
            moneyIndicatorRenderer.color = new Color (0.8f, 0.8f, 0.1f, 1f);
            moneyIndicatorText.color = new Color (0.8f, 0.8f, 0.1f, 1f);
            moneyFlash = 15;
        }

        moneyIndicatorText.text = amount+"";
        
    }
}
