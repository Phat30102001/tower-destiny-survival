using System;
using UnityEngine;

public class EnemyBaseController: MonoBehaviour
{
    private EnemyBaseData enemyData;
    private float cacheDefaultPosX;
    [SerializeField] private DamageReceiver damageReceiver;
    private int healthPoint=0;
    private Action onBaseDestroyed;
    public void Init()
    {
        cacheDefaultPosX = transform.position.x;
        damageReceiver.AssignEvent(onHit);
    }

    public void SetData(EnemyBaseData _data)
    { 
        enemyData = _data;
        healthPoint = enemyData.HealthPoint;
        gameObject.SetActive(true);
    }
    private void onHit(int _damage)
    {
        healthPoint -= _damage;
        if (healthPoint <= 0)
        {
            OnBaseDestroyed();
        }
    }
    public void RunToScene(Vector2 _inSceneTransform) {
        // move object to the _inSceneTransform

        if (enemyData != null) {
            transform.position=new Vector2(_inSceneTransform.x, transform.position.y);
        }






    }
    public void OnBaseDestroyed()
    {
        gameObject.SetActive(false);
        onBaseDestroyed?.Invoke();
        // move object to the cacheDefaultPosX
        transform.position = new Vector2(cacheDefaultPosX, transform.position.y);
    }
    public void AssignEvent(Action _onBaseDestroyed)
    {
        onBaseDestroyed = _onBaseDestroyed;
    }

}
[Serializable]
public class EnemyBaseData
{
    public int HealthPoint;
    public int CoinReceiveAmount;
}