using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    Vector3 mousePosition;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        HighlightGridPosition();
    }

    void HighlightGridPosition(){
            GetComponent<GridSystem>().HighLightGridPosition(mousePosition);
        
    }
}
