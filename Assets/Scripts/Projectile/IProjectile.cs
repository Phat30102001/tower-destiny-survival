using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IProjectile
    {
        public string GetProjectileId();
        public bool CheckIsAvailable();

        public void SetData(ProjectileData _data)
        {

        }

        public void Fire(Vector2 _startPosition,Vector2 _destination)
        {

        }
    }

    public struct ProjectileData
    {
        public float ShootForce;
        public int Damage;
        public string TargetTag;
        public bool HideOnHit;


    }
