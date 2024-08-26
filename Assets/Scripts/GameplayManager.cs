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
        turretManager.AssignEvent(enemySpawner.SwitchEnemyTarget,weaponController.RemoveWeapon);
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
            Uid=TargetConstant.PLAYER,
            Cooldown = 10f,
            WeaponId = WeaponIdConstant.SHOTGUN,
            DamageAmount = 10,
            ShootForce = 3000,
            NumberPerRound = 3,
            FireSpreadOffset = 100,
            TargetTag = TargetConstant.ENEMY,
            ProjectileId = "ShotgunBullet",

        };
        //var _turretWeaponBaseData = new MachineGunData
        //{
        //    Uid = "0",
        //    Cooldown = 1f,
        //    WeaponId = WeaponIdConstant.MACHINE_GUN,
        //    DamageAmount = 10,
        //    ShootForce = 3000,
        //    NumberPerRound = 3,
        //    TargetTag = TargetConstant.ENEMY,
        //    ProjectileId = "MachineGunBullet",

        //};
        //var _turretWeaponBaseData = new ChainSawData
        //{
        //    Uid = "0",
        //    Cooldown = 0f,
        //    BreakTimeBetweenSendDamage = 0.2f,
        //    WeaponId = WeaponIdConstant.CHAINSAW,
        //    DamageAmount = 10,
        //    TargetTag = TargetConstant.ENEMY,

        //};  
        var _turretWeaponBaseData = new FlameThrowerData
        {
            Uid = "0",
            Cooldown = 5f,
            BreakTimeBetweenSendDamage = 0.2f,
            WeaponId = WeaponIdConstant.FLAME_THROWER,
            DamageAmount = 10,
            TargetTag = TargetConstant.ENEMY,
            MaxRotateAngle=45f,
            RrotationSpeed=20

        };
        weaponController.SpawnWeapon( weaponBaseData);
        turretManager.GenerateTurret(new TurretData
        {
            HealthPoint = 100,
            TurretId = "0",

        });
        weaponController.SpawnTurretWeapon(_turretWeaponBaseData,turretManager.GetTurretTransformAtId(_turretWeaponBaseData.Uid));
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
    public static string MACHINE_GUN = "MachineGun";
    public static string CHAINSAW = "ChainSaw";
    public static string FLAME_THROWER = "FlameThrower";
}

