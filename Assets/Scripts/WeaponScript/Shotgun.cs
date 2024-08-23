using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private string weaponId;

    private ShotgunData shotgunData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public void SetData(WeaponBaseData _data)
    {
        if(_data is  ShotgunData _shotgunData)
        {
            shotgunData = _shotgunData;
        }
    }
    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot)
    {
        onShoot = _onShoot;
    }

    public string GetWeaponId()
    {
        return weaponId;
    }

    public void AutoAim(Vector2 _target)
    {
        float angle = Mathf.Atan2(_target.y, _target.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Fire(Vector2 _target)
    {
        if (shotgunData == null) return;
        onShoot?.Invoke(shotgunData.ProjectileId, transform.position,
                _target, shotgunData.NumberPerRound, shotgunData.Cooldown, shotgunData.FireSpreadOffset, new ProjectileData
                {
                    Damage = shotgunData.DamageAmount,
                    ShootForce = shotgunData.ShootForce,
                    TargetTag = shotgunData.TargetTag,
                    HideOnHit = true,
                });
    }

    public float GetWeaponCooldown()
    {
        return shotgunData.Cooldown;
    }
}
public class ShotgunData : WeaponBaseData
{

}
