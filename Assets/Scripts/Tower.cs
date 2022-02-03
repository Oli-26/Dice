using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Selectable
{
    public float damage = 1f;
    public float shotSpeed = 0.1f;
    protected float nextShot = 0;
    public float shotCoolDown = 1f;
    public float range = 5f;
    public int diceNumber = 1;
    public GameObject shotPrefab;
    public Sprite[] towerSprites;
    public Sprite defaultSprite;

    public int cost = 25;
    protected SpriteRenderer _renderer;
    protected Targeting _targeting;
    protected RoundManager _roundManager;

    protected Transform _transform;

    void Awake(){
        _renderer = GetComponent<SpriteRenderer>();
        _targeting = GetComponent<Targeting>();
        _roundManager = GameObject.FindWithTag("Grid").GetComponent<RoundManager>();
        _transform = transform;
    }
    protected void Start()
    {
        _transform.GetChild(0).localScale = new Vector3(range*0.67f, range*0.67f, 1f);
    }

    protected void FixedUpdate()
    {
        if(nextShot < 0 && _roundManager.IsRoundActive()){
            Attack();
        }else{
            nextShot -= Time.deltaTime;
        }
    }

    protected void Attack(){
        _targeting.Retarget(range);

        if(_targeting.TargetIsSet()){
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
        GameObject.FindWithTag("Grid").GetComponent<UIManager>().OpenSelectedMenu();
        GameObject.FindWithTag("Grid").GetComponent<UIManager>().UpdateSelectedMenu(defaultSprite);
        ShowRange();
    }

    protected void ShowRange(){
        _transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideRange(){
        _transform.GetChild(0).gameObject.SetActive(false);
    }
}
