using UnityEngine;

public class CursorController1 : MonoBehaviour
{
    //public static CursorController cc;

    [SerializeField] private GameObject cursorObject; // カーソル表示を制御するオブジェクト（任意）

    private void Awake()
    {
        //cc = this;
    }

    /// <summary>
    /// カーソルを有効/無効にする
    /// </summary>
    public void ChangeCursorEnable(bool enable)
    {
        if (cursorObject != null)
        {
            cursorObject.SetActive(enable);
        }
        else
        {
            Debug.LogWarning("cursorObject が未設定です。インスペクターで設定してください。");
        }
    }
}
