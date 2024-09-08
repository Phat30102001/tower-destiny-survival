using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkillButton : MonoBehaviour
{
    private string weaponId;
    Action<string> onUseSkill;
    [SerializeField] private Button useSkillButton;
    [SerializeField] private TextMeshProUGUI buttonWeaponidText;
    private void Awake()
    {
        useSkillButton.onClick.RemoveAllListeners();
        useSkillButton.onClick.AddListener(() => onUseSkill?.Invoke(weaponId)); 
    }
    public void SetData(WeaponSkillButtonData weaponSkillButtonData)
    {
        buttonWeaponidText.text = weaponSkillButtonData.weaponId;
    }
    public void AssignEvent(Action<string> _onUseSkill)
    {
        onUseSkill = _onUseSkill;
    }
}
public struct WeaponSkillButtonData
{
    public string weaponId;
}