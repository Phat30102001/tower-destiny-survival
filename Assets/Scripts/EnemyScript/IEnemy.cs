using UnityEngine;

public interface IEnemy
{
    
    public void ActiveAction(Transform _target,Vector2 _spawnPos)
    {
        
    }
    public void SetData(EnemyData _data)
    {
    }
}
public class EnemyData
{
    public int HealthPoint;
    public int Damage;
    public float AttackCooldown;
    public int CoinReceiveAmount;
}