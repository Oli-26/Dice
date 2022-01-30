using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    Vector3 mousePosition;
    public List<GameObject> allSelectableObjects;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if(Input.GetMouseButtonDown(0)){
            Click();
        }

        HighlightGridPosition();
    }

    void HighlightGridPosition(){
            GetComponent<GridSystem>().HighLightGridPosition(mousePosition);
    }

    void Click(){
        foreach(GameObject selectableObject in allSelectableObjects){
            SpriteRenderer renderer = selectableObject.GetComponent<SpriteRenderer>();
            float halfWidth = renderer.bounds.size.x/2;
            float halfHeight = renderer.bounds.size.y/2;

            if(InRectangle(selectableObject.transform.position, halfWidth, halfHeight)){
                if(selectableObject.GetComponent<Selectable>().IsUIElement){

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
}
