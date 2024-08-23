using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Player player;
    [SerializeField] private ProjectilePoolingManager projectilePoolingManager;



    private void Start()
    {
        projectilePoolingManager.Init();
        player.Init();
        enemySpawner.Init();

        AssignEvent();

        enemySpawner.SetData(player.transform);
        player.SetData(new PlayerData
        {
            health = 100
        });
        enemySpawner.ActiveEnemies();


    }
    private void AssignEvent()
    {
        enemySpawner.AssignEvent(projectilePoolingManager.GenerateProjectilePool);
        player.AssignEvent(activeGameOver);
    }



    private void activeGameOver()
    {

    }
}
public static class TargetConstant
{
    public static string PLAYER = "Player";
    public static string ENEMY = "Enemy";
}
