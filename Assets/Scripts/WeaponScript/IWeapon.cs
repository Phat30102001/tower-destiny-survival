

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public string GetWeaponId();
    public string GetUserId();
    public void DisableWeapon();
    public float GetWeaponCooldown();
    public IEnumerator<float> ActiveWeapon();
    public void SetData(WeaponBaseData _data);
    public void Fire(Vector2 _target);
    public void AssignEvent(Action<string, Vector2, Vector2, int
        , float, float, ProjectileData> _onShoot, Func<Vector2> onGetNearestTarget);
}
public class WeaponBaseData
{
    public string Uid;
    public string WeaponId;
    public int Level;
    public int DamageAmount;
    public float Cooldown;
    //public int NumberPerRound;
    //public float FireSpreadOffset;
    public string TargetTag;

    public ResourceData priceData;
    //public float ShootForce;
    //public string ProjectileId;
}
