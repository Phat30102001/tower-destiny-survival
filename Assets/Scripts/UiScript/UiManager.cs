using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // Dictionary to store different UIs by their name
    private Dictionary<string, UiBase> uiDictionary = new Dictionary<string, UiBase>();

    // List of all UI GameObjects to manage their active state
    public List<UiBase> uiList;
    Action onStartGame;
    Action onBackToMainMenu;

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
            uiBase.SetData(_data);
            AssignUiEvent(_uid, uiBase);
        }
        else
        {
            Debug.LogError($"UI with name {_uid} not found.");
        }
    }
    public void AssignEvent(Action _onStartGame,Action _onBackToMainMenu) 
    {
        onStartGame= _onStartGame;
        onBackToMainMenu = _onBackToMainMenu;
    }

    private void AssignUiEvent(string _uid,UiBase uiBase)
    {
        switch (_uid) {
            case UiConstant.MAIN_MENU_UI:
                MainMenuUI mainMenuUI = uiBase as MainMenuUI;
                mainMenuUI.AssignEvents(OnStartGame);
                break;

            case UiConstant.SETTING_UI:
                break;
            case UiConstant.RESULT_UI:
                ResultUi resultUi = uiBase as ResultUi;
                resultUi.AssignEvents(OnBecameInvisible);
                break;
            default:
                Debug.LogWarning("Unhandled UI type.");
                break;
        } 
    }
    private void OnStartGame()
    {
        onStartGame?.Invoke();
        HideUI(UiConstant.MAIN_MENU_UI);
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



