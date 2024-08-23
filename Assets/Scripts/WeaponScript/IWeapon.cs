

using System;
using UnityEngine;

public interface IWeapon
{
    public string GetWeaponId();
    public float GetWeaponCooldown();
    public void AutoAim(Vector2 _target);
    public void SetData(WeaponBaseData _data);
    public void Fire(Vector2 _target);
    public void AssignEvent(Action<string, Vector2, Vector2, int
        , float, float, ProjectileData> _onShoot);
}
public class WeaponBaseData
{
    public string WeaponId;
    public int DamageAmount;
    public float Cooldown;
    public int NumberPerRound;
    public float FireSpreadOffset;
    public string TargetTag;
    public float ShootForce;
    public string ProjectileId;
}
