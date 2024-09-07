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


    public void ActiveGameplay(ResourceManager _resourceManager)
    {
        resourceManager = _resourceManager;

        Init();

        setData();



        


    }

    
    public void Init()
    {
        projectilePoolingManager.Init();
        player.Init();
        waveController.Init();

        
    }
    public void AssignEvent(Action<ResultType> _onEndGame, Action<string, int> onUpgradeTurret, Action<string, string, int> onBuyWeapon)
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
        turretManager.RefreshManager();

    }
    public bool CreateTurret()
    {
       return turretManager.GenerateTurret(DataHolder.instance.GetTurretDataAtLevel(1),DataHolder.instance.GetAllLv1Turretweapondata());
    }
    public (string _weaponId,int _level) GetTurretWeaponId(string _turretId)
    {
        return (turretManager.GetWeaponIdTurretAtId(_turretId), turretManager.GetWeaponLevelTurretAtId(_turretId));
    }
    public void UpgradeTurret(TurretData _data,List<WeaponBaseData> _weaponDatas)
    {
        turretManager.UpgradeTurret(_data, _weaponDatas);
    }
    public bool BuyWeapon(string _turretId, string _weaponId,int _level)
    {
        WeaponBaseData _weaponData = DataHolder.instance.GetWeaponData(_weaponId, _level);
        WeaponBaseData _nextLevelWeaponData = DataHolder.instance.GetWeaponData(_weaponId, _level+1);

        
        _weaponData.Uid = _turretId;
        bool _isSuccess= weaponController.SpawnTurretWeapon(_weaponData,
             turretManager.GetTurretTransformAtId(_turretId));
        if(_isSuccess)
        {
            turretManager.SetTurretWeaponId(_turretId, _weaponData, _nextLevelWeaponData);
        }
        return _isSuccess;
    }
    public void StartGame()
    {
        //waveController.ActiveEnemies();
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

