using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : PlayerAction
{
    float chargeDuration;

    bool isAttacking;
    bool isCharged;
    bool isUseSkill;

    RaycastHit2D[] hit;
    
    public override void Init()
    {
        curChargeStack = 0;
        maxChargeStack = 20;

        chargeDuration = 3f;

        isAttacking = false;
        isCharged = false;
        isUseSkill = false;
    }

    public override void OnClickActionButton()
    {
        Attack();
    }

    void Attack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (ActionEvent != null)
            ActionEvent.Invoke();

        StartCoroutine(AttackCor());
        StartCoroutine(AttackCoolTimeCor());
    }

    IEnumerator AttackCor()
    {
        player.anim.SetTrigger("Attack");

        float t = player.weapon.AttackAnimDelay;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return GlobalCache.update;
        }

        AttackVFX();
        hit = Physics2D.RaycastAll(player.transform.position, Vector2.up, player.weapon.AttackRange, 1 << 7);

        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Enemy enemy = hit[i].collider.GetComponent<Enemy>();
    
                if (enemy != null)
                {
                    int randomN = Random.Range(1, 101);
                    bool isCritical;
  
                    if (randomN < (int)(player.weapon.AttackCriticalRatio * 100f))
                        isCritical = true;
                    else
                        isCritical = false;
 
                    float damage = isCritical ? player.weapon.AttackDamage * 2f : player.weapon.AttackDamage;
                    enemy.TakeDamage(damage);
                    UIManager.instance.FloatingDamageText(enemy.transform.position, damage, isCritical ? Color.red : Color.white);
                }
            }

            hit = null;
        }
    }

    IEnumerator AttackCoolTimeCor()
    {
        UIManager.instance.ButtonCoolTime(2, player.weapon.AttackCoolTime);

        float t = player.weapon.AttackCoolTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return GlobalCache.update;
        }

        isAttacking = false;
    }

    public override void OnClickChargedAction()
    {
        base.OnClickChargedAction();
        ChargedAttack();
    }

    void AttackVFX()
    {
        if(isUseSkill)
            ObjectPooler.SpawnFromPool("ClawAttackVFX", player.transform.position + new Vector3(Random.Range(-1f, 1f), 7.5f)).GetComponent<PoolingObjectVFX>().Timer(0.5f);
        else
            ObjectPooler.SpawnFromPool("Slash", player.transform.position + Vector3.left * 0.5f + Vector3.up * 1.5f).GetComponent<PoolingObjectVFX>().Timer(0.5f);
    }

    void ChargedAttack()
    {
        StartCoroutine(ChargedAttackCor());

        IEnumerator ChargedAttackCor()
        {
            isUseSkill = true;
            player.weapon.ExchangeWeapon(2);

            float t = chargeDuration;
            while (t >= 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;
            }

            player.weapon.ExchangeWeapon(1);
            isUseSkill = false;
        }
    }

    public override void ResetCharge()
    {
        CurChargeStack = 0;
        UIManager.instance.OnUpdateCharedGuage(2, 0, maxChargeStack);
        UIManager.instance.CloseChargedActionButton(2);
    }
}
