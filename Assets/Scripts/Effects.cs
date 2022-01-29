using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject escapeFlash;
    int flashTimer = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(escapeFlash.activeSelf && flashTimer > 0){
            flashTimer--;
        }else{
            escapeFlash.SetActive(false);
            flashTimer = 5;
        }
    }

    public void Flash(){
        escapeFlash.SetActive(true);
    }
}
