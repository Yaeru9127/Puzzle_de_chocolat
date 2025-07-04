using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToDestroy : MonoBehaviour
{
    public static GameObject SelectedObject { get; private set; }

    public GaugeController gaugeController;
    public Transform playerTransform;

    private GameInputActions inputActions;

    private void Awake()
    {
        inputActions = new GameInputActions();
    }

    //private void OnEnable()
    //{
    //    inputActions.Gameplay.Enable();
    //    inputActions.Gameplay.DestroyCandy.performed += OnDestroyCandyPerformed;
    //}

    //private void OnDisable()
    //{
    //    inputActions.Gameplay.DestroyCandy.performed -= OnDestroyCandyPerformed;
    //    inputActions.Gameplay.Disable();
    //}

    private void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            HandleMouseClick();
        }
    }

    //private void OnDestroyCandyPerformed(InputAction.CallbackContext context)
    //{
    //    TryDestroyCandyInFront();
    //}

    void HandleMouseClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (clickedObject.CompareTag("Button"))
                return;

            CandyDrag drag = clickedObject.GetComponent<CandyDrag>();
            if (drag != null && drag.IsDragged)
                return;

            // ★ Candy以外は無視
            if (!clickedObject.CompareTag("Candy"))
                return;

            // オブジェクトを選択
            ClearSelection();
            SelectedObject = clickedObject;

            if (gaugeController != null)
            {
                gaugeController.OnObjectDestroyed();
            }

            Destroy(clickedObject);
        }
    }


    //void TryDestroyCandyInFront()
    //{
    //    if (playerTransform == null) return;

    //    Vector2 origin = playerTransform.position;
    //    Vector2 direction = playerTransform.localScale.x >= 0 ? Vector2.right : Vector2.left;

    //    Debug.DrawRay(origin, direction, Color.red, 5f);

    //    RaycastHit2D hit = Physics2D.Raycast(origin, direction, 5f);

    //    if (hit.collider != null)
    //    {
    //        GameObject target = hit.collider.gameObject;

    //        if (!target.CompareTag("Candy"))
    //            return;

    //        CandyDrag drag = target.GetComponent<CandyDrag>();
    //        if (drag != null && drag.IsDragged)
    //            return;

    //        if (gaugeController != null)
    //        {
    //            gaugeController.OnObjectDestroyed();
    //        }

    //        Destroy(target);
    //    }
    //}

    public static void ClearSelection()
    {
        // 色変更は削除。選択解除のみ。
        SelectedObject = null;
    }
}
