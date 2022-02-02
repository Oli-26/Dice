using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    Vector3 mousePosition;
    public List<GameObject> allSelectableObjects;
    Camera mainCamera;
    GridSystem gridSystem;

    void Awake(){
        mainCamera = Camera.main;
        gridSystem = GetComponent<GridSystem>();
    }
    void Start()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("UISelectable")){
            allSelectableObjects.Add(obj);
        }
        
    }

    void Update()
    {
        mousePosition = GetMousePosition();

        if(Input.GetMouseButtonDown(0)){
            Click();
        }

        HighlightGridPosition();
    }

    void HighlightGridPosition(){
        gridSystem.HighLightGridPosition(mousePosition);
    }

    void Click(){
        foreach(GameObject selectableObject in allSelectableObjects){
            SpriteRenderer renderer = selectableObject.GetComponent<SpriteRenderer>();
            float halfWidth = renderer.bounds.size.x/2;
            float halfHeight = renderer.bounds.size.y/2;
            

            if(InRectangle(selectableObject.transform.position, halfWidth, halfHeight)){
                if(selectableObject.GetComponent<Selectable>().IsUIElement){
                    selectableObject.GetComponent<Selectable>().OnClick();
                }else{

                }
                return;
            }
        }
    }

    bool InRectangle(Vector3 objectPos, float halfWidth, float halfHeight){
        if(mousePosition.x > objectPos.x - halfWidth && mousePosition.x < objectPos.x + halfWidth){
            if(mousePosition.y > objectPos.y - halfHeight && mousePosition.y < objectPos.y + halfHeight){
                return true;
            }
        }
        return false;
    }

    public Vector3 GetMousePosition(){
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        return position;
    }
}
