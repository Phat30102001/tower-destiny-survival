using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectilePoolingManager : MonoBehaviour
{
    [SerializeField] private List<IProjectile> projectilePool;
    [SerializeField] private List<IProjectile> projectilePrefabList;
    
    [SerializeField] private Transform parentTransform;
    public int poolSize = 10;
    
    
    public void Init()
    {
        projectilePool=new List<IProjectile>();
    }

    public void GenerateProjectilePool(string projectileName)
    {
        // GameObject _projectilePrefab= projectilePrefabList.Find(x => x.GetProjectileId().Equal(projectileName));
        //     // Instantiate projectile
        //     GameObject projectileGameObject = Instantiate(_projectilePrefab, parentTransform);
        //
        //     var projectile = projectileGameObject.GetComponent<IProjectile>();
        //     
        //         projectile.Fire(_target.position,shootForce);
        //
    }


    public void ReturnToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }
}

