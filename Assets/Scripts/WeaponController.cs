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
    private Vector2 nearestEnemyPos=Vector2.zero;
    CoroutineHandle handle;



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
        _weapon.AssignEvent(onShoot, getNearestEnemy);
        weapon =_weapon;
    }
    public void ActiveWeapon()
    {
        handle = Timing.RunCoroutine(AutoAimEnemy());
        handle=Timing.RunCoroutine( weapon.AutoAim());
        
    }
    public IEnumerator<float> AutoAimEnemy()
    {
        while (true)
        {
            nearestEnemyPos = onGetNearestTarget();
            yield return Timing.WaitForOneFrame;
        }
    }
    
    private Vector2 getNearestEnemy()
    {
        return nearestEnemyPos;
    }
    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot,Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
        onGetNearestTarget = _onGetNearestTarget;
    }

    public IWeapon GetCurrentEquipWeapon()
    {
        return weapon;
    }
}