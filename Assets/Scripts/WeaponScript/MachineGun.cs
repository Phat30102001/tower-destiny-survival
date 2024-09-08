using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class MachineGun : MonoBehaviour, IWeapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private string weaponId;
    private Func<Vector2> onGetNearestEnemy;

    private MachineGunData weaponData;
    private MachineGunSkillData skillData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public bool isTriggerSkill = false;
    CoroutineHandle handle;
    private bool isRunHandle = false;
    public void SetData(WeaponBaseData _data)
    {
        if (_data is MachineGunData _weaponData)
        {
            weaponData = _weaponData;
            skillData = _weaponData.SkillData;
        }
    }

    public string GetWeaponId()
    {
        return weaponId;
    }

    public IEnumerator<float> ActiveWeapon()
    {
        gameObject.SetActive(true);
        while (gameObject.activeSelf)
        {
            if (isTriggerSkill && !isRunHandle)
            {
                handle = Timing.RunCoroutine(ActivateSkill());
                isRunHandle = true;
                Debug.Log($"active skill {weaponId} at{weaponData.Uid}");
            }
            Vector2 _target = onGetNearestEnemy();
            float angle = Mathf.Atan2(_target.y, _target.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Fire(_target);
            yield return Timing.WaitForSeconds(weaponData.Cooldown);
        }
    }
    private IEnumerator<float> ActivateSkill()
    {
        float originalCooldown = weaponData.Cooldown;
        int originalDamageAmount = weaponData.DamageAmount;

        // Use skill data
        weaponData.Cooldown = skillData.Cooldown;
        weaponData.DamageAmount = skillData.DamageAmount;

        float skillDuration = skillData.SkillDuration;
        float elapsed = 0;

        while (elapsed < skillDuration)
        {
            elapsed += Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }

        // Revert to original data
        weaponData.Cooldown = originalCooldown;
        weaponData.DamageAmount = originalDamageAmount;
        isTriggerSkill = false;
        isRunHandle = false;
    }
    public void Fire(Vector2 _target)
    {
        if (weaponData == null) return;
        onShoot?.Invoke(weaponData.ProjectileId, transform.position,
                _target, weaponData.NumberPerRound, weaponData.Cooldown, 0, new ProjectileData
                {
                    Damage = weaponData.DamageAmount,
                    ShootSpeed = weaponData.ShootSpeed,
                    TargetTag = weaponData.TargetTag,
                    HideOnHit = true,
                });
    }

    public float GetWeaponCooldown()
    {
        return weaponData.Cooldown;
    }


    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
        onGetNearestEnemy = _onGetNearestTarget;
    }

    public string GetUserId()
    {
        return weaponData.Uid;
    }

    public void DisableWeapon()
    {
        Timing.KillCoroutines(handle);
        gameObject.SetActive(false);
    }

    public void TriggerWeaponSkill()
    {
        isTriggerSkill = true;
    }
}
[Serializable]
public class MachineGunData : WeaponBaseData
{
    public int NumberPerRound;
    //public float FireSpreadOffset;
    public string ProjectileId;
    public float ShootSpeed;
    public MachineGunSkillData SkillData;
}

[Serializable]
public class MachineGunSkillData : WeaponSkillData
{
    public int DamageAmount;
    public float Cooldown;

}
