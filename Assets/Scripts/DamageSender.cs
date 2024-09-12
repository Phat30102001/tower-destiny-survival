using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public int damageAmount = 0;
    private float attackCooldown = 0;
    private List<DamageReceiver> receivers = new List<DamageReceiver>(); // Sử dụng danh sách để lưu tất cả các DamageReceiver
    public bool isInTrigger = false;
    CoroutineHandle handle;
    private string targetTag;
    private bool hideOnHit;
    private Action onHit;
    private bool isActive = true;

    private HashSet<string> receiverTags = new HashSet<string>(); // Sử dụng HashSet để theo dõi các tag của các đối tượng

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
        if (isActive && receivers.Count > 0)
        {
            // Sao chép danh sách receivers sang một danh sách tạm thời
            var receiversCopy = new List<DamageReceiver>(receivers);

            foreach (var receiver in receiversCopy)
            {
                if (receiver != null && receiverTags.Contains(targetTag))
                {
                    SendDamage(receiver);
                    onHit?.Invoke();
                    if (hideOnHit)
                    {
                        yield break; // Dừng Coroutine sau khi áp dụng damage nếu hideOnHit là true
                    }
                }
            }
            yield return Timing.WaitForSeconds(attackCooldown);
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

        var damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null && collision.gameObject.CompareTag(targetTag))
        {
            if (!receivers.Contains(damageReceiver))
            {
                receivers.Add(damageReceiver);
                receiverTags.Add(collision.gameObject.tag);
                //Debug.Log($"{gameObject.name} add target {collision.gameObject.name}");
            }
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver != null && receivers.Contains(damageReceiver))
        {
            receivers.Remove(damageReceiver);
            //receiverTags.Remove(collision.gameObject.tag);
        }
        if (receivers.Count == 0)
        {
            isInTrigger = false;
            receivers.Clear(); // Xóa danh sách receivers
            //receiverTags.Clear(); // Xóa danh sách receiverTags
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
        receivers.Clear(); // Xóa danh sách receivers
        //receiverTags.Clear(); // Xóa danh sách receiverTags
        Timing.KillCoroutines(handle);
    }

    public void SetDamageSenderStatus(bool _isActive)
    {
        isActive = _isActive;
    }

    public bool CheckDamageSenderStatus() => isActive;
}
