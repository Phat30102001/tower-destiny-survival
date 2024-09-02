using System;
using UnityEngine;

public class EnemyBaseController: MonoBehaviour
{
    private EnemyBaseData enemyData;
    private float cacheDefaultPosX;
    [SerializeField] private DamageReceiver damageReceiver;
    public void Init()
    {
        cacheDefaultPosX = transform.position.x;
        damageReceiver.AssignEvent(onHit);
    }

    public void SetData(EnemyBaseData _data)
    { 
        enemyData = _data;
        gameObject.SetActive(true);
    }
    private void onHit(int _damage)
    {
        enemyData.HealthPoint -= _damage;
        if (enemyData.HealthPoint <= 0)
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
        // move object to the cacheDefaultPosX
        transform.position = new Vector2(cacheDefaultPosX, transform.position.y);
    }

}
[Serializable]
public class EnemyBaseData
{
    public int HealthPoint;
    public int CoinReceiveAmount;
}