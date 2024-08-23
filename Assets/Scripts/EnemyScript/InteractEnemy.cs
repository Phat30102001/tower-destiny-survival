using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractEnemy: MonoBehaviour,IEnemy
{
    [SerializeField] private float speed = 500f;
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private float attackDistance = 10;
    [SerializeField] private DamageSender damageSender;
    CoroutineHandle handle;
    private EnemyData enemyData;

    public void SetData(EnemyData _data)
    {
        enemyData = _data;
        damageSender.SetData(enemyData.AttackCooldown, enemyData.Damage,enemyData.TargetTag);
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
    private IEnumerator<float> Moving(Transform _target)
    {
        while (state == 0)
        {
            if (Vector3.Distance(objectTransform.position, _target.position) <= attackDistance)
            {
                state++;
            }
            objectTransform.position -= Vector3.right * speed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target,objectTransform.position);
    }
    private IEnumerator<float> Attacking(Transform _target)
    {

        while (state == 1)
        {
            if (Vector3.Distance(objectTransform.position, _target.position) > attackDistance)
            {
                state--;
            }
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target, objectTransform.position);
    }

}