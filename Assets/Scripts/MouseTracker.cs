using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseTracker : MonoBehaviour
{
    Vector3 mousePosition;
    public List<GameObject> allSelectableObjects;
    Camera mainCamera;
    GridSystem gridSystem;
    UIManager UIManager;
    GameObject selectedObject;

    void Awake(){
        mainCamera = Camera.main;
        gridSystem = GetComponent<GridSystem>();
        UIManager = GetComponent<UIManager>();
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

    }



    void Click(){
        if(selectedObject != null){
            Tower tower = selectedObject.GetComponent<Tower>();
            if(tower != null){
                tower.HideRange();
            }
        }
        
        foreach(GameObject selectableObject in allSelectableObjects){
            SpriteRenderer renderer = selectableObject.GetComponent<SpriteRenderer>();
            float halfWidth = renderer.bounds.size.x/2;
            float halfHeight = renderer.bounds.size.y/2;

            if(InRectangle(selectableObject.transform.position, halfWidth, halfHeight)){
                selectedObject = selectableObject;
                if(selectableObject.GetComponent<Selectable>().IsUIElement){
                    selectedObject.GetComponent<Selectable>().OnClick();
                }else{
                    selectedObject.GetComponent<Selectable>().OnClick();
                }

                return;
            }
        }
        UIManager.CloseSelectedMenu();

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

    public void AddSelectableObject(GameObject obj){
        allSelectableObjects.Add(obj);
    }

    public void RemoveSelectableObject(GameObject obj){
        GameObject[] toRemove = new GameObject[]{obj};
        IEnumerable<GameObject> newList = allSelectableObjects.Except(toRemove);
        allSelectableObjects = newList.ToList();
    }
}
