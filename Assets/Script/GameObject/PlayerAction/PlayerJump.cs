using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerAction
{
    GroundChecker groundChecker;

    float jumpDelay;
    float jumpPower;
    float jumpCoolTime;
    float flashJumpPower;
    float flashJumpDuration;
    float flashRange;

    bool isJumpCoolTime;
    bool isCharged;
    bool isUseSkill;

    public override void Init()
    {
        groundChecker = player.gameObject.GetComponent<GroundChecker>();

        curChargeStack = 0;
        maxChargeStack = 4;

        jumpDelay = 0.15f;
        jumpPower = 30f;
        jumpCoolTime = 1f;
        flashJumpPower = 60f;
        flashJumpDuration = 2f;
        flashRange = 4f;

        isJumpCoolTime = false;
        isCharged = false;
        isUseSkill = false;
    }

    public override void OnClickActionButton()
    {
        Jump();
    }

    public override void OnClickChargedAction()
    {
        base.OnClickChargedAction();
        StartCoroutine(FlashJump());

        IEnumerator FlashJump()
        {
            isUseSkill = true;
            StartCoroutine(FlashVFX());

            float t = jumpDelay;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;
            }

            player.anim.SetTrigger("Jump");
            ObjectPooler.SpawnFromPool("JumpVFX", player.transform.position + Vector3.down * 0.5f, Quaternion.Euler(new Vector3(0, 0, 100f))).GetComponent<PoolingObjectVFX>().Timer(0.5f);
            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(Vector2.up * flashJumpPower, ForceMode2D.Impulse);

            float tt = flashJumpDuration;
            RaycastHit2D hit;
            while (tt > 0)
            {
                tt -= Time.deltaTime;

                hit = Physics2D.Raycast(player.transform.position, Vector2.up, flashRange, 1 << 7);
                if (hit.collider != null)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.TakeDamage(10000f);
                }

                yield return GlobalCache.update;
            }

            isUseSkill = false;
        }

        IEnumerator FlashVFX()
        {
            yield return new WaitForSeconds(0.2f);

            if (isUseSkill)
            {
                ObjectPooler.SpawnFromPool("FlashVFX", player.transform.position + Vector3.up * flashRange);
                StartCoroutine(FlashVFX());
            }
        }
    }

    void Jump()
    {
        if (isJumpCoolTime || !groundChecker.isGround)
            return;

        isJumpCoolTime = true;

        if (ActionEvent != null)
            ActionEvent.Invoke();

        StartCoroutine(JumpCor());
        StartCoroutine(JumpCoolTimeCor());

        IEnumerator JumpCor()
        {
            float t = jumpDelay;
            while(t > 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;
            }

            player.anim.SetTrigger("Jump");
            ObjectPooler.SpawnFromPool("JumpVFX", player.transform.position + Vector3.down * 0.5f, Quaternion.Euler(new Vector3(0, 0, 100f))).GetComponent<PoolingObjectVFX>().Timer(0.5f);
            player.rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        IEnumerator JumpCoolTimeCor()
        {
            UIManager.instance.ButtonCoolTime(0, jumpCoolTime);

            float t = jumpCoolTime;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;
            }

            isJumpCoolTime = false;
        }
    }

    public override void ResetCharge()
    {
        CurChargeStack = 0;
        UIManager.instance.OnUpdateCharedGuage(0, 0, maxChargeStack);
        UIManager.instance.CloseChargedActionButton(0);
    }
}
