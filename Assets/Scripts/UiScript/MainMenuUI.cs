using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UiBase
{
    [SerializeField] private string uid=UiConstant.MAIN_MENU_UI;
    [SerializeField] private TextMeshProUGUI coinAmount;
    public Button startButton;

    public override void SetData(UiBaseData data)
    {
        if(data is MainMenuUiData _mainMenuData)
        {
            MainMenuUiData mainMenuUiData= _mainMenuData;
            SetCoinAmount(mainMenuUiData.CoinAmount);
        }
        
    }
    public void SetCoinAmount(long amount)
    {
        coinAmount.text = amount.ToString();
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
    public long CoinAmount;
}



