using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField] private Projectile projectile;
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


}
public class ShotgunData : WeaponBaseData
{

}
