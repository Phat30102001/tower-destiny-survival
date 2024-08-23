using MEC;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public int damageAmount = 0;
    private float attackCooldown = 0;
    DamageReceiver receiver;
    private bool isInTrigger=false;
    CoroutineHandle handle;
    public void SetData(float _attackCooldown,int _damageAmount)
    {
        attackCooldown = _attackCooldown;
        damageAmount= _damageAmount;
        handle=Timing.RunCoroutine (ApplyDamage());
    }
    public IEnumerator<float> ApplyDamage()
    {
        
        while (gameObject.activeSelf)
        {
            if (isInTrigger)
            {
                SendDamage(receiver);
                yield return Timing.WaitForSeconds(attackCooldown);
                Debug.Log($"attack cooldown: {attackCooldown}");
            }
            else
                yield return Timing.WaitForOneFrame;
            
        }
    }
    public void SendDamage(DamageReceiver receiver)
    {
        if (receiver != null)
        {
            receiver.ReceiveDamage(damageAmount);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         receiver = collision.gameObject.GetComponent<DamageReceiver>();
        if(receiver)
            isInTrigger = true;
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        receiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (!receiver)
            isInTrigger = false;
    }
    private void OnDisable()
    {
        Timing.KillCoroutines(handle);
    }

}