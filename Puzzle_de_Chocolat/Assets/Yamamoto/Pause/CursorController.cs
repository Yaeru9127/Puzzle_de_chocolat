using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController cc { get; private set; }
    [SerializeField] private Texture2D cursorTexture;

    private void Awake()
    {
        if (cc == null) cc = this;
        else if (cc != null) Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //テスト
        ChangeCursorEnable(true);

        //画像をカーソルの位置にセットする
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        DontDestroyOnLoad(this.gameObject);
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
