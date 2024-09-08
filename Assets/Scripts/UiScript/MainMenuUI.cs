using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : UiBase
{
    [SerializeField] private string uid=UiConstant.MAIN_MENU_UI;
    [SerializeField] private TextMeshProUGUI coinAmount;
    [SerializeField] private Button addTurretButton;
    [SerializeField] private TextMeshProUGUI energyInfoText;
    [SerializeField] private Button upgradeEnergyButton;
    public Button startButton;
    Action onStartGame;
    Func<EnergyData> onUpgradeEnergy;
    Func<int> onAddTurret;
    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => onStartGame?.Invoke());

        upgradeEnergyButton.onClick.RemoveAllListeners();
        upgradeEnergyButton.onClick.AddListener(() =>
        {
            var _energyData = onUpgradeEnergy();
            SetEnergyData(_energyData);
        });

        addTurretButton.onClick.RemoveAllListeners();
        addTurretButton.onClick.AddListener(() => {
            SetCoinAmount(onAddTurret());

        });
    }
    public override void SetData(UiBaseData data)
    {
        if(data is MainMenuUiData _mainMenuData)
        {
            MainMenuUiData mainMenuUiData= _mainMenuData;
            SetCoinAmount(mainMenuUiData.CoinAmount);
            SetEnergyData(mainMenuUiData.EnergyData);
        }
        
    }
    public void SetCoinAmount(long amount)
    {
        coinAmount.text = amount.ToString();
    }  
    public void SetEnergyData(EnergyData _data)
    {
        energyInfoText.text = $"Price: {_data.Price.ResourceValue}: {_data.EnergyGenerateValuePerSecond}/s";
    }

    public void AssignEvents(Action _onStartGame,Func<int> _onAddTurret, Func<EnergyData> _onUpgradeEnergy)
    {
        onAddTurret = _onAddTurret;
        onStartGame = _onStartGame;
        onUpgradeEnergy = _onUpgradeEnergy;
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
    public EnergyData EnergyData;
}



