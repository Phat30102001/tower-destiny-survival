using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;
    private void Start()
    {
        instance = this;
    }
    private Dictionary<string, TurretData> saveTurretData = new();

    public void SaveTurretData(TurretData _data)
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
    public void SaveWeaponTurretData(string _uid, string _weaponId, int _level)
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
    public Dictionary<string,TurretData> LoadSaveTurretData()
    {
        return saveTurretData;
    }
}
