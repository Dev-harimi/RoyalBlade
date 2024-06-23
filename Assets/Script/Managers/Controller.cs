using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public Player player;

    PlayerAttack playerAttack;
    PlayerJump playerJump;
    PlayerDefense playerDefense;

    private void Awake()
    {
        InitAction();
        AddOnClickPlayerActionEvent();
        AddActionEvent();
        AddChargedCompleteEvent();
        InitOnClickChargedActionEvent();
    }

    void InitAction()
    {
        playerAttack = gameObject.AddComponent<PlayerAttack>();
        playerJump = gameObject.AddComponent<PlayerJump>();
        playerDefense = gameObject.AddComponent<PlayerDefense>();
    }

    void AddOnClickPlayerActionEvent()
    {
        UIManager.instance.AddOnClickActionEvent(0, playerJump);
        UIManager.instance.AddOnClickActionEvent(1, playerDefense);
        UIManager.instance.AddOnClickActionEvent(2, playerAttack);
    }

    void AddActionEvent()
    {
        playerJump.ActionEvent += () => playerJump.CurChargeStack++;
        playerJump.ActionEvent += () => UIManager.instance.OnUpdateCharedGuage(0, playerJump.CurChargeStack, playerJump.MaxChargeStack);

        playerDefense.ActionEvent += () => playerDefense.CurChargeStack++;
        playerDefense.ActionEvent += () => UIManager.instance.OnUpdateCharedGuage(1, playerDefense.CurChargeStack, playerDefense.MaxChargeStack);

        playerAttack.ActionEvent += playerDefense.ResetDefense;
        playerAttack.ActionEvent += () => playerAttack.CurChargeStack++;
        playerAttack.ActionEvent += () => UIManager.instance.OnUpdateCharedGuage(2, playerAttack.CurChargeStack, playerAttack.MaxChargeStack);
    }

    void AddChargedCompleteEvent()
    {
        playerJump.ChargedEvent += () => { UIManager.instance.OpenChargedActionButton(0); };
        playerDefense.ChargedEvent += () => { UIManager.instance.OpenChargedActionButton(1); };
        playerAttack.ChargedEvent += () => { UIManager.instance.OpenChargedActionButton(2); };
    }

    void InitOnClickChargedActionEvent()
    {
        UIManager.instance.AddOnClickChargedActionEvent(0, playerJump);
        UIManager.instance.AddOnClickChargedActionEvent(1, playerDefense);
        UIManager.instance.AddOnClickChargedActionEvent(2, playerAttack);
    }
}
