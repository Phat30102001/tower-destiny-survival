using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private DamageReceiver damageReceiver;
    private PlayerData playerData;
    [SerializeField] private Transform weaponContainer;
    private int healthPoint = 0;
    private Action onZeroHealthCallback;
    public void Init()
    {
        damageReceiver.AssignEvent(onReceiveDamage);
    }
    public void SetData(PlayerData _data)
    {
        playerData = _data;
        healthPoint = playerData.health;


    }
    public Transform GetWeaponCointainer()
    {
        return weaponContainer;
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
public class PlayerData
{
    public int health;
}
