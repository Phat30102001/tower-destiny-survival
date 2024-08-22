using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 500f;
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private float attackDistance=50;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootForce=1000;
    private string projectileName="LinearBullet";

    private Action<string, Vector2, Vector2, float> onShoot;
    CoroutineHandle handle;
    private EnemyData enemyData;

    public void SetData(EnemyData _data)
    {
        enemyData = _data;
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
                handle = Timing.RunCoroutine(Shooting(_target));
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
        ActiveAction(_target, objectTransform.position);
    }

    private IEnumerator<float> Shooting(Transform _target)
    {

        while (state == 1)
        {

            onShoot?.Invoke(projectileName, transform.position, _target.position, shootForce);
            yield return Timing.WaitForSeconds(2f);
        }
        ActiveAction(_target, objectTransform.position);
    }

    public void AssignEvent(Action<string, Vector2, Vector2, float> _onShoot)
    {
        onShoot = _onShoot;
    }
}