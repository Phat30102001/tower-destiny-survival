using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlameThrower : MonoBehaviour, IWeapon
{
    [SerializeField] DamageSender damageSender;
    [SerializeField] private string weaponId;
    private float rotationSpeed;
    private float maxRotation;

    private float currentRotation = 0.0f;
    private bool rotatingClockwise = true;
    private bool isCooldown=false;

    private FlameThrowerData weaponData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public void SetData(WeaponBaseData _data)
    {
        if (_data is FlameThrowerData _weaponData)
        {
            weaponData = _weaponData;
            rotationSpeed = weaponData.RrotationSpeed;
            maxRotation=weaponData.MaxRotateAngle;
            damageSender.SetData(weaponData.Cooldown, weaponData.DamageAmount, weaponData.TargetTag, false);
        }
    }
    public string GetWeaponId()
    {
        return weaponId;
    }

    public IEnumerator<float> ActiveWeapon()
    {
        gameObject.SetActive(true);
        while (gameObject.activeSelf)
        {
            if (isCooldown)
            {
                damageSender.SetDamageSenderStatus(false);
                yield return Timing.WaitForSeconds(weaponData.Cooldown);
                isCooldown = false; 
            }
            else
            {
                if (!damageSender.CheckDamageSenderStatus())
                    damageSender.SetDamageSenderStatus(true);
                float rotationAmount = rotationSpeed * Time.deltaTime;


                if (rotatingClockwise)
                {
                    currentRotation += rotationAmount;
                    if (currentRotation >= maxRotation)
                    {
                        currentRotation = maxRotation;
                        rotatingClockwise = false;
                    }
                }
                else
                {
                    currentRotation -= rotationAmount;
                    if (currentRotation <= -maxRotation)
                    {
                        isCooldown=true;
                        currentRotation = -maxRotation;
                        rotatingClockwise = true;
                    }
                }

                transform.rotation = Quaternion.Euler(0, 0, currentRotation);
                yield return Timing.WaitForOneFrame;
            }
        }
    }

    public void Fire(Vector2 _target)
    {
    }

    public float GetWeaponCooldown()
    {
        return weaponData.Cooldown;
    }


    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<Vector2> _onGetNearestTarget)
    {
        onShoot = _onShoot;
    }

    public string GetUserId()
    {
        return weaponData.Uid;
    }

    public void DisableWeapon()
    {
        gameObject.SetActive(false);
    }
}
[Serializable]
public class FlameThrowerData : WeaponBaseData
{
    public float BreakTimeBetweenSendDamage;
    public float MaxRotateAngle;
    public float RrotationSpeed;
}
