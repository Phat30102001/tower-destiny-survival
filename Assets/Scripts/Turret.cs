using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turret: MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private TurretData turretData;
    [SerializeField] DamageReceiver damageReceiver;
    [SerializeField] Transform weaponContainer;
    [SerializeField] private RectTransform rectTransform;
    private int healthPoint;
    private Action onZeroHealthCallback;
    private Action onDestroyTurret;
    Action<string> onDisableWeapon;
    Action<string> onDeleteTurretData;
    private string weaponId="";
    private int weaponLevel;
    private Canvas canvas;
    private bool isDraggable = false;
    [SerializeField] private CanvasGroup canvasGroup;

    public void SetData(TurretData _data, Canvas _canvas)
    {
        turretData = _data;
        canvas = _canvas;
        healthPoint =turretData.HealthPoint;
        damageReceiver.AssignEvent(onReceiveDamage);
    }
    public void SetTurretWeapon(string _weaponId, int _level)
    {
        weaponId = _weaponId;
        weaponLevel = _level;
    }
    private void onReceiveDamage(int _amount)
    {
        healthPoint -= _amount;
        //Debug.Log($"{gameObject.name}'s health: {healthPoint}");
        if (healthPoint <= 0)
        {
            onZeroHealthCallback?.Invoke();
            onDisableWeapon?.Invoke(turretData.TurretId);
        }
    }
    public void AssignEvent(Action _onZeroHealthCallback,Action _onDestroyTurret
        , Action<string> _onDisableWeapon,Action<string> _onDeleteTurretData)
    {
        onDisableWeapon = _onDisableWeapon;
        onZeroHealthCallback = _onZeroHealthCallback;
        onDestroyTurret = _onDestroyTurret;
    }
    public Transform GetWeaponCointainer()
    {
        return weaponContainer;
    }
    public string CurrentEquipWeaponId()
    {
        return weaponId;
    }
    public int CurrentEquipWeaponLevel()
    {
        return weaponLevel;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDraggable)return;
        // Update the object's position as the mouse is dragged
        if (canvas != null)
        {
            // Convert the mouse position to local position in the canvas
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out position
            );
            rectTransform.localPosition = position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha= 1;
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = Vector3.zero;
    }
    public void SetDragable(bool _isDraggable)
    {
        isDraggable = _isDraggable;
    }
    public void OnDestroyTurret()
    {
        onDestroyTurret?.Invoke();
        onDisableWeapon?.Invoke(turretData.TurretId);
        onDeleteTurretData?.Invoke(turretData.TurretId);
        weaponId = "";
        weaponLevel = 0;
    }
}
[Serializable]
public struct TurretData
{
    public string TurretId;
    public int Level;
    public int HealthPoint;
    public string WeaponId;
    public int WeaponLevel;
    public ResourceData priceData;
}