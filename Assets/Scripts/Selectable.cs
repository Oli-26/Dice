using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public bool IsUIElement = false;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    public abstract void OnClick();
}
