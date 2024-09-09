using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChainSaw : MonoBehaviour, IWeapon
{
    [SerializeField] DamageSender damageSender;
    [SerializeField] private string weaponId;

    private ChainSawData weaponData;
    private ChainSawSkillData skillData;
    private Action<string, Vector2, Vector2, int, float, float, ProjectileData> onShoot;
    public void SetData(WeaponBaseData _data)
    {
        if (_data is ChainSawData _weaponData)
        {
            weaponData = _weaponData;
            skillData= _weaponData.SkillData;
            damageSender.SetData(weaponData.BreakTimeBetweenSendDamage, weaponData.DamageAmount, weaponData.TargetTag, false);
        }
    }

    public string GetWeaponId()
    {
        return weaponId;
    }

    public IEnumerator<float> ActiveWeapon()
    {
        gameObject.SetActive(true);
        yield break;
    }

    public void Fire(Vector2 _target)
    {
    }

    public float GetWeaponCooldown()
    {
        return weaponData.Cooldown;
    }


    public void AssignEvent(Action<string, Vector2, Vector2, int, float, float, ProjectileData> _onShoot, Func<bool, Vector2> _onGetNearestTarget)
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

    public void TriggerWeaponSkill()
    {
        Vector2 _cacheTrasformPos= transform.position;
        Vector2 _target = new Vector2(_cacheTrasformPos.x+1, _cacheTrasformPos.y);
        onShoot?.Invoke(skillData.ProjectileId, _cacheTrasformPos,
               _target, skillData.NumberPerRound, skillData.Cooldown, 0, new ProjectileData
                {
                    Damage = skillData.DamageAmount,
                    ShootSpeed = skillData.ShootSpeed,
                    TargetTag = weaponData.TargetTag,
                    HideOnHit = true,
                });
    }
}
[Serializable]
public class ChainSawData : WeaponBaseData
{
    public float BreakTimeBetweenSendDamage;
    public ChainSawSkillData SkillData;
}

[Serializable]
public class ChainSawSkillData : WeaponSkillData
{
    public int DamageAmount;
    public float Cooldown;
    public float ShootSpeed;
    public string ProjectileId;
    public int NumberPerRound;
}


