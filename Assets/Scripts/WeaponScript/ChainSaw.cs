using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChainSaw : MonoBehaviour, IWeapon
{
    [SerializeField] DamageSender damageSender;
    [SerializeField] private string weaponId;

    private ChainSawData weaponData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public void SetData(WeaponBaseData _data)
    {
        if (_data is ChainSawData _weaponData)
        {
            weaponData = _weaponData;
            damageSender.SetData(weaponData.Cooldown, weaponData.DamageAmount, weaponData.TargetTag, false);
        }
    }

    public string GetWeaponId()
    {
        return weaponId;
    }

    public IEnumerator<float> ActiveWeapon()
    {
        gameObject.SetActive(true);
        yield break;
    }

    public void Fire(Vector2 _target)
    {
    }

    public float GetWeaponCooldown()
    {
        return weaponData.Cooldown;
    }


    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
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
public class ChainSawData : WeaponBaseData
{
}
