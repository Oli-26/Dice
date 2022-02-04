using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Selectable
{
    protected bool peaceful;
    public float damage = 1f;
    public float shotSpeed = 0.1f;
    protected float nextShot = 0;
    public float shotCoolDown = 1f;
    public float range = 5f;
    public int diceNumber = 1;
    public GameObject shotPrefab;
    public Sprite[] towerSprites;
    public Sprite defaultSprite;
    protected bool buffed = false;
    public int cost = 25;
    protected SpriteRenderer _renderer;
    protected Targeting _targeting;
    protected RoundManager _roundManager;
    protected Effects _effects;

    protected Transform _transform;

    void Awake(){
        _renderer = GetComponent<SpriteRenderer>();
        _targeting = GetComponent<Targeting>();
        _roundManager = GameObject.FindWithTag("Grid").GetComponent<RoundManager>();
        _effects = GameObject.FindWithTag("Grid").GetComponent<Effects>();
        _transform = transform;
    }
    protected void Start()
    {
        _transform.GetChild(0).localScale = new Vector3(range*0.67f, range*0.67f, 1f);
    }

    protected void FixedUpdate()
    {
        if(!peaceful && nextShot < 0 && _roundManager.IsRoundActive()){
            Attack();
        }else{
            nextShot -= Time.deltaTime;
        }
    }

    protected virtual void Attack(){
        _targeting.Retarget(range);

        if(_targeting.TargetIsSet()){
            nextShot = shotCoolDown * (1f * Mathf.Pow(0.85f, diceNumber));
            _targeting.GetTarget().GetComponent<Unit>().TargetedForDamage(damage);
            Shot shot = Instantiate(shotPrefab, transform.position, Quaternion.identity).GetComponent<Shot>();
            shot.Init(_targeting.GetTarget(), damage, shotSpeed);
        }
    }

    public void ReRoll(){
        int random = Random.Range(0, 5);
        diceNumber = random + 1;
        UpdateNumberSprite();
        _effects.RollAt(_transform.position);
        buffed = false;
    }

    public bool isBuffed(){
        return buffed;
    }

    public bool Buff(){
        if(buffed || diceNumber == 6){
            return false;
        }

        diceNumber = diceNumber + 1;
        buffed = true;
        UpdateNumberSprite();
        _effects.RollAt(_transform.position);
        return true;
    }

    protected void UpdateNumberSprite(){
        if(diceNumber == 0){
            _renderer.sprite = defaultSprite;
            return;
        }
        _renderer.sprite = towerSprites[diceNumber-1];
    }

    public override void OnClick(){
        GameObject.FindWithTag("Grid").GetComponent<UIManager>().OpenSelectedMenu();
        GameObject.FindWithTag("Grid").GetComponent<UIManager>().UpdateSelectedMenu(gameObject, defaultSprite);
        ShowRange();
    }

    protected void ShowRange(){
        _transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideRange(){
        _transform.
        GetChild(0).gameObject.SetActive(false);
    }

    public virtual void UsePower(){

    }

    public virtual void DePower(){

    }
}
