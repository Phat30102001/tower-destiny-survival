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
    [SerializeField] private EnergyController energyController;
    private ResourceManager resourceManager;
    private DataHolder dataHolder;

    Action<ResultType> onEndGame;


    CoroutineHandle handleCheckTarget;
    CoroutineHandle handleGenEnergy;
    private WeaponBaseData weaponBaseData;
    private WeaponBaseData weaponTriggerSkillData;
    private Canvas canvas;

    public void ActiveGameplay(ResourceManager _resourceManager, Canvas _canvas)
    {
        resourceManager = _resourceManager;
        canvas= _canvas;    
        Init();

        setData();



        


    }

    
    public void Init()
    {
        dataHolder=DataHolder.instance;
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
        //weaponBaseData = new ShotgunData
        //{
        //    Uid=TargetConstant.PLAYER,
        //    Cooldown = 0.8f,
        //    WeaponId = WeaponIdConstant.SHOTGUN,
        //    DamageAmount = 10,
        //    ShootSpeed = 30,
        //    NumberPerRound = 3,
        //    FireSpreadOffset = 100,
        //    TargetTag = TargetConstant.ENEMY,
        //    ProjectileId = "ShotgunBullet",

        //};
        var _weaponKeyData = SaveGameManager.GetPlayerWeaponData();
        if (_weaponKeyData._weaponId == "")
        {
            weaponBaseData = SaveGameManager.SavePlayerWeaponData(WeaponIdConstant.SHOTGUN,1); 
        }
        else
        {
            weaponBaseData=dataHolder.GetWeaponData(_weaponKeyData._weaponId, _weaponKeyData._level);
        } 
        
        var _weaponSkillKeyData = SaveGameManager.GetPlayerWeaponTriggerSkillData();
        if (_weaponSkillKeyData._weaponId == "")
        {
            weaponTriggerSkillData = SaveGameManager.SavePlayerWeaponTriggerSkillData(WeaponIdConstant.GRENADE,1); 
        }
        else
        {
            weaponTriggerSkillData = dataHolder.GetWeaponData(_weaponSkillKeyData._weaponId, _weaponSkillKeyData._level);
        }

        weaponController.SpawnWeapon( weaponBaseData);
        weaponController.SpawnWeaponSkill(weaponTriggerSkillData);
        gameplayProgression.GetMilestone(waveController.GetWaveMilestones());
        turretManager.RefreshManager(canvas);
        

    }
    public void OnUseWeaponSkill(string _weaponId)
    {
        energyController.ConsumeEnergy((float)dataHolder.GetWeaponData(_weaponId, 1).EnergyRequire, () =>
        {
            weaponController.TriggerWeaponSkill(_weaponId);
        },null);
    }
    public bool CreateTurret()
    {
       return turretManager.GenerateTurret(dataHolder.GetTurretDataAtLevel(1),dataHolder.GetAllLv1Turretweapondata());
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
        WeaponBaseData _weaponData = dataHolder.GetWeaponData(_weaponId, _level);
        WeaponBaseData _nextLevelWeaponData = dataHolder.GetWeaponData(_weaponId, _level+1);

        
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
        turretManager.ActiveGameplay();
        energyController.SetEnergyData(SaveGameManager.LoadSaveEnergyData());
        weaponController.ActiveWeapon();
        handleCheckTarget = Timing.RunCoroutine(gameplayProgression.OnCheckEnemyInRange());
        handleGenEnergy=Timing.RunCoroutine(energyController.GenerateEnergy());
    }
    private void endGame()
    {
        Timing.KillCoroutines(handleCheckTarget);
        Timing.KillCoroutines(handleGenEnergy);
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
        Timing.KillCoroutines(handleCheckTarget);
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
    public static string GRENADE = "Grenade";
    public static string MACHINE_GUN = "MachineGun";
    public static string CHAINSAW = "ChainSaw";
    public static string FLAME_THROWER = "FlameThrower";
}

