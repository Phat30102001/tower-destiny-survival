using MEC;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Player player;
    [SerializeField] private ProjectilePoolingManager projectilePoolingManager;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private TurretManager turretManager;
    
    CoroutineHandle handle;
    private WeaponBaseData weaponBaseData;

    private GameplayState currentState=GameplayState.INIT;

    private void Start()
    {

        Init();

        AssignEvent();

        setData();

        currentState = GameplayState.PLAYING;

        startGame();
        


    }
    private void Init()
    {
        if(currentState!=GameplayState.INIT)return;
        projectilePoolingManager.Init();
        player.Init();
        enemySpawner.Init();

        
    }
    private void AssignEvent()
    {
        if (currentState != GameplayState.INIT) return;

        weaponController.AssignEvent(projectilePoolingManager.GenerateProjectilePool, enemySpawner.GetClosestEnemyPos);


        enemySpawner.AssignEvent(projectilePoolingManager.GenerateProjectilePool);
        turretManager.AssignEvent(enemySpawner.SwitchEnemyTarget);
        player.AssignEvent(activeGameOver);
    }

    private void setData()
    {
        if (currentState != GameplayState.INIT) return;


        enemySpawner.SetData(turretManager.GetTurretTransform(), player.transform);

        player.SetData(new PlayerData
        {
            health = 100
        });
        weaponController.SetData(player.GetWeaponCointainer());
        weaponBaseData = new ShotgunData
        {
            Cooldown = 10f,
            WeaponId = WeaponIdConstant.SHOTGUN,
            DamageAmount = 10,
            ShootForce = 3000,
            NumberPerRound = 3,
            FireSpreadOffset = 100,
            TargetTag = TargetConstant.ENEMY,
            ProjectileId="ShotgunBullet",

        };
        weaponController.SpawnWeapon(WeaponIdConstant.SHOTGUN, weaponBaseData);
        turretManager.GenerateTurret(new TurretData
        {
            HealthPoint = 100,

        });
    }
    private void startGame()
    {
        handle=Timing.RunCoroutine( enemySpawner.ActiveEnemies());
        turretManager.CheckAnyTurretAlive();
        weaponController.ActiveWeapon();
    }





    private void activeGameOver()
    {

    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(handle);
    }
}

public enum GameplayState { INIT,PLAYING,END}
public static class TargetConstant
{
    public static string PLAYER = "Player";
    public static string TURRET = "Turret";
    public static string ENEMY = "Enemy";
}
public static class WeaponIdConstant
{
    public static string SHOTGUN = "Shotgun";
}

