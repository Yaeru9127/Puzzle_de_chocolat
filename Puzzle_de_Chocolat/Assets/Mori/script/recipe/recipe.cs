using UnityEngine;

public class Recipe : MonoBehaviour
{
    [SerializeField] private KeyCode triggerKey = KeyCode.Escape;
    [SerializeField] private float scaleSpeed = 1f;
    [SerializeField] private float maxScaleX = 3f;
    [SerializeField] private float maxScaleY = 3f;

    private bool scaling = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
            scaling = true;

        if (!scaling) return;

        //左下固定処理
        Vector3 prevLeftBottom = GetLeftBottomWorldPos();

        float newScaleX = Mathf.Min(transform.localScale.x + scaleSpeed * Time.deltaTime, maxScaleX);
        float newScaleY = Mathf.Min(transform.localScale.y + scaleSpeed * Time.deltaTime, maxScaleY);

        transform.localScale = new Vector3(newScaleX, newScaleY, transform.localScale.z);

        Vector3 newLeftBottom = GetLeftBottomWorldPos();
        transform.position += prevLeftBottom - newLeftBottom;

        if (IsOutOfScreen())
        {
            transform.localScale = new Vector3(newScaleX - scaleSpeed * Time.deltaTime, newScaleY - scaleSpeed * Time.deltaTime, transform.localScale.z);
            transform.position -= prevLeftBottom - newLeftBottom;
            scaling = false;
            Debug.Log("画面外のため拡大停止");
        }
        else if (Mathf.Approximately(newScaleX, maxScaleX) && Mathf.Approximately(newScaleY, maxScaleY))
        {
            scaling = false;
        }
    }

    Vector3 GetLeftBottomWorldPos()
    {
        var rend = GetComponent<Renderer>();
        if (rend != null)
            return new Vector3(rend.bounds.min.x, rend.bounds.min.y, transform.position.z);
        else
            return transform.position;
    }

    bool IsOutOfScreen()
    {
        var rend = GetComponent<Renderer>();
        if (rend == null) return false;

        Vector3 leftBottomScreen = mainCamera.WorldToScreenPoint(new Vector3(rend.bounds.min.x, rend.bounds.min.y, transform.position.z));
        Vector3 rightTopScreen = mainCamera.WorldToScreenPoint(new Vector3(rend.bounds.max.x, rend.bounds.max.y, transform.position.z));

        return leftBottomScreen.x < 0 || leftBottomScreen.y < 0 || rightTopScreen.x > Screen.width || rightTopScreen.y > Screen.height;
    }
}
