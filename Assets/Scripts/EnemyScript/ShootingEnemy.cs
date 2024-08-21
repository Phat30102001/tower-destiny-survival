using System;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 500f;
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private float shootingDistance=50;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootForce=10;
    private string projectileName="linearBullet";

    private Action<string> onShoot;

    private void Start()
    {
        objectTransform = transform;
    }

    public void ActiveAction(Transform _target)
    {
        switch (state)
        {
            case 0:
                Moving(_target);
                break;
            case 1:
                Shooting(_target);
                break;
        }
        
    }

    private void Moving(Transform _target)
    {
        if (Vector3.Distance(objectTransform.position, _target.position) <= shootingDistance)
        {
            state++;
            return;
        }
        objectTransform.position = Vector3.MoveTowards(objectTransform.position, _target.position, speed * Time.deltaTime);
    }

    private void Shooting(Transform _target)
    {
        onShoot?.Invoke(projectileName);
        // if (projectilePrefab != null && _target != null)
        // {
        //     // Instantiate projectile
        //     GameObject projectileGameObject = Instantiate(projectilePrefab, objectTransform);
        //
        //     var projectile = projectileGameObject.GetComponent<IProjectile>();
        //     
        //         projectile.Fire(_target.position,shootForce);
        // }
    }

    public void AssignEvent(Action<string> _onShoot)
    {
        onShoot = _onShoot;
    }
}