using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiTrashBin : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            Turret turret = droppedObject.GetComponent<Turret>();
            if (turret != null)
            {
                turret.OnDestroyTurret();
            }
        }
    }
}
