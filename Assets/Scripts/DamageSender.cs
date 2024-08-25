using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public int damageAmount = 0;
    private float attackCooldown = 0;
    private DamageReceiver receiver;
    public bool isInTrigger = false;
    CoroutineHandle handle;
    private string targetTag;
    private bool hideOnHit;
    private Action onHit;

    private string receiverTag;

    public void AssignEvent(Action _onHit)
    {
        onHit = _onHit;
    }

    public void SetData(float _attackCooldown, int _damageAmount, string _targetTag, bool _hideOnHit)
    {
        attackCooldown = _attackCooldown;
        damageAmount = _damageAmount;
        targetTag = _targetTag;
        hideOnHit = _hideOnHit;
    }

    public void SwitchTargetTag(string _targetTag)
    {
        targetTag = _targetTag;
    }

    public IEnumerator<float> ApplyDamage()
    {
        while (gameObject.activeSelf)
        {
            if (isInTrigger && receiver != null)
            {
                if (receiverTag.Equals(targetTag))
                {
                    SendDamage(receiver);
                    onHit?.Invoke();
                    if (hideOnHit)
                    {
                        resetData();
                    }
                    yield return Timing.WaitForSeconds(attackCooldown);
                }
                else
                {
                    resetData();
                }
            }
            else
            {
                yield return Timing.WaitForOneFrame;
            }
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
        if (targetTag == null || targetTag == "") return;
        receiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (receiver != null && collision.gameObject.CompareTag(targetTag))
        {
            receiverTag = collision.gameObject.tag;
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageReceiver>() == receiver)
        {
            isInTrigger = false;
            receiver = null;
            receiverTag = null;
        }
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
        receiverTag = null;
        Timing.KillCoroutines(handle);
    }
}
