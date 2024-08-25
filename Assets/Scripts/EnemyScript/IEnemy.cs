using UnityEngine;

public interface IEnemy
{
    public Vector2 getEnemyCurrentPos();
    public bool CheckEnemyIsAlive();
    public void ActiveAction(Transform _target, Vector2 _spawnPos);
    public void SetData(EnemyData _data);
    public string GetCurrentTargetTag();
    public void SwitchEnemyTarget(string _targetTag);
}
public class EnemyData
{
    public int HealthPoint;
    public int Damage;
    public float AttackCooldown;
    public int CoinReceiveAmount;
    public string TargetTag = TargetConstant.TURRET;
    public string SubTargetTag = TargetConstant.PLAYER;
    public float MovingSpeed;
    public float AttackRange;
}