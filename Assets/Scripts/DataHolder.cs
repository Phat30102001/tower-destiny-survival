using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    [SerializeField] private Dictionary<string, List<WeaponBaseData>> weaponDataHolder = new Dictionary<string, List<WeaponBaseData>>();

    [SerializeField] private List<ShotgunData> shotgunLevelData = new List<ShotgunData>();
    [SerializeField] private List<MachineGunData> machineGunLevelDatas = new List<MachineGunData>();
    [SerializeField] private List<FlameThrowerData> flameThrowerLevelDatas = new List<FlameThrowerData>();
    [SerializeField] private List<ChainSawData> chainSawLevelDatas = new List<ChainSawData>();

    [SerializeField] private List<TurretData> turretLevelDatas = new List<TurretData>();

    public void Init()
    {
        weaponDataHolder.Add(WeaponIdConstant.SHOTGUN, shotgunLevelData.ConvertAll(x => x as WeaponBaseData));
        weaponDataHolder.Add(WeaponIdConstant.MACHINE_GUN, machineGunLevelDatas.ConvertAll(x => x as WeaponBaseData));
        weaponDataHolder.Add(WeaponIdConstant.FLAME_THROWER, flameThrowerLevelDatas.ConvertAll(x => x as WeaponBaseData));
        weaponDataHolder.Add(WeaponIdConstant.CHAINSAW, chainSawLevelDatas.ConvertAll(x => x as WeaponBaseData));
    }
    public TurretData GetTurretDataAtLevel(int _level)
    {
        return turretLevelDatas.First(x=>x.Level==_level);
    }
    public bool IsMaxTurretLevel(int _level)
    {
        return turretLevelDatas.Count <= _level;
    }


    public WeaponBaseData GetWeaponData(string _weaponId, int _level)
    {
        if (_weaponId == "") return null;
        return weaponDataHolder[_weaponId].Find(x => x.Level == _level);
    }
    public List<WeaponBaseData> GetAllLv1Turretweapondata()
    {
        
        List < WeaponBaseData > lv1Weapon= new List<WeaponBaseData>();
        foreach (var weaponData in weaponDataHolder)
        {
            if (weaponData.Value[0].Uid.Equals(TargetConstant.PLAYER)) continue;
            lv1Weapon.Add(weaponData.Value[0]);
        }
        return lv1Weapon;
    }
}


