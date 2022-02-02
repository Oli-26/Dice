using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Selectable
{
    public float damage = 1f;
    public float shotSpeed = 0.1f;
    float nextShot = 0;
    public float shotCoolDown = 1f;
    public float range = 5f;
    public int diceNumber = 1;
    public GameObject shotPrefab;
    public Sprite[] towerSprites;

    public int cost = 25;
    SpriteRenderer _renderer;
    Targeting _targeting;
    RoundManager _roundManager;

    void Awake(){
        _renderer = GetComponent<SpriteRenderer>();
        _targeting = GetComponent<Targeting>();
        _roundManager = GameObject.FindWithTag("Grid").GetComponent<RoundManager>();
    }
    void Start()
    {
        
    }

    protected void FixedUpdate()
    {
        if(nextShot < 0 && _roundManager.IsRoundActive()){
            Attack();
        }else{
            nextShot -= Time.deltaTime;
        }
    }

    void Attack(){
        _targeting.Retarget(range);

        if(_targeting.TargetIsSet()){
            Debug.Log("Shooting");
            nextShot = shotCoolDown;
            _targeting.GetTarget().GetComponent<Unit>().TargetedForDamage(damage);
            Shot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<Shot>();
            shot.Init(_targeting.GetTarget(), damage, shotSpeed);
        }
    }

    public void ReRoll(){
        int random = Random.Range(0, 5);
        diceNumber = random + 1;
        _renderer.sprite = towerSprites[random];
    }

    public override void OnClick(){
        Debug.Log("Not implemented");
    }
}
