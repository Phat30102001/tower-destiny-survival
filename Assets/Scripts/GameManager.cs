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
        gameplayManager.ActiveGameplay(resourceManager);
        uiManager.AssignEvent(gameplayManager.StartGame);
        uiManager.ShowUI(UiConstant.MAIN_MENU_UI, new UiBaseData());
    }

}
