using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private ProjectilePoolingManager projectilePoolingManager;



    private void Start()
    {
        projectilePoolingManager.Init();
        enemySpawner.AssignEvent(projectilePoolingManager.GenerateProjectilePool);
        enemySpawner.Init();
        
    }

    private void Update()
    {
        enemySpawner.ActiveEnemies();
    }
}
