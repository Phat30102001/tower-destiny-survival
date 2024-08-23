using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{ 
    private Transform target;
    [SerializeField] private List<GameObject> enemyVariationPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform bottomSpawnBound;
    [SerializeField] private Transform topSpawnBound;
    
    private List<IEnemy> enemies;
    [SerializeField] private int spawnAmount=10;
    [SerializeField] private int spawnAtIndex = 0;
    private bool isActive = false;

    private float bottomSpawnBoundPosY, topSpawnBoundPosY;

    private Action<string, Vector2, Vector2, ProjectileData> onShooting;

    public void Init()
    {
        enemies = new List<IEnemy>();
        bottomSpawnBoundPosY=bottomSpawnBound.transform.position.y;
        topSpawnBoundPosY=topSpawnBound.transform.position.y;   
    }
    public void SetData(Transform _playerTransform)
    {
        target= _playerTransform;
        generateEnemy();

    }
    public void ActiveEnemies()
    {
        if (isActive) return;
        isActive = true;
        foreach (var _enemy in enemies)
        {
            _enemy.SetData(new EnemyData
            {
                AttackCooldown = 1f,
                Damage=10,
                HealthPoint=50,
                CoinReceiveAmount=10,
                MovingSpeed=500,
                AttackRange=900,
            });
            _enemy.ActiveAction(target, new Vector2(spawnLocation.position.x, UnityEngine.Random.Range(bottomSpawnBoundPosY, topSpawnBoundPosY)));
        }
    }

    public void AssignEvent(Action<string, Vector2, Vector2, ProjectileData> _onShooting)
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