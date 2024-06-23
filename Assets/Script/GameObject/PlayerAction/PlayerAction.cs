using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerAction : MonoBehaviour
{
    protected Player player;
    protected int curChargeStack;
    protected int maxChargeStack;
    private UnityAction chargedEvent;
    private UnityAction actionEvent;

    public int CurChargeStack
    {
        get => curChargeStack;
        set
        {
            curChargeStack = value;

            if (curChargeStack >= maxChargeStack)
            {
                if (ChargedEvent != null)
                    ChargedEvent.Invoke();
            }
        }
    }
    public int MaxChargeStack { get => maxChargeStack; set => maxChargeStack = value; }
    public UnityAction ChargedEvent { get => chargedEvent; set => chargedEvent = value; }
    public UnityAction ActionEvent { get => actionEvent; set => actionEvent = value; }

    protected void Awake()
    {
        player = GetComponent<Controller>().player;
    }

    protected void Start()
    {
        Init();
    }

    abstract public void Init();
     
    abstract public void OnClickActionButton();

    virtual public void OnClickChargedAction()
    {
        ResetCharge();
    }

    abstract public void ResetCharge();
}
