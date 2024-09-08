using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private UiManager uiManager;
    private DataHolder dataHolder;


    private void Start()
    {
        dataHolder = DataHolder.instance;
        resourceManager.AddResource(ResourceConstant.COIN, 1000);
        resourceManager.AddResource(ResourceConstant.GEM, 0);

        uiManager.Init();
        uiManager.AssignEvent(gameplayManager.StartGame, InitStartPoint, CreateTurret, UpgradeEnergy, onUseEnergy);
        gameplayManager.AssignEvent(OnEndGame, UpgradeTurret, UpgradeWeapon);
        InitStartPoint();
    }
    private int CreateTurret()
    {
        return resourceManager.ConsumeResource(
            dataHolder.GetTurretDataAtLevel(1).priceData, null, null, gameplayManager.CreateTurret);
    }
    private void onUseEnergy(string _weaponId)
    {

    }
    private EnergyData UpgradeEnergy()
    {
        var _currentEnergyData = SaveGameManager.LoadSaveEnergyData();
        EnergyData _resultData = _currentEnergyData;
        if (!dataHolder.CheckEnergyMaxlevel(_currentEnergyData.Level))
        {
            var _nextEnergyLevelData = dataHolder.GetEnergyData(_currentEnergyData.Level + 1);
            resourceManager.ConsumeResource(_nextEnergyLevelData.Price, (_remainAmount) =>
            {
                SaveGameManager.SaveEnergyData(_nextEnergyLevelData);
                uiManager.SetCoinData(_remainAmount);
                _resultData = _nextEnergyLevelData;
            });
        }
        return _resultData;

    }
    private void UpgradeTurret(string _turretIndex,int _currentLevel)
    {
        if (dataHolder.IsMaxTurretLevel(_currentLevel)) return;
        TurretData _data = dataHolder.GetTurretDataAtLevel(_currentLevel + 1);
        resourceManager.ConsumeResource(
    _data.priceData, ((_remainAmount) => { 
    
        _data.TurretId = _turretIndex;
        List<WeaponBaseData> _weaponBaseDatas = new List<WeaponBaseData>();
        var _turretKeyData = gameplayManager.GetTurretWeaponId(_turretIndex);
        WeaponBaseData _turretWeaponData= dataHolder.GetWeaponData(_turretKeyData._weaponId, _turretKeyData._level+1);
        
        if(_turretKeyData._weaponId!=""&&_turretKeyData._level>0)
            _weaponBaseDatas.Add(_turretWeaponData);

        else if(_turretWeaponData == null&& _turretKeyData._weaponId=="")
            _weaponBaseDatas = dataHolder.GetAllLv1Turretweapondata();


        gameplayManager.UpgradeTurret(_data, _weaponBaseDatas);
        uiManager.SetCoinData(_remainAmount);
        }), null);
    }
    private void UpgradeWeapon(string _turretId, string _weaponId, int _level)
    {
        resourceManager.ConsumeResource(
            dataHolder.GetWeaponData(_weaponId, _level).priceData, uiManager.SetCoinData, null,
            () => gameplayManager.BuyWeapon(_turretId, _weaponId, _level));
    }
    private void InitStartPoint()
    {
        gameplayManager.ActiveGameplay(resourceManager);

        uiManager.ShowUI(UiConstant.MAIN_MENU_UI, new MainMenuUiData()
        {
            Uid = UiConstant.MAIN_MENU_UI,
            CoinAmount = resourceManager.GetResourceValue(ResourceConstant.COIN),
            EnergyData=SaveGameManager.LoadSaveEnergyData(),
        });
    }
    private void OnEndGame(ResultType _type)
    {
        uiManager.ShowUI(UiConstant.RESULT_UI, new ResultUiData()
        {
            Uid = UiConstant.RESULT_UI,
            CoinAmount = resourceManager.GetResourceValue(ResourceConstant.COIN),
            ResultType = _type
        });

    }
    
}
public enum GameState
{
    Prepare, Playing, End
}
