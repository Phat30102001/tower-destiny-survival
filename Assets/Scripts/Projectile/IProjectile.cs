using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IProjectile
    {
        public void SetData(ProjectileData _data)
        {

        }

        public void Fire(Vector3 _destination, float _shootForce)
        {

        }
    }

    public struct ProjectileData
    {
        public string ProjectileId;
        public float ShootForce;
        public int Damage;
        public string TargetTag;


    }
