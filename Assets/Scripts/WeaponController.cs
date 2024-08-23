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
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public Func<Vector2> onGetNearestTarget;




    public void SetData(Transform _weaponContainer)
    {
        weaponContainer = _weaponContainer;
    }

    public void SpawnWeapon(string _weaponId, WeaponBaseData _data)
    {
        if (weaponContainer == null) return;
        GameObject _weaponPrefab = weapons.Find(x => x.GetComponent<IWeapon>().GetWeaponId().Equals(_weaponId));
        if (_weaponPrefab == null) return;


        GameObject _weaponObject = Instantiate(_weaponPrefab, weaponContainer);

        IWeapon _weapon= _weaponObject.GetComponent<IWeapon>();
        _weapon.SetData(_data);
        _weapon.AssignEvent(onShoot);
        weapon =_weapon;
    }
    public IEnumerator<float> CoroWeaponAutoAim()
    {
        while (true)
        {

            Vector2 _nearestEnemyPos = onGetNearestTarget();
            WeaponAutoAim(_nearestEnemyPos);
            yield return Timing.WaitForSeconds(weapon.GetWeaponCooldown());
        }
        yield return 0;
    }
    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot,Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
        onGetNearestTarget = _onGetNearestTarget;
    }

    public void WeaponAutoAim(Vector2 _pos)
    {
        weapon.AutoAim(_pos);
        weapon.Fire(_pos);
    }
    public IWeapon GetCurrentEquipWeapon()
    {
        return weapon;
    }
}