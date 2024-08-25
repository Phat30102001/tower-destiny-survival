using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractEnemy : MonoBehaviour, IEnemy
{
    public string tag;
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private DamageSender damageSender;
    [SerializeField] private DamageReceiver damageReceiver;
    CoroutineHandle handle;
    private EnemyData enemyData;
    private int healthPoint;

    private void Update()
    {
        if (enemyData != null)
            tag = enemyData.TargetTag;
    }
    public void SetData(EnemyData _data)
    {
        enemyData = _data;
        healthPoint=enemyData.HealthPoint;
        damageSender.SetData(enemyData.AttackCooldown, enemyData.Damage,enemyData.TargetTag,false);
        damageReceiver.AssignEvent(onReceiveDamage);
    }
    public void ActiveAction(Transform _target, Vector2 _spawnPos)
    {
        if(!objectTransform) objectTransform = transform;
        objectTransform.position = _spawnPos;
        Timing.KillCoroutines(handle);
        switch (state)
        {
            case 0:
                handle = Timing.RunCoroutine(Moving(_target));
                break;
            case 1:
                handle = Timing.RunCoroutine(Attacking(_target));
                break;
        }

    }
    private void onReceiveDamage(int _amount)
    {
        healthPoint -= _amount;
        //Debug.Log($"{gameObject.name}'s health: {healthPoint}");
        if (healthPoint <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator<float> Moving(Transform _target)
    {
        while (state == 0)
        {
            if (Vector3.Distance(objectTransform.position, _target.position) <= enemyData.AttackRange)
            {
                state++;
            }
            objectTransform.position -= Vector3.right * enemyData.MovingSpeed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target,objectTransform.position);
    }
    private IEnumerator<float> Attacking(Transform _target)
    {

        while (state == 1)
        {
            if (Vector3.Distance(objectTransform.position, _target.position) > enemyData.AttackRange)
            {
                state--;
            }
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target, objectTransform.position);
    }

    public Vector2 getEnemyCurrentPos()
    {
        return objectTransform.position;
    }
    private void OnDisable()
    {
        Timing.KillCoroutines(handle);
    }

    public bool CheckEnemyIsAlive()
    {
        return healthPoint > 0;
    }

    public string GetCurrentTargetTag()
    {
        return enemyData.TargetTag;
    }

    public void SwitchEnemyTarget(string _targetTag)
    {
        enemyData.TargetTag = _targetTag;
        damageSender.SetData(enemyData.AttackCooldown, enemyData.Damage, enemyData.TargetTag, false);
    }
}