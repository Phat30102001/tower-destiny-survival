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
    private Func<Vector2> onGetNearestEnemy;

    private ShotgunData shotgunData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public void SetData(WeaponBaseData _data)
    {
        if(_data is  ShotgunData _shotgunData)
        {
            shotgunData = _shotgunData;
        }
    }

    public string GetWeaponId()
    {
        return weaponId;
    }

    public IEnumerator<float> AutoAim()
    {
        while (gameObject.activeSelf)
        {
            Vector2 _target = onGetNearestEnemy();
            float angle = Mathf.Atan2(_target.y, _target.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Fire(_target);
            yield return Timing.WaitForSeconds( shotgunData.Cooldown);
        }
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


    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
        onGetNearestEnemy=_onGetNearestTarget;
    }
}
public class ShotgunData : WeaponBaseData
{

}
