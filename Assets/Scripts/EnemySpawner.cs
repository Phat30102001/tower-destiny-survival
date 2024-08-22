using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{ 
    private Transform target;
    [SerializeField] private List<GameObject> enemyVariationPrefab;
    [SerializeField] private Transform spawnLocation;
    private List<IEnemy> enemies;
    [SerializeField] private int spawnAmount=10;
    [SerializeField] private int spawnAtIndex = 0;
    private bool isActive = false;

    private Action<string, Vector2, Vector2, float> onShooting;

    public void Init()
    {
        enemies = new List<IEnemy>();
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
            _enemy.ActiveAction(target);
        }
    }

    public void AssignEvent(Action<string, Vector2, Vector2, float> _onShooting)
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