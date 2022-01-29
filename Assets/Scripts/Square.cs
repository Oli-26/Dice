using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : ScriptableObject
{
    public int x;
    public int y;
    public GameObject squareGameObject;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialise(int x, int y, GameObject parent){
        this.x = x;
        this.y = y;
        squareGameObject = new GameObject();
        squareGameObject.name = x + "-" + y;
        squareGameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0f);
        squareGameObject.transform.parent = parent.transform;
    }
}
