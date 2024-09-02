using MEC;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Transform weaponContainer;
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    private IWeapon weapon;
    private List<IWeapon> turretWeapons = new List<IWeapon>();
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
        GameObject _weaponPrefab = weapons.Find(x => x.GetComponent<IWeapon>().GetWeaponId().Equals(_data.WeaponId));
        if (_weaponPrefab == null) return;

        GameObject _weaponObject = Instantiate(_weaponPrefab, weaponContainer);

        IWeapon _weapon = _weaponObject.GetComponent<IWeapon>();
        _weapon.SetData(_data);
        _weapon.AssignEvent(onShoot, getNearestEnemy);
        weapon = _weapon;
    }

    public void SpawnTurretWeapon(WeaponBaseData _data, Transform _weaponContainer)
    {
        if (_weaponContainer == null) return;
        GameObject _weaponPrefab = weapons.Find(x => x.GetComponent<IWeapon>().GetWeaponId().Equals(_data.WeaponId));
        if (_weaponPrefab == null) return;

        GameObject _weaponObject = Instantiate(_weaponPrefab, _weaponContainer);

        IWeapon _weapon = _weaponObject.GetComponent<IWeapon>();
        _weapon.SetData(_data);
        _weapon.AssignEvent(onShoot, getNearestEnemy);
        turretWeapons.Add(_weapon);
    }

    public void ActiveWeapon()
    {
        handle = Timing.RunCoroutine(AutoAimEnemy());
        handle = Timing.RunCoroutine(weapon.ActiveWeapon());

        foreach (var _weapon in turretWeapons)
        {
            handle = Timing.RunCoroutine(_weapon.ActiveWeapon());
        }
    }

    public void RemoveWeapon(string _uid)
    {
        var _weapon = turretWeapons.Find(x => x.GetUserId().Equals(_uid));
        if ((weapon == null))
        {
            return;
        }
        turretWeapons.Remove(_weapon);
    }

    public IEnumerator<float> AutoAimEnemy()
    {
        while (true)
        {
            if (TouchManager.Instance.IsTouching())
            {
                nearestEnemyPos = TouchManager.Instance.CurrentTouchPosition;
                Debug.Log($"nearestEnemyPos: {nearestEnemyPos}");
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
