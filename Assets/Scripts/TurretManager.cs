using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private List<TurretSlot> turretSlots;
    [SerializeField] private Turret turretPrefab;
    [SerializeField] private Transform turretTransform;
    Action onZeroTurret;

    public void CheckAnyTurretAlive()
    {
        foreach (var _slot in turretSlots)
        {
            if (_slot.CheckIsOccupied())
                return;
        }
        onZeroTurret?.Invoke();
    }

    public void GenerateTurret(TurretData turretData)
    {
        TurretSlot _turretSlot=new TurretSlot();
        bool _isHaveSlot=false;
        foreach (TurretSlot _slot in turretSlots)
        {
            if (!_slot.CheckIsOccupied())
            {
                _turretSlot = _slot;
                _isHaveSlot = true;
                break;
            }
        }
        if (!_isHaveSlot) return;
        
        _turretSlot.SetDataForTurret(turretData);
    }
    public Transform GetTurretTransformAtId(string _id)
    {
        foreach(var _slot in turretSlots)
        {
            if(!_slot.CheckIsOccupied())continue;
            if (_slot.GetTurretid().Equals(_id))
            {
                return _slot.GetTurretWeaponContainer();    
            }
        }
        return null;
    }
    public Transform GetTurretTransform()
    {
        return turretTransform;
    }
    public void AssignEvent(Action _onZeroTurret,Action<string> _onDisableTurretWeapon)
    {
        foreach (var _slot in turretSlots)
        {
            _slot.AssignEvent(CheckAnyTurretAlive, _onDisableTurretWeapon);
        }
        onZeroTurret = _onZeroTurret;
    }
}
