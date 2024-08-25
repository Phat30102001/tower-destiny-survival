using System;
using UnityEngine;

public class Turret: MonoBehaviour
{
    private TurretData turretData;
    [SerializeField] DamageReceiver damageReceiver;
    private int healthPoint;
    private Action onZeroHealthCallback;
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
        }
    }
    public void AssignEvent(Action _onZeroHealthCallback)
    {
        onZeroHealthCallback = _onZeroHealthCallback;
    }
}
public class TurretData
{
    public int HealthPoint;
    public string WeaponId;
}