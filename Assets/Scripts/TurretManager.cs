using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private List<TurretSlot> turretSlots;
    [SerializeField] private Turret turretPrefab;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private TurretSwaper turretSwaper;
    Action onZeroTurret;
    private Canvas canvas;

    public void RefreshManager(Canvas _canvas)
    {
        canvas = _canvas;
        turretSwaper.AssignEvent(swapTurretPostion);
        var _cacheTurretData = SaveGameManager.LoadSaveTurretData();
        foreach (var _slot in turretSlots)
        {

            if (_slot.CheckIsOccupied())
            {
                var _turretData = _cacheTurretData[_slot.GetTurretid()];
                List <WeaponBaseData> _weaponBaseDatas = new List<WeaponBaseData>();
                WeaponBaseData _turretWeaponData = DataHolder.instance.GetWeaponData(_turretData.WeaponId, _turretData.WeaponLevel + 1);

                if (_turretData.WeaponId != "" && _turretData.WeaponLevel > 0)
                    _weaponBaseDatas.Add(_turretWeaponData);

                else if (_turretWeaponData == null && _turretData.WeaponId == "")
                    _weaponBaseDatas = DataHolder.instance.GetAllLv1Turretweapondata();


                _slot.gameObject.SetActive(true);
                _slot.SetDataForTurret(_turretData, _weaponBaseDatas,canvas);
            }
            
        }
        ActivePrepareState();
    }
    private void swapTurretPostion(int _swapTurretIndex,int _turretBeingSwapIndex)
    {
        if (_swapTurretIndex < 0 || _swapTurretIndex >= turretSlots.Count ||
    _turretBeingSwapIndex < 0 || _turretBeingSwapIndex >= turretSlots.Count)
        {
            Debug.LogError("Invalid turret indices for swapping.");
            return;
        }

        // Swap the turrets in the list
        TurretSlot temp = turretSlots[_swapTurretIndex];
        turretSlots[_swapTurretIndex] = turretSlots[_turretBeingSwapIndex];
        turretSlots[_turretBeingSwapIndex] = temp;

        // Update the sibling indices in the hierarchy
        turretSlots[_swapTurretIndex].transform.SetSiblingIndex(_swapTurretIndex);
        turretSlots[_turretBeingSwapIndex].transform.SetSiblingIndex(_turretBeingSwapIndex);
    }

    public void CheckAnyTurretAlive()
    {
        foreach (var _slot in turretSlots)
        {
            if (_slot.gameObject.activeSelf)
                return;
        }
        onZeroTurret?.Invoke();
    }

    public bool GenerateTurret(TurretData turretData, List<WeaponBaseData> _weaponDatas)
    {
        TurretSlot _turretSlot = new TurretSlot();
        bool _isHaveSlot = false;
        for (int i = 0; i < turretSlots.Count; i++)
        {
            TurretSlot _slot = turretSlots[i];
            if (!_slot.CheckIsOccupied())
            {
                _turretSlot = _slot;
                _isHaveSlot = true;
                turretData.TurretId = i.ToString();
                break;
            }
        }
        if (!_isHaveSlot) return false;
        _turretSlot.SetDataForTurret(turretData, _weaponDatas,canvas);
        return true;
    }
    public void UpgradeTurret(TurretData turretData, List<WeaponBaseData> _weaponDatas)
    {
        TurretSlot _turretSlot = turretSlots.
            Find(x => x.GetTurretid().Equals(turretData.TurretId));

        _turretSlot?.SetDataForTurret(turretData, _weaponDatas, canvas);
        
    }
    public string GetWeaponIdTurretAtId(string _id)
    {
        foreach (var _slot in turretSlots)
        {
            if (!_slot.CheckIsOccupied()) continue;
            if (_slot.GetTurretid().Equals(_id))
            {
                return _slot.GetTurretWeaponId();
            }
        }
        return null;
    } 
    public int GetWeaponLevelTurretAtId(string _id)
    {
        foreach (var _slot in turretSlots)
        {
            if (!_slot.CheckIsOccupied()) continue;
            if (_slot.GetTurretid().Equals(_id))
            {
                return _slot.GetTurretWeaponLevel();
            }
        }
        return -1;
    }
    public Transform GetTurretTransformAtId(string _id)
    {
        foreach (var _slot in turretSlots)
        {
            if (!_slot.CheckIsOccupied()) continue;
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
    public void AssignEvent(Action _onZeroTurret, Action<string> _onDisableTurretWeapon,
        Action<string, int> onUpgradeTurret, Action<string, string, int> onBuyWeapon)
    {
        foreach (var _slot in turretSlots)
        {
            _slot.AssignEvent(CheckAnyTurretAlive,SaveGameManager.DeleteTurretData, _onDisableTurretWeapon
                , onUpgradeTurret, onBuyWeapon, turretSwaper.SetCurrentTurretBeingDrag);
        }
        onZeroTurret = _onZeroTurret;
    }

    public void SetTurretWeaponId(string _turretId, WeaponBaseData _weaponData, WeaponBaseData _nextLevelWeaponData)
    {
        foreach (var _slot in turretSlots)
        {
            if (_slot.GetTurretid().Equals(_turretId))
            {
                _slot.SetTurretWeapon(_weaponData,_nextLevelWeaponData);

            }
        }
    }
    public void ActiveGameplay()
    {
        foreach(var _turretSlot in turretSlots)
        {
            _turretSlot.DisablePrepareUi();
        }
        turretSwaper.SetDragable(false);
    }
    public void ActivePrepareState()
    {
        foreach (var _turretSlot in turretSlots)
        {
            _turretSlot.ActivePrepareUi();
        }
        turretSwaper.SetDragable(true);
    }
}
