using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
   
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI weaponLevelText;
    [SerializeField] private TextMeshProUGUI weaponPriceText;
    [SerializeField] private TextMeshProUGUI weaponId;
    private WeaponButtonData data;

    public void SetData(WeaponButtonData _data)
    {
        data = _data;
        weaponLevelText.text =  $"Lv.{data.weaponLevel}";
        weaponPriceText.text = data.weaponPrice.ToString();
        weaponId.text = data.weaponId;
    }
    public void AssignEvent(Action<string, int> _onBuyWeapon)
    {
        buyButton.onClick.AddListener(() => _onBuyWeapon?.Invoke(data.weaponId,data.weaponLevel));
    }
}
public class WeaponButtonData
{
    public int weaponLevel;
    public int weaponPrice;
    public string weaponId;

}
