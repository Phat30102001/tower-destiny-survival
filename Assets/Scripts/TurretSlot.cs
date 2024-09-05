using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    [SerializeField] private RectTransform slotTransform;
    [SerializeField] private Turret turret;
    [SerializeField] private TurretUi turretUi;
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
        gameObject.SetActive(true);
        isOccupied=true;
        turretUi.UpdateTurretData(data.Level);
    }
    public void AssignEvent(Action _onTurretDestroy, Action<string> _onDisableWeaponTurret,
        Action<string,int> onUpgradeTurret, Action<Action, string> onBuyWeapon)
    {
        turret.AssignEvent(()=> 
        {
            isOccupied=false ;
            onTurretDestroy?.Invoke();
            gameObject.SetActive(false);
        }, onDisableWeaponTurret);
        onTurretDestroy= _onTurretDestroy;
        onDisableWeaponTurret= _onDisableWeaponTurret;
        turretUi.AssignEvent(()=>onUpgradeTurret?.Invoke(data.TurretId, data.Level), onBuyWeapon);
    }
    public string GetTurretid()
    {
        return data.TurretId;
    }
    public Transform GetTurretWeaponContainer() => turret.GetWeaponCointainer();
}
