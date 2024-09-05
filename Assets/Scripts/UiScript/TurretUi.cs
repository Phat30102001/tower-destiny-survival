using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUi : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI turretLevelText;
    [SerializeField] private GameObject mainmenuGroup, gameplayGroup;

    public void AssignEvent(Action onUpgradeTurret, Action<Action,string> onBuyWeapon)
    {
        upgradeButton.onClick.AddListener(()=> onUpgradeTurret?.Invoke());


    }
    public void UpdateTurretData(int _level)
    {
        turretLevelText.text = _level.ToString();
    }
    public void UpdateGameState(GameState _state)
    {
        ResetUi();
        switch (_state)
        {
            case GameState.Prepare:
                ResetUi();
                mainmenuGroup.SetActive(true);
                break;
            case GameState.Playing:
                ResetUi();
                gameplayGroup.SetActive(true);
                break;

        }
    }
    private void ResetUi()
    {
        mainmenuGroup.SetActive(false);
        gameplayGroup.SetActive(false);
    }

}
