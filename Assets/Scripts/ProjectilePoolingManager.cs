using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using MEC;
using Random = UnityEngine.Random;

public class ProjectilePoolingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> projectilePrefabList;
    [SerializeField] private Transform parentTransform;

    public int poolSize = 10;
    private List<IProjectile> projectilePool;

    public void Init()
    {
        projectilePool = new List<IProjectile>();
    }

    public void GenerateProjectilePool(string projectileName, Vector2 _startPosition, Vector2 _targetPosition, int _amount, float _delayOffset, float _shotSpread, ProjectileData _data)
    {
        //Debug.Log($"GenerateProjectilePool target: {_targetPosition}");

        GameObject projectilePrefab = projectilePrefabList.Find(prefab =>
            prefab.GetComponent<IProjectile>().GetProjectileId().Equals(projectileName));

        if (projectilePrefab != null)
        {

            int _cacheAmount = _amount;
            foreach (var _item in projectilePool)
            {
                if(_item.GetProjectileId() == projectileName&& _item.CheckIsAvailable())
                {
                    ActiveProjectile(_item, _startPosition, _targetPosition, _delayOffset, _shotSpread, _data);
                    _cacheAmount--;
                    
                    if (_cacheAmount <= 0)
                        break;
                }
            }
            if (_cacheAmount > 0)
            {
                for (int i = 0; i < _cacheAmount; i++)
                {

                    GameObject projectileGameObject = Instantiate(projectilePrefab, parentTransform);
                    IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
                    projectilePool.Add(projectile);
                    if (projectile != null)
                    {
                        ActiveProjectile(projectile, _startPosition, _targetPosition, _delayOffset, _shotSpread, _data);
                    }
                }
            }
        }


    }
    private void ActiveProjectile(IProjectile _projectile, Vector2 _startPosition, Vector2 _targetPosition,float _delayOffset, float shotSpread, ProjectileData _data)
    {
        if (_projectile is Projectile _bullet)
        {
         
            _bullet.AssignEvent(() => ReturnToPool(_bullet.gameObject));
        }
        _projectile.SetData(_data);
        float yOffset = Random.Range(-shotSpread, shotSpread);
        Vector2 _destination=new Vector2(_targetPosition.x,_targetPosition.y+yOffset);
        //Debug.Log($"ActiveProjectile target: {_targetPosition}. start pos: {_startPosition}");
        _projectile.Fire(_startPosition,_targetPosition);
    }


    public void ReturnToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }
   
}


