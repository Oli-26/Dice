using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    int frame;
    int frameTime;
    SpriteRenderer _renderer;

    void Awake(){
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        frame = 0;
    }

    void FixedUpdate()
    {
        if(frameTime > 2){
            frame++;
            frameTime = 0;
            if(frame < sprites.Length){
                _renderer.sprite = sprites[frame];
            }else{
                Destroy(gameObject);
            }
        }else{
            frameTime++;
        }
        
    }
}
