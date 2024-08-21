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

    public void Init()
    {
        
    }
    private void Start()
    {
        enemies = new List<IEnemy>();
         generateEnemy();
    }

    private void Update()
    {
        foreach (var _enemy in enemies)
        {
            _enemy.ActiveAction(target);
        }
    }

    private void generateEnemy()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            
            GameObject _ememyObject = Instantiate(enemyVariationPrefab[0], spawnLocation);
            var _ememy = _ememyObject.GetComponent<IEnemy>();
            enemies.Add(_ememy);
        }
    }
}