﻿using MEC;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Transform weaponContainer;
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    private IWeapon weapon;
    private Dictionary<string,IWeapon> turretWeapons = new Dictionary<string, IWeapon>();
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public Func<Vector2> onGetNearestTarget;
    private Vector2 nearestEnemyPos = Vector2.zero;
    CoroutineHandle handle;

    public void SetData(Transform _weaponContainer)
    {
        weaponContainer = _weaponContainer;
    }

    public void SpawnWeapon(WeaponBaseData _data)
    {

        if (weaponContainer == null || _data.Uid != TargetConstant.PLAYER) return;
        if (weapon != null && weapon.GetWeaponId().Equals(_data.WeaponId))
        {

        }
        else
        {
            if (weapon != null) weapon.DisableWeapon();
            GameObject _weaponPrefab = weapons.Find(x => x.GetComponent<IWeapon>().GetWeaponId().Equals(_data.WeaponId));
            if (_weaponPrefab == null) return;

            GameObject _weaponObject = Instantiate(_weaponPrefab, weaponContainer);
            IWeapon _weapon = _weaponObject.GetComponent<IWeapon>();
            weapon = _weapon;

        }

        weapon.SetData(_data);
        weapon.AssignEvent(onShoot, getNearestEnemy);
    }

    public void SpawnTurretWeapon(WeaponBaseData _data, Transform _weaponContainer)
    {
        if (_weaponContainer == null) return;
        if(turretWeapons.TryGetValue(_data.Uid, out IWeapon _turretWeapon))
        {
            if (_turretWeapon.GetWeaponId().Equals(_data.WeaponId))
            {

            }
            else
            {
                _turretWeapon.DisableWeapon();

                GameObject _weaponPrefab = weapons.Find(x => x.GetComponent<IWeapon>().GetWeaponId().Equals(_data.WeaponId));
                if (_weaponPrefab == null) return;

                GameObject _weaponObject = Instantiate(_weaponPrefab, _weaponContainer);

                IWeapon _weapon = _weaponObject.GetComponent<IWeapon>();
                turretWeapons[_data.Uid]= _weapon;
            }
        }
        else
        {

            GameObject _weaponPrefab = weapons.Find(x => x.GetComponent<IWeapon>().GetWeaponId().Equals(_data.WeaponId));
            if (_weaponPrefab == null) return;

            GameObject _weaponObject = Instantiate(_weaponPrefab, _weaponContainer);

            IWeapon _weapon = _weaponObject.GetComponent<IWeapon>();
            turretWeapons.Add(_data.Uid, _weapon);
        }
        turretWeapons[_data.Uid].SetData(_data);
        turretWeapons[_data.Uid].AssignEvent(onShoot, getNearestEnemy);
    }
    public void DisableWeapon()
    {
        weapon.DisableWeapon();
        foreach (var _weapon in turretWeapons)
        {
            _weapon.Value.DisableWeapon();
        }
    }
    public void ActiveWeapon()
    {
        handle = Timing.RunCoroutine(AutoAimEnemy());
        handle = Timing.RunCoroutine(weapon.ActiveWeapon());

        foreach (var _weapon in turretWeapons)
        {
            handle = Timing.RunCoroutine(_weapon.Value.ActiveWeapon());
        }
    }

    public void RemoveWeapon(string _uid)
    {
        turretWeapons.TryGetValue(_uid, out IWeapon _weapon);
        _weapon.DisableWeapon();
    }

    public IEnumerator<float> AutoAimEnemy()
    {
        while (true)
        {
            if (TouchManager.Instance.IsTouching())
            {
                nearestEnemyPos = TouchManager.Instance.CurrentTouchPosition;
            }
            else
            {
                nearestEnemyPos = onGetNearestTarget();
            }
            yield return Timing.WaitForOneFrame;
        }
    }

    private Vector2 getNearestEnemy()
    {
        return nearestEnemyPos;
    }

    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
        onGetNearestTarget = _onGetNearestTarget;
    }

    public IWeapon GetCurrentEquipWeapon()
    {
        return weapon;
    }
}
