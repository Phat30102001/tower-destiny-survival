using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGameManager
{
    private static Dictionary<string, TurretData> saveTurretData = new();
    private static EnergyData saveEnergyData;

    public static void SaveTurretData(TurretData _data)
    {
        if (saveTurretData.ContainsKey(_data.TurretId))
        {
            saveTurretData[_data.TurretId] = _data;
            return;
        }
        else
        {
            saveTurretData.Add(_data.TurretId, _data);
        }
    }
    public static void SaveWeaponTurretData(string _uid, string _weaponId, int _level)
    {
        if (saveTurretData.ContainsKey(_uid))
        {
            TurretData data = saveTurretData[_uid];
            data.WeaponId = _weaponId;
            data.WeaponLevel = _level;
            saveTurretData[_uid] = data;
            return;
        }
    }
    public static Dictionary<string,TurretData> LoadSaveTurretData()
    {
        return saveTurretData;
    }

    public static void SaveEnergyData(EnergyData _data)
    {
        saveEnergyData = _data;
    }
    public static EnergyData LoadSaveEnergyData()
    {
        if(saveEnergyData.Level<=0)
        {
            saveEnergyData = DataHolder.instance.GetEnergyData(1);
        }
        return saveEnergyData;
    }
}
