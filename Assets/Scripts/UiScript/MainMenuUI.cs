using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UiBase
{
    [SerializeField] private string uid=UiConstant.MAIN_MENU_UI;
    public Button startButton;

    public override void SetData(UiBaseData data)
    {

    }

    public void AssignEvents(Action onStartGame)
    {
        startButton.onClick.AddListener(()=> onStartGame?.Invoke());
    }

    public override string GetUiId()
    {
       return uid;
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
public class MainMenuUiData : UiBaseData
{
 
}



