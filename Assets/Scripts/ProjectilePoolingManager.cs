using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ProjectilePoolingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> projectilePrefabList;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private bool stopSpawning=false;

    public int poolSize = 10;
    private List<IProjectile> projectilePool;

    public void Init()
    {
        projectilePool = new List<IProjectile>();
    }

    public void GenerateProjectilePool(string projectileName, Vector2 _startPosition, Vector2 _targetPosition, float _shootForce)
    {
        if(stopSpawning) return;


        GameObject projectilePrefab = projectilePrefabList.Find(prefab =>
            prefab.GetComponent<IProjectile>().GetProjectileId().Equals(projectileName));

        if (projectilePrefab == null)
        {
            return;
        }
        foreach(var _item in projectilePool)
        {
            if(_item.GetProjectileId() == projectileName&& _item.CheckIsAvailable())
            {
                ActiveProjectile(_item, _startPosition, _targetPosition, _shootForce);
                return;
            }
        }

        GameObject projectileGameObject = Instantiate(projectilePrefab, parentTransform);
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        projectilePool.Add(projectile);
        if (projectile != null)
        {
            ActiveProjectile(projectile, _startPosition, _targetPosition, _shootForce);
        }
    }
    private void ActiveProjectile(IProjectile _projectile, Vector2 _startPosition, Vector2 _targetPosition, float _shootForce)
    {
        if (_projectile is Projectile _bullet)
        {
         
            _bullet.AssignEvent(() => ReturnToPool(_bullet.gameObject));
        }
        _projectile.SetData(new ProjectileData 
        { 
            Damage=10,
            ShootForce=_shootForce,
            TargetTag=TargetConstant.PLAYER
        });
        _projectile.Fire(_startPosition, _targetPosition, _shootForce);
    }


    public void ReturnToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }
   
}


