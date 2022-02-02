using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public int x;
    public int y;
    public GameObject squareGameObject;
    bool resetOpacity = true;
    SpriteRenderer _renderer;

    void Awake(){
        
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(resetOpacity){
            _renderer.color = new Color(1f,1f,1f,0f);
            resetOpacity = false;
        }
        
        resetOpacity = true;

    }

    public void Initialise(int x, int y, GameObject parent, Sprite sprite){
        this.x = x;
        this.y = y;
        gameObject.name = x + "-" + y;
        gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0f);
        gameObject.transform.parent = parent.transform;
        gameObject.AddComponent<SpriteRenderer>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = sprite;
        _renderer.color = new Color(1f,1f,1f,0f);
    }

    public void HighLight(){
        _renderer.color = new Color(1f, 1f, 1f, 0.25f);
        resetOpacity = false;
    }
}
