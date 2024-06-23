using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    protected void OnDisable()
    {
       // transform.SetParent(ObjectPooler.inst.transform);

        ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� 
        CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� 
    }
}
