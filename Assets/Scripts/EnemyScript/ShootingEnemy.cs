using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemybase
{
    [SerializeField] private string enemyId = "RangerZombie";
    private Transform objectTransform;
    private int state = 0;
    [SerializeField] private float shootForce=1000;
    [SerializeField] private DamageReceiver damageReceiver;
    private string projectileName="LinearBullet";
    private int healthPoint;
    private int _amount=1;
    private float _delayOffset=0;
    private float _shotSpread=0;

    public override string GetEnemyId()
    {
        return enemyId;
    }

    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    private Action<int> onEnemyDropResource;
    CoroutineHandle handle;
    private EnemyData enemyData;

    public override void SetData(EnemyData _data)
    {
        enemyData = _data;
        healthPoint=enemyData.HealthPoint;
        damageReceiver.AssignEvent(onReceiveDamage);
    }
    public override void ActiveAction(Transform _target, Vector2 _spawnPos)
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
    private void onReceiveDamage(int _amount)
    {
        healthPoint -= _amount;
        //Debug.Log($"{gameObject.name}'s health: {healthPoint}");
        if (healthPoint <= 0)
        {
            onEnemyDropResource?.Invoke(enemyData.CoinReceiveAmount);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator<float> Moving(Transform _target)
    {
        while (state == 0)
        {
            float _space = objectTransform.position.x - _target.position.x;
            if (_space <= enemyData.AttackRange)
            {
                state++;
            }
            objectTransform.position -= Vector3.right * enemyData.MovingSpeed * Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
        ActiveAction(_target, objectTransform.position);
    }

    private IEnumerator<float> Shooting(Transform _target)
    {

        while (state == 1)
        {

            onShoot?.Invoke(projectileName, transform.position, _target.position,_amount,_delayOffset,_shotSpread, new ProjectileData
            {
                Damage=enemyData.Damage,
                ShootSpeed=shootForce,
                TargetTag=enemyData.TargetTag,
                HideOnHit=true,

            });
            yield return Timing.WaitForSeconds(2f);
        }
        ActiveAction(_target, objectTransform.position);
    }

    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Action<int> _onEnemyDropResource)
    {
        onShoot = _onShoot;
        onEnemyDropResource = _onEnemyDropResource;
    }

    public override Vector2 getEnemyCurrentPos()
    {
        return objectTransform.position;
    }
    public override bool CheckEnemyIsAlive()
    {
        return healthPoint > 0;
    }
    private void OnDisable()
    {
        Timing.KillCoroutines(handle);
    }

    public override string GetCurrentTargetTag()
    {
        return enemyData.TargetTag;
    }

    public override void SwitchEnemyTarget(string _targetTag)
    {
        enemyData.TargetTag = _targetTag;
    }
}
