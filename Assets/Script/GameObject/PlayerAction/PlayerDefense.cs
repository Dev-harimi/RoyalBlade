using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : PlayerAction
{
    float defenseDelay;
    float defenseRange;
    float defenseCoolTime;
    float defensePower;
    float chargedSkillDuration;

    bool isDefensing;
    bool isCoolTime;
    bool isCharged;
    bool isUseSkill;

    PoolingObjectVFX defenseShieldVFX;

    public override void Init()
    {
        curChargeStack = 0;
        maxChargeStack = 4;

        defenseDelay = 0.1f;
        defenseRange = 2.5f;
        defenseCoolTime = 1f;
        defensePower = 30f;
        chargedSkillDuration = 5f;

        isDefensing = false;
        isCoolTime = false;
        isCharged = false;
        isUseSkill = false;
    }

    public override void OnClickActionButton()
    {
        Defense();
    }

    void Defense()
    {
        if (isDefensing || isCoolTime || isUseSkill)
            return;

        isDefensing = true;

        if (ActionEvent != null)
            ActionEvent.Invoke();

        StartCoroutine(DefenseCor());
        StartCoroutine(DefenseCoolTimeCor());

        IEnumerator DefenseCor()
        {
            player.anim.SetBool("Defense", true);

            float t = defenseDelay;
            while(t > 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;
            }

            if (defenseShieldVFX == null)
            {
                Vector3 preset = Vector3.up * 0.5f;
                defenseShieldVFX = ObjectPooler.SpawnFromPool("DefenseShieldVFX", player.transform.position + preset).GetComponent<PoolingObjectVFX>();
                defenseShieldVFX.FollowTarget(player.transform, preset);
            }

            RaycastHit2D hit;
            while(isDefensing)
            {
                hit = Physics2D.Raycast(player.transform.position, Vector2.up, defenseRange, 1 << 7);
                if(hit.collider != null)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    ObjectPooler.SpawnFromPool("DefenseSuccessVFX", enemy.transform.position).GetComponent<PoolingObjectVFX>().Timer(0.3f);
                    Rigidbody2D rb = hit.collider.GetComponent<Enemy>().group.rb;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * defensePower, ForceMode2D.Impulse);
                    ResetDefense();
                }

                yield return GlobalCache.update;
            }
        }
    }

    public void ResetDefense()
    {
        isDefensing = false;
        player.anim.SetBool("Defense", false);

        if (defenseShieldVFX != null)
        {
            defenseShieldVFX.gameObject.SetActive(false);
            defenseShieldVFX = null;
        }
    }

    IEnumerator DefenseCoolTimeCor()
    {
        isCoolTime = true;

        UIManager.instance.ButtonCoolTime(1, defenseCoolTime);

        float t = defenseCoolTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return GlobalCache.update;
        }

        isCoolTime = false;
    }

    public override void OnClickChargedAction()
    {
        base.OnClickChargedAction();
        StartCoroutine(SpawnBlueBarrier());

        IEnumerator SpawnBlueBarrier()
        {
            isUseSkill = true;
            GameObject blueBarrier = ObjectPooler.SpawnFromPool("BlueBarrier", player.transform.position + Vector3.up * 2f);

            float t = chargedSkillDuration;
            while(t > 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;
            }

            blueBarrier.gameObject.SetActive(false);
            isUseSkill = false;
        }
    }

    public override void ResetCharge()
    {
        CurChargeStack = 0;
        UIManager.instance.OnUpdateCharedGuage(1, 0, maxChargeStack);
        UIManager.instance.CloseChargedActionButton(1);
    }
}
