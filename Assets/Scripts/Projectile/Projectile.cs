using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] private DamageSender damageSender;


    //[SerializeField] private Rigidbody2D rigidbody;
    private ProjectileData cachedProjectileData;
    private Action onDisable;
    [SerializeField] private string ProjectileId;
    [SerializeField] private float timeTillDisable=4f;
    CoroutineHandle coroWaitToDisable;
    Vector2 cacheDirection;
    public string GetProjectileId()
    {
        return ProjectileId;
    }

    

    public void SetData(ProjectileData _data)
    {
        cachedProjectileData = _data;
        damageSender.SetData(timeTillDisable, cachedProjectileData.Damage,cachedProjectileData.TargetTag,cachedProjectileData.HideOnHit);
        damageSender.AssignEvent(() =>
        {
            Timing.KillCoroutines(coroWaitToDisable);
            gameObject.SetActive(false);
        });
    }

    public void AssignEvent(Action _onDisable)
    {
        onDisable = _onDisable;
    }

    public void Fire(Vector2 _startPosition,Vector2 _destination)
    {
    gameObject.SetActive(true);
    transform.position = _startPosition;
    //Vector2 _direction = (_destination - _startPosition).normalized;
    cacheDirection = (_destination - _startPosition).normalized;
    float angle = Mathf.Atan2(cacheDirection.y, cacheDirection.x) * Mathf.Rad2Deg;
    //Debug.Log($"ActiveProjectile target: {_destination}");
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        

        coroWaitToDisable = Timing.RunCoroutine(coroFire());
    }

    private IEnumerator<float> coroFire()
    {
        float _cacheTimeTillDisable = timeTillDisable;
        while (_cacheTimeTillDisable>0)
        {
            transform.position = (Vector2)transform.position + cacheDirection * 10 * Time.deltaTime;
            _cacheTimeTillDisable -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
            
        }
        onDisable?.Invoke();
    }
    private void OnDisable()
    {
        Timing.KillCoroutines(coroWaitToDisable);
    }

    public bool CheckIsAvailable()
    {
        return !gameObject.activeSelf;
    }
}
    




