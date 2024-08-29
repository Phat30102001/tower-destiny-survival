using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemySpawner : MonoBehaviour
{ 
    //private Transform target;
    [SerializeField] private List<Enemybase> enemyVariationPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform bottomSpawnBound;
    [SerializeField] private Transform topSpawnBound;
    
    private List<IEnemy> enemies;
    //[SerializeField] private int spawnAmount=10;
    //[SerializeField] private int spawnAtIndex = 0;
    [SerializeField] private List<EnemyData> enemyDatas;
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

    }
    public IEnumerator<float> ActiveEnemies(EnemyWaveData _data)
    {
        while (true)
        {
            string _randomKey = _data.EnemyId[UnityEngine.Random.Range(0, _data.EnemyId.Count)];
            generateEnemy(_randomKey);
            yield return Timing.WaitForSeconds(
                UnityEngine.Random.Range(_data.SpawnFrequencyMin,_data.SpawnFrequencyMax));
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

    private void generateEnemy(string _enemyId)
    {
        var _prefab= enemyVariationPrefab.Find(x=>x.GetEnemyId().Equals(_enemyId));
        var _data= enemyDatas.Find(x=>x.EnemyId.Equals(_enemyId));
        Enemybase _enemy = Instantiate(_prefab, spawnLocation);
        if (_enemy != null)
        {

            if (_enemy is ShootingEnemy shootingEnemy)
            {
                shootingEnemy.AssignEvent(onShooting);

            }

            _enemy.SetData(_data);
            _enemy.ActiveAction(targets[_enemy.GetCurrentTargetTag()],
                    new Vector2(spawnLocation.position.x, UnityEngine.Random.Range(bottomSpawnBoundPosY, topSpawnBoundPosY)));
            
                enemies.Add(_enemy);
        }

        
    }
}