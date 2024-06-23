using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble
{
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;
    public PlayerWeapon weapon;

    int curHP;
    int maxHP;
    bool isDie;
    bool isLosingHeart;
    float pushPower;

    public int MaxHP { get; set; }
    public int CurHP
    {
        get => curHP;
        set
        {
            curHP = value;

            if (curHP <= 0)
                Die();
        }
    }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        SetData();

        void SetData()
        {
            transform.position = GameManager.instance.PlayerDefaultPos;

            maxHP = 3;
            curHP = maxHP;
            pushPower = 30f;
            isLosingHeart = false;
            isDie = false;
            weapon = new PlayerWeapon();
            weapon.ExchangeWeapon(1);
        }
    }

    public void LoseHeart(Enemy attacker)
    {
        if (isLosingHeart || isDie)
            return;

        isLosingHeart = true;
        StartCoroutine(RecoveryBool());
        PushEnemyGroup(attacker);

        TakeDamage(1f);
        UIManager.instance.OnUpdatePlayerHeart(curHP);


        IEnumerator RecoveryBool()
        {
            yield return new WaitForSeconds(0.1f);
            isLosingHeart = false;
        }
    }

    void PushEnemyGroup(Enemy enemy)
    {
        Rigidbody2D rb = enemy.group.rb;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * pushPower, ForceMode2D.Impulse);
    }

    public void TakeDamage(float damage)
    {
        CurHP -= (int)damage;
    }

    public void Die()
    {
        if (!isDie)
        {
            isDie = true;
            GameManager.instance.GameOver();
        }
    }
}
