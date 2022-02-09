using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public bool IsUIElement = false;
    public bool IsActive = true;
    public int layer = 0;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToggleLayer(int l){
        if(layer == l){
            IsActive = !IsActive;
        }
    }

    public void DisableLayer(int l){
        if(layer == l){
            IsActive = false;
        }
    }

    public void EnableLayer(int l){
        if(layer == l){
            IsActive = true;
        }
    }
    
    public abstract void OnClick();
}
