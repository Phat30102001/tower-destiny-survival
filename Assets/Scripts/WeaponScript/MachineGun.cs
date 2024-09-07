using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MachineGun : MonoBehaviour, IWeapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private string weaponId;
    private Func<Vector2> onGetNearestEnemy;

    private MachineGunData weaponData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public void SetData(WeaponBaseData _data)
    {
        if (_data is MachineGunData _weaponData)
        {
            weaponData = _weaponData;
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
            Vector2 _target = onGetNearestEnemy();
            float angle = Mathf.Atan2(_target.y, _target.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Fire(_target);
            yield return Timing.WaitForSeconds(weaponData.Cooldown);
        }
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
        gameObject.SetActive(false);
    }
}
[Serializable]
public class MachineGunData : WeaponBaseData
{
    public int NumberPerRound;
    //public float FireSpreadOffset;
    public string ProjectileId;
    public float ShootSpeed;
}
