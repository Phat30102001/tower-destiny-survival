using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class GameplayManager : MonoBehaviour
{
    
    [SerializeField] private Player player;
    [SerializeField] private ProjectilePoolingManager projectilePoolingManager;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private TurretManager turretManager;
    [SerializeField] private EnemyTracker enemyTracker;
    [SerializeField] private GameplayProgression gameplayProgression;
    [SerializeField] private EnemyWaveController waveController;
    private ResourceManager resourceManager;

    Action<ResultType> onEndGame;


    CoroutineHandle handle;
    private WeaponBaseData weaponBaseData;
    private DataHolder dataHolder;


    public void ActiveGameplay(ResourceManager _resourceManager, DataHolder _dataHolder)
    {
        resourceManager = _resourceManager;
        dataHolder= _dataHolder;

        Init();

        setData();



        


    }

    
    public void Init()
    {
        projectilePoolingManager.Init();
        player.Init();
        waveController.Init();

        
    }
    public void AssignEvent(Action<ResultType> _onEndGame, Action<string, int> onUpgradeTurret, Action<Action, string> onBuyWeapon)
    {


        onEndGame = _onEndGame;

        weaponController.AssignEvent(projectilePoolingManager.GenerateProjectilePool, waveController.GetClosestEnemyPos);


        waveController.AssignEvent(projectilePoolingManager.GenerateProjectilePool,(_resource)=> resourceManager.AddResource(ResourceConstant.COIN, _resource));
        turretManager.AssignEvent(waveController.SwitchEnemyTarget,weaponController.RemoveWeapon, onUpgradeTurret, onBuyWeapon);
        gameplayProgression.AssignEvent(enemyTracker.IsEnemyInArea,waveController.ActiveEnemies);
        player.AssignEvent(activeGameOver);
    }

    private void setData()
    {

        waveController.SetData(turretManager.GetTurretTransform(), player.transform);

        player.SetData(new PlayerData
        {
            health = 100
        });
        weaponController.SetData(player.GetWeaponCointainer());
        weaponBaseData = new ShotgunData
        {
            Uid=TargetConstant.PLAYER,
            Cooldown = 0.8f,
            WeaponId = WeaponIdConstant.SHOTGUN,
            DamageAmount = 10,
            ShootSpeed = 30,
            NumberPerRound = 3,
            FireSpreadOffset = 100,
            TargetTag = TargetConstant.ENEMY,
            ProjectileId = "ShotgunBullet",

        };
        var _turretWeaponBaseData = new MachineGunData
        {
            Uid = "0",
            Cooldown = 1f,
            WeaponId = WeaponIdConstant.MACHINE_GUN,
            DamageAmount = 10,
            ShootForce = 50,
            NumberPerRound = 3,
            TargetTag = TargetConstant.ENEMY,
            ProjectileId = "MachineGunBullet",

        };
        //var _turretWeaponBaseData = new ChainSawData
        //{
        //    Uid = "0",
        //    Cooldown = 0f,
        //    BreakTimeBetweenSendDamage = 0.2f,
        //    WeaponId = WeaponIdConstant.CHAINSAW,
        //    DamageAmount = 10,
        //    TargetTag = TargetConstant.ENEMY,

        //};  
        //var _turretWeaponBaseData = new FlameThrowerData
        //{
        //    Uid = "0",
        //    Cooldown = 5f,
        //    BreakTimeBetweenSendDamage = 0.2f,
        //    WeaponId = WeaponIdConstant.FLAME_THROWER,
        //    DamageAmount = 10,
        //    TargetTag = TargetConstant.ENEMY,
        //    MaxRotateAngle=45f,
        //    RrotationSpeed=20

        //};
        weaponController.SpawnWeapon( weaponBaseData);
        gameplayProgression.GetMilestone(waveController.GetWaveMilestones());
        weaponController.SpawnTurretWeapon(_turretWeaponBaseData, turretManager.GetTurretTransformAtId(_turretWeaponBaseData.Uid));
    }
    public bool CreateTurret()
    {
       return turretManager.GenerateTurret(dataHolder.GetTurretDataAtLevel(1));
    }
    public void UpgradeTurret(TurretData _data)
    {
        turretManager.UpgradeTurret(_data);
    }
    public void StartGame()
    {
        //waveController.ActiveEnemies();
        turretManager.CheckAnyTurretAlive();
        weaponController.ActiveWeapon();
        handle = Timing.RunCoroutine(gameplayProgression.OnCheckEnemyInRange());
    }
    private void endGame()
    {
        Timing.KillCoroutines(handle);
        waveController.onEndGame();
        onEndGame?.Invoke(ResultType.Lose);
        gameplayProgression.ResetData();
        weaponController.DisableWeapon();
    }





    private void activeGameOver()
    {
        endGame();
    }

    private void OnDestroy()
    {
        Timing.KillCoroutines(handle);
    }
}

//public enum GameplayState { INIT,PLAYING,END}
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

