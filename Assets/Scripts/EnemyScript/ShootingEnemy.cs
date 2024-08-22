using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 500f;
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private float shootingDistance=50;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootForce=1000;
    private string projectileName="LinearBullet";

    private Action<string, Vector2, Vector2, float> onShoot;
    CoroutineHandle handle;

    private void Start()
    {
        objectTransform = transform;
    }

    public void ActiveAction(Transform _target)
    {
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
            if (Vector3.Distance(objectTransform.position, _target.position) <= shootingDistance)
            {
                state++;
            }
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, _target.position, speed * Time.deltaTime);
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target);
    }

    private IEnumerator<float> Shooting(Transform _target)
    {

        while (state == 1)
        {

            onShoot?.Invoke(projectileName, transform.position, _target.position, shootForce);
            yield return Timing.WaitForSeconds(2f);
        }
        ActiveAction(_target);
    }

    public void AssignEvent(Action<string, Vector2, Vector2, float> _onShoot)
    {
        onShoot = _onShoot;
    }
}