using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject escapeFlash;
    int flashTimer = 5;

    public Sprite[] RollAnimation;
    public GameObject rollEffect;
    public SpriteRenderer rollEffectRenderer;
    int rollFrame = 0;
    int frameTime = 0;

    void Start()
    {
        rollEffectRenderer = rollEffect.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(escapeFlash.activeSelf && flashTimer > 0){
            flashTimer--;
        }else{
            escapeFlash.SetActive(false);
            flashTimer = 5;
        }

        if(rollEffect.activeSelf){
            if(frameTime > 2){
                rollFrame++;
                if(rollFrame < RollAnimation.Length){
                    rollEffectRenderer.sprite = RollAnimation[rollFrame];
                }else{
                    rollEffect.SetActive(false);
                    rollFrame = 0;
                }

                frameTime = 0;
            }else{
                frameTime++;
            }
        }
    }

    public void Flash(){
        escapeFlash.SetActive(true);
    }

    public void Roll(){
        rollEffect.SetActive(true);
    }
}
