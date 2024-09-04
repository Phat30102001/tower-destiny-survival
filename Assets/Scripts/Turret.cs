using System;
using UnityEngine;

public class Turret: MonoBehaviour
{
    private TurretData turretData;
    [SerializeField] DamageReceiver damageReceiver;
    [SerializeField] Transform weaponContainer;
    private int healthPoint;
    private Action onZeroHealthCallback;
    Action<string> onDisableWeapon;
    public void SetData(TurretData _data)
    {
        turretData = _data;
        healthPoint=turretData.HealthPoint;
        damageReceiver.AssignEvent(onReceiveDamage);
    }
    private void onReceiveDamage(int _amount)
    {
        healthPoint -= _amount;
        //Debug.Log($"{gameObject.name}'s health: {healthPoint}");
        if (healthPoint <= 0)
        {
            onZeroHealthCallback?.Invoke();
            onDisableWeapon?.Invoke(turretData.TurretId);
        }
    }
    public void AssignEvent(Action _onZeroHealthCallback, Action<string> _onDisableWeapon)
    {
        onDisableWeapon = _onDisableWeapon;
        onZeroHealthCallback = _onZeroHealthCallback;
    }
    public Transform GetWeaponCointainer()
    {
        return weaponContainer;
    }
}
[Serializable]
public class TurretData
{
    public string TurretId;
    public int HealthPoint;
    public string WeaponId;
    public ResourceData priceData;
}