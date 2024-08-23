using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public Action<int> onReceiveDamageCallback;

    public void ReceiveDamage(int damageAmount)
    {
        //Debug.Log($"{gameObject.name} received {damageAmount} damage");
        onReceiveDamageCallback?.Invoke(damageAmount);
    }

    public void AssignEvent( Action<int> _onReceiveDamageCallback)
    {
        onReceiveDamageCallback = _onReceiveDamageCallback;
    }
}