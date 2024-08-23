using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public int damageAmount = 0;
    private float attackCooldown = 0;
    DamageReceiver receiver;
    public bool isInTrigger=false;
    CoroutineHandle handle;
    private string targetTag;
    private bool hideOnHit;
    private Action onHit;

    public void AssignEvent(Action _onHit)
    {
        onHit = null;
        onHit = _onHit;
    }
    public void SetData(float _attackCooldown,int _damageAmount,string _targetTag,bool _hideOnHit)
    {
        attackCooldown = _attackCooldown;
        damageAmount= _damageAmount;
        targetTag = _targetTag;
        hideOnHit = _hideOnHit;


    }
    public IEnumerator<float> ApplyDamage()
    {
        
        while (gameObject.activeSelf)
        {
            if (isInTrigger)
            {
                if (receiver && receiver.gameObject.tag.Equals(targetTag))
                {
                    //Debug.Log("coro still running");
                    SendDamage(receiver);
                    onHit?.Invoke();
                    if (hideOnHit)
                    {
                        resetData();
                    }
                    yield return Timing.WaitForSeconds(attackCooldown);
                }
                //Debug.Log($"attack cooldown: {attackCooldown}");
            }
            else
                yield return Timing.WaitForOneFrame;
            
        }
    }
    public void SendDamage(DamageReceiver receiver)
    {
        if (receiver != null)
        {
            if (!receiver.gameObject.tag.Equals(targetTag)) return;
            receiver.ReceiveDamage(damageAmount);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         receiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (receiver)
        {
            if (!receiver.gameObject.tag.Equals(targetTag)) return;
                isInTrigger = true;

        }
    }
    
    public void OnTriggerExit2D(Collider2D collision)
    {
        receiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (!receiver)
            isInTrigger = false;
    }
    private void OnEnable()
    {
        resetData();
        handle = Timing.RunCoroutine(ApplyDamage());
    }

    private void resetData()
    {
        isInTrigger = false;
        receiver = null;
        Timing.KillCoroutines(handle);
    }

}