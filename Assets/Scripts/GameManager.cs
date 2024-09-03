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
        resourceManager.AddResource(ResourceConstant.COIN, 0);
        resourceManager.AddResource(ResourceConstant.GEM, 0);

        uiManager.Init();
        uiManager.AssignEvent(gameplayManager.StartGame, InitStartPoint);
        gameplayManager.AssignEvent(OnEndGame);
        InitStartPoint();
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
