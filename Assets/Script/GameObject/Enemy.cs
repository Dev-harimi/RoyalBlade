using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageAble
{
    [HideInInspector]
    public EnemyGroup group;

    [HideInInspector]
    public SpriteRenderer sr;
    [HideInInspector]
    public Animator anim;

    int goldReward;
    float scoreReward;
    float curHP;
    float maxHP;
    bool isDie;

    UnityAction dieEvent;

    public float CurHP
    {
        get => curHP;
        set
        {
            curHP = value;

            if (curHP <= 0)
                Die();
        }
    }

    public float MaxHP { get => maxHP; set => maxHP = value; }
    public float ScoreReward { get => scoreReward; set => scoreReward = value; }
    public int GoldReward { get => goldReward; set => goldReward = value; }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        SetDieEvent();
    }

    void OnEnable()
    {
        isDie = false;
    }

    void SetDieEvent()
    {
        dieEvent = null;
        dieEvent += () =>
        {
            ObjectPooler.SpawnFromPool("EnemyDieVFX", transform.position, Quaternion.Euler(new Vector3(90f, 0, 0))).GetComponent<PoolingObjectVFX>().Timer(0.5f);

            GameManager.instance.Score += ScoreReward;
            GameManager.instance.Gold += GoldReward;
            group.EnemyCount--;
            gameObject.SetActive(false);
        };
    }

    public void SetHP(float HP)
    {
        maxHP = HP;
        curHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        CurHP -= damage;
    }

    public void Die()
    {
        if (!isDie)
        {
            isDie = true;

            if(dieEvent != null)
            dieEvent.Invoke();
        }
    }
}
