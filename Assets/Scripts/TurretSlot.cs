using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    [SerializeField] private RectTransform slotTransform;
    [SerializeField] private Turret turret;
    private bool isOccupied=false;
    private Action onTurretDestroy;
    

    public bool CheckIsOccupied() => isOccupied;
    public Transform GetSlotTransform()=> slotTransform;

    public void SetDataForTurret(TurretData _data)
    {
        if (turret == null) return;
        turret.SetData(_data);
        turret.AssignEvent(()=> 
        {
            isOccupied=false ;
            onTurretDestroy?.Invoke();
            gameObject.SetActive(false);
        });
        gameObject.SetActive(true);
        isOccupied=true;
    }
    public void AssignEvent(Action _onTurretDestroy)
    {
        onTurretDestroy= _onTurretDestroy;
    }
}
