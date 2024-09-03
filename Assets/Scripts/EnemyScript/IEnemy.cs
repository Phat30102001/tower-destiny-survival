using System;
using UnityEngine;
public interface IEnemy
{
    string GetEnemyId();
    void SetData(EnemyData data);
    void ActiveAction(Transform target, Vector2 position);
    bool CheckEnemyIsAlive();
    Vector2 getEnemyCurrentPos();
    string GetCurrentTargetTag();
    void SwitchEnemyTarget(string _targetTag);
}
//public interface IEnemy
//{
//    public string GetEnemyId();
//    public Vector2 getEnemyCurrentPos();
//    public bool CheckEnemyIsAlive();
//    public void ActiveAction(Transform _target, Vector2 _spawnPos);
//    public void SetData(EnemyData _data);
//    public string GetCurrentTargetTag();
//    public void SwitchEnemyTarget(string _targetTag);
//}

// Base class
public abstract class Enemybase : MonoBehaviour, IEnemy
{
    public abstract string GetEnemyId();
    public abstract void SetData(EnemyData data);
    public abstract void ActiveAction(Transform target, Vector2 position);
    public abstract bool CheckEnemyIsAlive();
    public abstract Vector2 getEnemyCurrentPos();
    public abstract string GetCurrentTargetTag();
    public abstract void SwitchEnemyTarget(string _targetTag);

}
//public class Enemybase : MonoBehaviour, IEnemy
//{
//    [SerializeField] private string EnemyId;
//    public void ActiveAction(Transform _target, Vector2 _spawnPos)
//    {
//        throw new NotImplementedException();
//    }

//    public bool CheckEnemyIsAlive()
//    {
//        throw new NotImplementedException();
//    }

//    public string GetCurrentTargetTag()
//    {
//        throw new NotImplementedException();
//    }

//    public Vector2 getEnemyCurrentPos()
//    {
//        throw new NotImplementedException();
//    }

//    public string GetEnemyId()
//    {
//        return EnemyId;
//    }

//    public void SetData(EnemyData _data)
//    {
//        throw new NotImplementedException();
//    }

//    public void SwitchEnemyTarget(string _targetTag)
//    {
//        throw new NotImplementedException();
//    }
//}

[Serializable]
public class EnemyData
{
    public string EnemyId;
    public int HealthPoint;
    public int Damage;
    public float AttackCooldown;
    public int CoinReceiveAmount;
    public string TargetTag = TargetConstant.TURRET;
    public string SubTargetTag = TargetConstant.PLAYER;
    public float MovingSpeed;
    public float AttackRange;
}