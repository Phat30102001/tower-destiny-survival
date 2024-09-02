using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    public Vector2 CurrentTouchPosition { get; private set; }
    private bool isTouching = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                isTouching = true;
                CurrentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                CurrentTouchPosition = new Vector2(CurrentTouchPosition.x, CurrentTouchPosition.y);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }
#if UNITY_EDITOR
        else
        {
            // Handle mouse input
            if (Input.GetMouseButton(0))
            {
                Debug.Log($"touching:{Input.mousePosition}");
                isTouching = true;
                CurrentTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CurrentTouchPosition = new Vector2(CurrentTouchPosition.x, CurrentTouchPosition.y);
            }
            else
            {
                isTouching = false;
            }
        }
#endif
    }


    public bool IsTouching()
    {
        return isTouching;
    }
}