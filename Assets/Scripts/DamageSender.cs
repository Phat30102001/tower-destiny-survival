using UnityEngine;

public class DamageSender : MonoBehaviour
{
    public int damageAmount = 10;


    public void SendDamage(DamageReceiver receiver)
    {
        if (receiver != null)
        {
            receiver.ReceiveDamage(damageAmount);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageReceiver receiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            SendDamage(receiver);
        }
    }

}