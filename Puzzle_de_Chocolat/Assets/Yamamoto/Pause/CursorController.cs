using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;

        //画像をカーソルの位置にセットする
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// カーソルのオンオフを設定する関数
    /// </summary>
    /// <param name="torf"></param> trueなら表示、falseなら非表示
    public void ChangeCursorEnable(bool torf)
    {
        Cursor.visible = torf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
