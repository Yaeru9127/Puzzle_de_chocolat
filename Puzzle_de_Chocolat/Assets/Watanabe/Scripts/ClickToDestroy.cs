using UnityEngine;

public class ClickToDestroy : MonoBehaviour
{
    public static GameObject SelectedObject { get; private set; }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                // タグが "Button" のオブジェクトは無視
                if (clickedObject.CompareTag("Button"))
                    return;

                CandyDrag drag = clickedObject.GetComponent<CandyDrag>();
                if (drag != null && drag.IsDragged)
                    return;

                // 選択を切り替え
                SetSelectedVisual(SelectedObject, false); // 旧
                SelectedObject = clickedObject;
                SetSelectedVisual(SelectedObject, true);  // 新
            }
        }
    }

    void SetSelectedVisual(GameObject obj, bool isSelected)
    {
        if (obj == null) return;

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = isSelected ? Color.yellow : Color.white;
        }
    }

    public static void ClearSelection()
    {
        if (SelectedObject != null)
        {
            SpriteRenderer renderer = SelectedObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
                renderer.color = Color.white;

            SelectedObject = null;
        }
    }
}
