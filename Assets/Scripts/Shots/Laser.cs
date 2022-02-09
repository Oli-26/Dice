using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    List<int> damagedEnemies;
    RoundManager _roundManager;
    SpriteRenderer _spriteRenderer;
    Transform _transform;

    float damage = 1f;

    void Awake(){
        _roundManager = GameObject.Find("Grid").GetComponent<RoundManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
        damagedEnemies = new List<int>();
    }

    public void Init(int diceDifference){
        damage = 0.5f + 0.4f*diceDifference;
        _transform.localScale = new Vector3(_transform.localScale.x * (0.35f * (diceDifference + 1)), _transform.localScale.y, _transform.localScale.z);
    }

    void Start()
    {
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject enemy = col.gameObject;
        if(enemy == null || enemy.GetComponent<Unit>() == null){
            return;
        }
        if(!damagedEnemies.Contains(enemy.GetInstanceID())){
            damagedEnemies.Add(enemy.GetInstanceID());
            enemy.GetComponent<Unit>().TakeLaserDamage(damage);
            
        }
       
    }
}
