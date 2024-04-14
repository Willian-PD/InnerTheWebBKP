using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private bool attackButtonDown, isAttacking;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Combat.Attack.started += _ => attackButtonDown = true;
        playerControls.Combat.Attack.canceled += _ => attackButtonDown = false;
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
        AttackCoolDown();
    }

    private void AttackCoolDown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCoolDown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
