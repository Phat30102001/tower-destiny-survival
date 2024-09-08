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
    [SerializeField] private TextMeshProUGUI turretLevelPriceText;
    [SerializeField] private TextMeshProUGUI turretWeaponText;
    [SerializeField] private GameObject mainmenuGroup, gameplayGroup;
    [SerializeField] List<WeaponButton> weaponButtons;
    [SerializeField] private Transform weaponButtonGroup;
    [SerializeField] private WeaponButton weaponButtonPrefab;
    private List<WeaponButtonData> weaponButtonsData=new();
    private Action<string, int> onBuyWeapon;
    public void AssignEvent(Action onUpgradeTurret, Action<string,int> _onBuyWeapon)
    {
        upgradeButton.onClick.AddListener(()=> onUpgradeTurret?.Invoke());
        onBuyWeapon = _onBuyWeapon;


    }
    public void UpdateTurretData(int _level,int _price, List<WeaponButtonData> _datas)
    {
        turretLevelText.text = _level.ToString();
        turretLevelPriceText.text = _price.ToString();
        SetWeaponData(_datas);
    }
    public void SetWeaponData(List<WeaponButtonData> _datas)
    {
        GenerateWeaponButton(_datas);
    }

    public void GenerateWeaponButton(List<WeaponButtonData> _datas)
    {
        refreshWeaponButton();

        for (int i = 0; i < _datas.Count; i++)
        {
            if (i < weaponButtons.Count)
            {
                weaponButtons[i].SetData(_datas[i]);
                weaponButtons[i].gameObject.SetActive(true);
            }
            else
            {
                WeaponButton weaponButton = Instantiate(weaponButtonPrefab, weaponButtonGroup);
                weaponButton.SetData(_datas[i]);
                weaponButton.AssignEvent(onBuyWeapon);
                weaponButtons.Add(weaponButton);
            }
        }
        
        if (_datas.Count == 1)
            turretWeaponText.text = $"{_datas[0].weaponId} {_datas[0].weaponLevel}";
    }
    private void refreshWeaponButton()
    {
        foreach (var weaponButton in weaponButtons)
        {
            weaponButton.gameObject.SetActive(false);
        }
    }
    public void UpdateGameState(GameState _state)
    {
        ResetUi();
        switch (_state)
        {
            case GameState.Prepare:
                mainmenuGroup.SetActive(true);
                break;
            case GameState.Playing:
                gameplayGroup.SetActive(true);
                break;

        }
    }
    public void ResetUi()
    {
        mainmenuGroup.SetActive(false);
        gameplayGroup.SetActive(false);
    }

}
