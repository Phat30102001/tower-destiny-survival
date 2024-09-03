using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField] List<EnemyWaveData> waveDatas;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] EnemyBaseController enemyBaseController;
    [SerializeField] private Transform inSceneTransform;
    CoroutineHandle handle;
    public void Init()
    {
        enemySpawner.Init();
    }
    public void SetData(Transform _attackPoint, Transform _playerTransform)
    {
        enemySpawner.SetData(_attackPoint, _playerTransform);
    }
    public List<int> GetWaveMilestones()
    {
        List<int> _milestone = new List<int>();
        foreach (EnemyWaveData _waveData in waveDatas)
        {
            _milestone.Add(_waveData.SpawnPoint);
        }
        return _milestone;
    }
    public Vector2 GetClosestEnemyPos()
    {
        return enemySpawner.GetClosestEnemyPos();
    }
    public void SwitchEnemyTarget()
    {
        enemySpawner.SwitchEnemyTarget();
    }
    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShooting, Action<int> _onEnemyDropResource)
    {
        enemySpawner.AssignEvent(_onShooting, onGetSpawnPos, _onEnemyDropResource);
    }
    private Vector2 onGetSpawnPos()
    {
        return enemyBaseController.transform.position;
    }
    public void ActiveEnemies(int _index) 
    {
        Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(enemySpawner.ActiveEnemies(waveDatas[_index]));
        if(waveDatas[_index].IsSpawnAtBase)
        {
            enemyBaseController.SetData(waveDatas[_index].BaseData);
            enemyBaseController.RunToScene(inSceneTransform.position);
        }
    }
   
    public void onEndGame()
    {
        Timing.KillCoroutines(handle);
        enemySpawner.ClearAllEnemy();
        enemyBaseController.OnBaseDestroyed();
    }
        
}
[Serializable]
public class EnemyWaveData
{
    public List<string> EnemyId = new List<string>();
    public float SpawnFrequencyMax, SpawnFrequencyMin;
    public int SpawnPoint;
    public bool IsSpawnAtBase;
    public EnemyBaseData BaseData;
}
