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
        //�e�X�g
        ChangeCursorEnable(true);

        //�摜���J�[�\���̈ʒu�ɃZ�b�g����
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// �J�[�\���̃I���I�t��ݒ肷��֐�
    /// </summary>
    /// <param name="torf"></param> true�Ȃ�\���Afalse�Ȃ��\��
    public void ChangeCursorEnable(bool torf)
    {
        Cursor.visible = torf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
