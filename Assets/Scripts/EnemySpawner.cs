using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemySpawner : MonoBehaviour
{ 
    //private Transform target;
    [SerializeField] private List<GameObject> enemyVariationPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform bottomSpawnBound;
    [SerializeField] private Transform topSpawnBound;
    
    private List<IEnemy> enemies;
    [SerializeField] private int spawnAmount=10;
    [SerializeField] private int spawnAtIndex = 0;
    private bool isActive = false;
    private Dictionary<string,Transform> targets = new Dictionary<string,Transform>();

    private float bottomSpawnBoundPosY, topSpawnBoundPosY;

    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShooting;
    public void Init()
    {
        enemies = new List<IEnemy>();
        bottomSpawnBoundPosY=bottomSpawnBound.transform.position.y;
        topSpawnBoundPosY=topSpawnBound.transform.position.y;   
    }
    // player transform is for enemy who directly aim at player, attack point is for enemy who destroy the lowest turret
    public void SetData(Transform _attackPoint,Transform _playerTransform)
    {
        targets.Add(TargetConstant.TURRET, _attackPoint);
        targets.Add(TargetConstant.PLAYER, _playerTransform);
        generateEnemy();

    }
    public IEnumerator<float> ActiveEnemies()
    {
        if (!isActive)
        {

            isActive = true;
            foreach (var _enemy in enemies)
            {
                yield return Timing.WaitForSeconds(0.5f);
                //melee enemy data
                _enemy.SetData(new EnemyData
                {
                    AttackCooldown = 1f,
                    Damage=10,
                    HealthPoint=50,
                    CoinReceiveAmount=10,
                    MovingSpeed=500,
                    AttackRange=200,
                    TargetTag = TargetConstant.TURRET,
                });    
                //range enemy data
                //_enemy.SetData(new EnemyData
                //{
                //    AttackCooldown = 1f,
                //    Damage=10,
                //    HealthPoint=50,
                //    CoinReceiveAmount=10,
                //    MovingSpeed=500,
                //    AttackRange=900,
                //    TargetTag = TargetConstant.TURRET,
                //});
                _enemy.ActiveAction(targets[_enemy.GetCurrentTargetTag()], new Vector2(spawnLocation.position.x, UnityEngine.Random.Range(bottomSpawnBoundPosY, topSpawnBoundPosY)));
            }
        }
        yield break;
    }
    public void SwitchEnemyTarget()
    {
        foreach (var _enemy in enemies)
        {
            if (_enemy.CheckEnemyIsAlive())
            {
                _enemy.SwitchEnemyTarget(TargetConstant.PLAYER);
                _enemy.ActiveAction(targets[TargetConstant.PLAYER], _enemy.getEnemyCurrentPos());
            }
        }
    }
    public Vector2 GetClosestEnemyPos()
    {
        Vector2 minPos=Vector2.zero;
        foreach (var _enemy in enemies)
        {
            if (!_enemy.CheckEnemyIsAlive()) continue;
            if (minPos==Vector2.zero||minPos.x>_enemy.getEnemyCurrentPos().x)
            {
                minPos=_enemy.getEnemyCurrentPos();
            }
        }
        return minPos;
    }

    public void AssignEvent(Action<string, Vector2, Vector2,int,float,float, ProjectileData> _onShooting)
    {
        onShooting = _onShooting;
    }

    private void generateEnemy()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            int _index = spawnAtIndex;
            if (spawnAtIndex < 0 || spawnAtIndex > enemyVariationPrefab.Count)
            {
                _index = UnityEngine.Random.Range(0, enemyVariationPrefab.Count);
            }
            GameObject _ememyObject = Instantiate(enemyVariationPrefab[_index], spawnLocation);
            var _ememy = _ememyObject.GetComponent<IEnemy>();
            if (_ememy is ShootingEnemy shootingEnemy)
            {
                shootingEnemy.AssignEvent(onShooting);
            }
            if (_ememy != null)
                enemies.Add(_ememy);
        }
    }
}