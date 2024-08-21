using System;
using UnityEngine;

    public class Projectile : MonoBehaviour,IProjectile
    {
        [SerializeField] private Rigidbody2D rigidbody;
        private ProjectileData cachedProjectileData;
        private Action onDisable;


        public string GetProjectileId()
        {
            return cachedProjectileData.ProjectileId;
        }
    

        public void SetData(ProjectileData _data)
        {
            cachedProjectileData = _data;
        }

        public void AssignEvent(Action _onDisable)
        {
            onDisable = _onDisable;
        }
    
        public  void Fire(Vector3 _destination,float _shootForce)
        {
            // Add force to the projectile
            if (rigidbody != null)
            {
                Vector3 direction = (_destination - transform.position).normalized;
                rigidbody.AddForce(direction * _shootForce, ForceMode2D.Impulse);
            }
        }
    }
    




