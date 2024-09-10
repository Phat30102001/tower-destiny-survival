using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    // Dictionary to store different UIs by their name
    private Dictionary<string, UiBase> uiDictionary = new Dictionary<string, UiBase>();

    // List of all UI GameObjects to manage their active state
    public List<UiBase> uiList;
    Action onStartGame;
    Action onBackToMainMenu;
    Func<int> onAddTurret;
    Action<string> onUseEnergy;
    Func<EnergyData> onUpgradeEnergy;

    public Canvas GetCanvas()
    {
        return canvas;
    }
    // Called when the script instance is being loaded
    public void Init()
    {
        // Initialize UI dictionary
        foreach (UiBase ui in uiList)
        {
            uiDictionary[ui.GetUiId()] = ui;
            ui.Hide();
        }
    }

    public void ShowUI(string _uid, UiBaseData _data)
    {
        HideAllUIs();

        if (uiDictionary.TryGetValue(_uid, out UiBase uiBase))
        {
            uiBase.Show();
            AssignUiEvent(_uid, uiBase);
            uiBase.SetData(_data);
        }
        else
        {
            Debug.LogError($"UI with name {_uid} not found.");
        }
    }
    public void AssignEvent(Action _onStartGame,Action _onBackToMainMenu,Func<int> _onAddTurret
        ,Func<EnergyData> _onUpgradeEnergy,Action<string> _onUseEnergy) 
    {
        onStartGame= _onStartGame;
        onBackToMainMenu = _onBackToMainMenu;
        onAddTurret= _onAddTurret;
        onUpgradeEnergy = _onUpgradeEnergy;
        onUseEnergy = _onUseEnergy;
    }

    private void AssignUiEvent(string _uid,UiBase uiBase)
    {
        switch (_uid) {
            case UiConstant.MAIN_MENU_UI:
                MainMenuUI mainMenuUI = uiBase as MainMenuUI;
                mainMenuUI.AssignEvents(OnStartGame, OnAddTurret,onUpgradeEnergy);
                break;

            case UiConstant.SETTING_UI:
                break;
            case UiConstant.RESULT_UI:
                ResultUi resultUi = uiBase as ResultUi;
                resultUi.AssignEvents(OnBecameInvisible);
                break;
            case UiConstant.GAMEPLAY_UI:
                GameplayUi gameplayUi = uiBase as GameplayUi;
                gameplayUi.AssignEvents(onUseEnergy);
                break;

            default:
                Debug.LogWarning("Unhandled UI type.");
                break;
        } 
    }
    public void SetCoinData(long amount)
    {
        uiDictionary.TryGetValue(UiConstant.MAIN_MENU_UI, out var _ui);
        if (_ui != null && _ui is MainMenuUI _mainMenuUi)
            _mainMenuUi.SetCoinAmount(amount);
    }
    private void OnStartGame()
    {
        onStartGame?.Invoke();
        HideUI(UiConstant.MAIN_MENU_UI);
        ShowUI(UiConstant.GAMEPLAY_UI, GetGameplayData());
    }
    private GameplayUiData GetGameplayData()
    {
        var turretDataList = new List<TurretData>(SaveGameManager.LoadSaveTurretData().Values);
        var weaponBaseDataList = DataHolder.instance.GetWeaponBaseDatasFromTurretDatas(turretDataList);

        List<WeaponSkillButtonData> _weaponSkillButtonDatas = new();
        foreach (var _weaponBaseData in weaponBaseDataList)
        {
            if (_weaponBaseData == null) continue;
            if (_weaponSkillButtonDatas.Any(x => x.weaponId.Equals(_weaponBaseData.WeaponId))) continue;
            _weaponSkillButtonDatas.Add( ConvertWeaponBaseDataToWeaponSkillData(_weaponBaseData));
        }
        var _weaponKeyData = SaveGameManager.GetPlayerWeaponTriggerSkillData();

        return new GameplayUiData
        {
            WeaponSkillButtonDatas = _weaponSkillButtonDatas,
            PlayerWeaponSkillButtonData=ConvertWeaponBaseDataToWeaponSkillData(
                DataHolder.instance.GetWeaponData(_weaponKeyData._weaponId, _weaponKeyData._level))
        };
    }
    private WeaponSkillButtonData ConvertWeaponBaseDataToWeaponSkillData (WeaponBaseData _weaponBaseData)
    {
        WeaponSkillButtonData _data = new WeaponSkillButtonData()
        {
            weaponId = _weaponBaseData.WeaponId,
            EnergyRequire = _weaponBaseData.EnergyRequire
        };
        return _data;


    }
    private int OnAddTurret()
    {
        return onAddTurret();
    }
    private void OnBecameInvisible()
    {
        onBackToMainMenu?.Invoke();
    }


    public void HideAllUIs()
    {
        foreach (var _ui in uiDictionary.Values)
        {
            _ui.Hide();
        }
    }  
    public void HideUI(string _uid)
    {
        if (uiDictionary.TryGetValue(_uid, out UiBase uiBase))
        {
            uiBase.Hide();
        }
    }
}
// Base class for all UIs
public abstract class UiBase : MonoBehaviour
{
    public abstract string GetUiId();
    public abstract void SetData(UiBaseData data);
    public abstract void Show();
    public abstract void Hide();

}
public class UiBaseData
{
    public string Uid;
}

public static class UiConstant
{
    public const string MAIN_MENU_UI = "MainMenuUI";
    public const string GAMEPLAY_UI = "GameplayUI";
    public const string SETTING_UI="SettingUI";
    public const string RESULT_UI = "ResultUI";
}



