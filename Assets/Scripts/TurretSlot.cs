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

    public void SetDataForTurret(TurretData _data,List<WeaponBaseData> _weaponData)
    {
        if (turret == null) return;
        data = _data;
        turret.SetData(data);
        gameObject.SetActive(true);
        isOccupied=true;
        SetTurretDataUi(_data.Level, _data.priceData.ResourceValue, _weaponData);
        SaveGameManager.SaveTurretData(_data);
        if(_data.WeaponId != "" && _data.WeaponLevel > 0)
            turret.SetTurretWeapon(_data.WeaponId, _data.WeaponLevel);
    }
    public void SetTurretDataUi(int _level, int _price, List<WeaponBaseData> _weaponData)
    {
        turretUi.UpdateTurretData(_level, _price, ConvertWeaponData(_weaponData));

    }

    private List<WeaponButtonData> ConvertWeaponData(List<WeaponBaseData> _weaponData)
    {
        List < WeaponButtonData > weaponButtonDataList = new List<WeaponButtonData>();
        foreach (var weaponData in _weaponData)
        {
            if (weaponData == null) continue;
            WeaponButtonData weaponButtonData = new WeaponButtonData();
            weaponButtonData.weaponId = weaponData.WeaponId;
            weaponButtonData.weaponLevel = weaponData.Level;
            weaponButtonData.weaponPrice = weaponData.priceData.ResourceValue;
            weaponButtonDataList.Add(weaponButtonData);
        }
        return weaponButtonDataList;
    }

    public void AssignEvent(Action _onTurretDestroy, Action<string> _onDisableWeaponTurret,
        Action<string,int> onUpgradeTurret, Action<string,string,int> onBuyWeapon)
    {
        turret.AssignEvent(()=> 
        {
            onTurretDestroy?.Invoke();
            gameObject.SetActive(false);
        }, onDisableWeaponTurret);
        onTurretDestroy= _onTurretDestroy;
        onDisableWeaponTurret= _onDisableWeaponTurret;
        turretUi.AssignEvent(()=>onUpgradeTurret?.Invoke(data.TurretId, data.Level),(_weaponId, _level)=> {
            onBuyWeapon?.Invoke(GetTurretid(), _weaponId, _level);
        });
    }
    public string GetTurretid()
    {
        return data.TurretId??"";
    }
    public Transform GetTurretWeaponContainer() => turret.GetWeaponCointainer();
    public string GetTurretWeaponId() => turret.CurrentEquipWeaponId();
    public int GetTurretWeaponLevel() => turret.CurrentEquipWeaponLevel();
    public void SetTurretWeapon(WeaponBaseData _data,WeaponBaseData _nextLevelData)
    {
        turret.SetTurretWeapon(_data.WeaponId, _data.Level);
        turretUi.SetWeaponData(ConvertWeaponData(new List<WeaponBaseData>() { _nextLevelData }));
        SaveGameManager.SaveWeaponTurretData(data.TurretId, _data.WeaponId, _data.Level);
    }

    public void DisablePrepareUi()
    {
        turretUi.ResetUi();
    }
    public void ActivePrepareUi()
    {
        turretUi.UpdateGameState(GameState.Prepare);
    }

}
