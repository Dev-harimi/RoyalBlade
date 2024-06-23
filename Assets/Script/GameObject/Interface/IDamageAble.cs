using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAble
{
    void TakeDamage(float damage);

    void Die();
}
