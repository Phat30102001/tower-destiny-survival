using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    [SerializeField] private RectTransform slotTransform;
    [SerializeField] private Turret turret;
    private bool isOccupied=false;
    TurretData data;
    private Action onTurretDestroy;
    private Action<string> onDisableWeaponTurret;
    

    public bool CheckIsOccupied() => isOccupied;
    public Transform GetSlotTransform()=> slotTransform;

    public void SetDataForTurret(TurretData _data)
    {
        if (turret == null) return;
        data = _data;
        turret.SetData(data);
        turret.AssignEvent(()=> 
        {
            isOccupied=false ;
            onTurretDestroy?.Invoke();
            gameObject.SetActive(false);
        }, onDisableWeaponTurret);
        gameObject.SetActive(true);
        isOccupied=true;
    }
    public void AssignEvent(Action _onTurretDestroy, Action<string> _onDisableWeaponTurret)
    {
        onTurretDestroy= _onTurretDestroy;
        onDisableWeaponTurret= _onDisableWeaponTurret;
    }
    public string GetTurretid()
    {
        return data.TurretId;
    }
    public Transform GetTurretWeaponContainer() => turret.GetWeaponCointainer();
}
