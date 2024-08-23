using MEC;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private DamageSender damageSender;


        [SerializeField] private Rigidbody2D rigidbody;
        private ProjectileData cachedProjectileData;
        private Action onDisable;
        [SerializeField] private string ProjectileId;
        [SerializeField] private float timeTillDisable=4f;
        CoroutineHandle coroWaitToDisable;

        public string GetProjectileId()
        {
            return ProjectileId;
        }

    

        public void SetData(ProjectileData _data)
        {
            cachedProjectileData = _data;
            damageSender.SetData(timeTillDisable, cachedProjectileData.Damage,cachedProjectileData.TargetTag);
        }

        public void AssignEvent(Action _onDisable)
        {
            onDisable = _onDisable;
        }

        public void Fire(Vector2 _startPosition,Vector2 _destination, float _shootForce)
        {
        gameObject.SetActive(true);
        transform.position = _startPosition;
            if (rigidbody != null)
            {
                Vector2 direction = (_destination - _startPosition).normalized;
                rigidbody.AddForce(direction * _shootForce, ForceMode2D.Impulse);
            }

        coroWaitToDisable = Timing.CallDelayed(timeTillDisable, () => onDisable?.Invoke());
    }

    public bool CheckIsAvailable()
    {
        return !gameObject.activeSelf;
    }
}
    




