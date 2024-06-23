using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon
{
    public enum WEAPON
    {
        NormalSword,
        RareSword,
        SpecialSword
    }

    WEAPON Weapon;

    float attackDamage;
    float attackAnimDelay;
    float attackCoolTime;
    float attackRange;
    float attackCriticalRatio;

    public float AttackDamage { get => attackDamage; }
    public float AttackAnimDelay { get => attackAnimDelay; }
    public float AttackCoolTime { get => attackCoolTime; }
    public float AttackRange { get => attackRange; }
    public float AttackCriticalRatio { get => attackCriticalRatio; }

    public void ExchangeWeapon(int weaponIndex)
    {
        SetWeaponData(weaponIndex);
    }

    void SetWeaponData(int weaponIndex)
    {
        if (Weapon == (WEAPON)weaponIndex)
            return;

        Weapon = (WEAPON)weaponIndex;

        switch (Weapon)
        {
            case WEAPON.NormalSword:
                {
                    attackDamage = 100;
                    attackAnimDelay = 0.1f;
                    attackCoolTime = 0.15f;
                    attackRange = 3f;
                    attackCriticalRatio = 0.2f;
                }
                break;

            case WEAPON.RareSword:
                {
                    attackDamage = 1000f;
                    attackAnimDelay = 0.1f;
                    attackCoolTime = 0.15f;
                    attackRange = 5f;
                    attackCriticalRatio = 0.2f;
                }
                break;
            case WEAPON.SpecialSword:
                {
                    attackDamage = 10000f;
                    attackAnimDelay = 0.1f;
                    attackCoolTime = 0.05f;
                    attackRange = 10f;
                    attackCriticalRatio = 0.5f;
                }
                break;
        }
    }
}
