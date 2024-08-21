using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class EnemySpawner : MonoBehaviour
{ 
    public Transform target;
    [SerializeField] private List<GameObject> enemyVariationPrefab;
    [SerializeField] private Transform spawnLocation;
    private List<IEnemy> enemies;
    [SerializeField] private int spawnAmount=10;
    [SerializeField] private int spawnAtIndex = 0;

    private Action<string> onShooting;

    public void Init()
    {
        enemies = new List<IEnemy>();
        generateEnemy();
    }
    public void ActiveEnemies()
    {
        foreach (var _enemy in enemies)
        {
            _enemy.ActiveAction(target);
        }
    }

    public void AssignEvent(Action<string> _onShooting)
    {
        onShooting = _onShooting;
    }

    private void generateEnemy()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            
            GameObject _ememyObject = Instantiate(enemyVariationPrefab[spawnAtIndex], spawnLocation);
            var _ememy = _ememyObject.GetComponent<IEnemy>();
            if (_ememy is ShootingEnemy shootingEnemy)
            {
                shootingEnemy.AssignEvent(onShooting);
            }
            if(_ememy != null)
                enemies.Add(_ememy);
        }
    }
}