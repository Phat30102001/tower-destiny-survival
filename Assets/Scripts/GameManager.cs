using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private UiManager uiManager;


    private void Start()
    {

        resourceManager.AddResource(ResourceConstant.COIN, 1000);
        resourceManager.AddResource(ResourceConstant.GEM, 0);

        uiManager.Init();
        uiManager.AssignEvent(gameplayManager.StartGame, InitStartPoint, CreateTurret);
        gameplayManager.AssignEvent(OnEndGame, UpgradeTurret, UpgradeWeapon);
        InitStartPoint();
    }
    private int CreateTurret()
    {
        return resourceManager.ConsumeResource(
            DataHolder.instance.GetTurretDataAtLevel(1).priceData, null, null, gameplayManager.CreateTurret);
    }
    private void UpgradeTurret(string _turretIndex,int _currentLevel)
    {
        if (DataHolder.instance.IsMaxTurretLevel(_currentLevel)) return;
        TurretData _data = DataHolder.instance.GetTurretDataAtLevel(_currentLevel + 1);
        resourceManager.ConsumeResource(
    _data.priceData, ((_remainAmount) => { 
    
        _data.TurretId = _turretIndex;
        List<WeaponBaseData> _weaponBaseDatas = new List<WeaponBaseData>();
        var _turretKeyData = gameplayManager.GetTurretWeaponId(_turretIndex);
        WeaponBaseData _turretWeaponData= DataHolder.instance.GetWeaponData(_turretKeyData._weaponId, _turretKeyData._level+1);
        
        if(_turretKeyData._weaponId!=""&&_turretKeyData._level>0)
            _weaponBaseDatas.Add(_turretWeaponData);

        else if(_turretWeaponData == null&& _turretKeyData._weaponId=="")
            _weaponBaseDatas = DataHolder.instance.GetAllLv1Turretweapondata();


        gameplayManager.UpgradeTurret(_data, _weaponBaseDatas);
        uiManager.SetCoinData(_remainAmount);
        }), null);
    }
    private void UpgradeWeapon(string _turretId, string _weaponId, int _level)
    {
        resourceManager.ConsumeResource(
            DataHolder.instance.GetWeaponData(_weaponId, _level).priceData, uiManager.SetCoinData, null,
            () => gameplayManager.BuyWeapon(_turretId, _weaponId, _level));
    }
    private void InitStartPoint()
    {
        gameplayManager.ActiveGameplay(resourceManager);

        uiManager.ShowUI(UiConstant.MAIN_MENU_UI, new MainMenuUiData()
        {
            Uid = UiConstant.MAIN_MENU_UI,
            CoinAmount = resourceManager.GetResourceValue(ResourceConstant.COIN)
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
