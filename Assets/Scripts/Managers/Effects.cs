using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject escapeFlash;
    int flashTimer = 5;
    public GameObject reRollAnimation;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if(escapeFlash.activeSelf && flashTimer > 0){
            flashTimer--;
        }else{
            escapeFlash.SetActive(false);
            flashTimer = 5;
        }
    }

    public void RollAt(Vector3 position){
        Instantiate(reRollAnimation, position, Quaternion.identity);
    }

    public void Flash(){
        escapeFlash.SetActive(true);
    }
}
