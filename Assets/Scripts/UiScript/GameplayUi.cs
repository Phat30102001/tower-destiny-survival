using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUi : UiBase
{
    [SerializeField] private string uid = UiConstant.GAMEPLAY_UI;
    private Action<string> onUseEnergy;

    private List<WeaponSkillButton> weaponButtons=new() ;
    [SerializeField] private WeaponSkillButton weaponButtonPrefab;
    [SerializeField] private Transform weaponButtonGroup;
    public override string GetUiId()
    {
        return uid;
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void SetData(UiBaseData data)
    {
        if (data is GameplayUiData _gameplayUiData)
        {
            GameplayUiData gameplayUiData = _gameplayUiData;
            GenerateWeaponSkillButtons(gameplayUiData.WeaponSkillButtonDatas);

        }
    }
    private void GenerateWeaponSkillButtons(List<WeaponSkillButtonData> _datas)
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
                WeaponSkillButton _weaponButton = Instantiate(weaponButtonPrefab, weaponButtonGroup);
                _weaponButton.SetData(_datas[i]);
                _weaponButton.AssignEvent(onUseEnergy);
                weaponButtons.Add(_weaponButton);
            }
        }
    }
    private void refreshWeaponButton()
    {
        foreach (var weaponButton in weaponButtons)
        {
            weaponButton.gameObject.SetActive(false);
        }
    }
    public void AssignEvents(Action<string> _onUseEnergy )
    {
        onUseEnergy = _onUseEnergy;
    }
    public override void Show()
    {
        gameObject.SetActive(true);
    }
}
public class GameplayUiData : UiBaseData
{
    public List<WeaponSkillButtonData> WeaponSkillButtonDatas;
}
