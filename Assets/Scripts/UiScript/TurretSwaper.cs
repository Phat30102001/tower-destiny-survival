using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSwaper:MonoBehaviour
{
    [SerializeField] private  List<RectTransform> turretRect;
    private RectTransform currentTurretBeingDrag;
    private RectTransform currentTurretBeingSwaped;
    private Action<int, int> onSwapTurret;
    private bool dragable = false;
    private void Start()
    {
        Timing.RunCoroutine(CheckSwapable());
    }
    //public void SetTurretRect(List<RectTransform> _turretRect)
    //{
    //    for(int i = 0; i < _turretRect.Count; i++)
    //    {
    //        turretRect.Add(i, _turretRect[i]);
    //    }
    //    Timing.RunCoroutine(CheckSwapable());
    //}
    public void SetCurrentTurretBeingDrag(RectTransform _currentTurretBeingDrag)
    {
        currentTurretBeingDrag = _currentTurretBeingDrag;
    }
    private IEnumerator<float> CheckSwapable()
    {
        while (gameObject.activeSelf)
        {
            if(currentTurretBeingDrag != null&& dragable)
            {
                SwapTurret();

            }
            yield return Timing.WaitForOneFrame;
        }
    }
    public void SwapTurret()
    {
        foreach (var t in turretRect)
        {
            if (t == currentTurretBeingDrag)
            {
                continue;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                t, Input.mousePosition, Camera.main, out Vector2 _mousePos);

            if (t.rect.Contains(_mousePos))
            {
                currentTurretBeingSwaped = t;
                int dragIndex = turretRect.IndexOf(currentTurretBeingDrag);
                int swapIndex = turretRect.IndexOf(currentTurretBeingSwaped);

                // Trigger the swap event
                Debug.Log($"swap index: {dragIndex} to {swapIndex}");
                onSwapTurret?.Invoke(dragIndex, swapIndex);

                //// Swap the turrets in the list
                turretRect[dragIndex] = currentTurretBeingSwaped;
                turretRect[swapIndex] = currentTurretBeingDrag;


                break;
            }
        }
    }
    public void SetDragable(bool _isDragable)
    {
        dragable = _isDragable;
    }
    public void AssignEvent(Action<int,int> _onSwapTurret)
    {
        onSwapTurret = _onSwapTurret;
    }
}
