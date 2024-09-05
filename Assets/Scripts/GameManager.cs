using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private DataHolder dataHolder;

    private void Start()
    {
        dataHolder.Init();

        resourceManager.AddResource(ResourceConstant.COIN, 100);
        resourceManager.AddResource(ResourceConstant.GEM, 0);

        uiManager.Init();
        uiManager.AssignEvent(gameplayManager.StartGame, InitStartPoint, CreateTurret);
        gameplayManager.AssignEvent(OnEndGame, UpgradeTurret,null);
        InitStartPoint();
    }
    private int CreateTurret()
    {
        return resourceManager.ConsumeResource(
            dataHolder.GetTurretDataAtLevel(1).priceData, null, null, gameplayManager.CreateTurret);
    }
    private void UpgradeTurret(string _turretIndex,int _currentLevel)
    {
        TurretData _data = dataHolder.GetTurretDataAtLevel(_currentLevel + 1);
        resourceManager.ConsumeResource(
    _data.priceData, (() => { 
    
        _data.TurretId = _turretIndex;
        gameplayManager.UpgradeTurret(_data);
        }), null);
    }
    private void InitStartPoint()
    {
        gameplayManager.ActiveGameplay(resourceManager, dataHolder);
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
