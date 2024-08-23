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

    public void GenerateProjectilePool(string projectileName, Vector2 _startPosition, Vector2 _targetPosition, int _amount, float _delayOffset, float _shotSpread, ProjectileData _data)
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
                ActiveProjectile(_item, _startPosition, _targetPosition, _amount, _delayOffset, _shotSpread, _data);
                return;
            }
        }

        GameObject projectileGameObject = Instantiate(projectilePrefab, parentTransform);
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        projectilePool.Add(projectile);
        if (projectile != null)
        {
            ActiveProjectile(projectile, _startPosition, _targetPosition, _amount, _delayOffset, _shotSpread, _data);
        }
    }
    private void ActiveProjectile(IProjectile _projectile, Vector2 _startPosition, Vector2 _targetPosition, int _amount,float _delayOffset, float shotSpread, ProjectileData _data)
    {
        if (_projectile is Projectile _bullet)
        {
         
            _bullet.AssignEvent(() => ReturnToPool(_bullet.gameObject));
        }
        _projectile.SetData(_data);
        _projectile.Fire(_startPosition, _targetPosition);
    }


    public void ReturnToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }
   
}


