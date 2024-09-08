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
    [SerializeField] private TextMeshProUGUI energyRequireText;
    private void Awake()
    {
        useSkillButton.onClick.RemoveAllListeners();
        useSkillButton.onClick.AddListener(triggerSkill); 
    }
    public void SetData(WeaponSkillButtonData weaponSkillButtonData)
    {
        weaponId = weaponSkillButtonData.weaponId;
        buttonWeaponidText.text = weaponSkillButtonData.weaponId;
        energyRequireText.text = weaponSkillButtonData.EnergyRequire.ToString();
    }
    private void triggerSkill()
    {
        onUseSkill?.Invoke(weaponId);
    }
    public void AssignEvent(Action<string> _onUseSkill)
    {
        onUseSkill = _onUseSkill;
    }
}
public struct WeaponSkillButtonData
{
    public string weaponId;
    public int EnergyRequire;
}