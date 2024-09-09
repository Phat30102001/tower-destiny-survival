using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IWeapon
{
    [SerializeField] DamageSender damageSender;
    [SerializeField] private string weaponId;
    private GrenadeData weaponData;
    Func<bool,Vector2> onGetNearestTarget;
    public IEnumerator<float> ActiveWeapon()
    {
        gameObject.SetActive(true);
        yield break;
    }

    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<bool,Vector2> _onGetNearestTarget)
    {
        onGetNearestTarget = _onGetNearestTarget;
    }

    public void DisableWeapon()
    {
        gameObject.SetActive(false);
    }

    public void Fire(Vector2 _target)
    {

    }

    public string GetUserId()
    {
        return weaponData.Uid;
    }

    public float GetWeaponCooldown()
    {
        return weaponData.Cooldown;
    }

    public string GetWeaponId()
    {
        return weaponId;
    }

    public void SetData(WeaponBaseData _data)
    {
        if (_data is GrenadeData _weaponData)
        {
            weaponData = _weaponData;
            damageSender.SetData(0, weaponData.DamageAmount, weaponData.TargetTag, false);
        }
    }

    public void TriggerWeaponSkill()
    {
        damageSender.transform.position = onGetNearestTarget(false);
        damageSender.gameObject.SetActive(true);
        Timing.CallDelayed(0.1f, () => damageSender.gameObject.SetActive(false));
        //damageSender.gameObject.SetActive(false);
    }
}
[Serializable]
public class GrenadeData : WeaponBaseData
{

}
