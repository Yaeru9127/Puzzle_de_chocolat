using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager stage { get; private set; }

    public enum Phase
    {
        Title,
        Game,
        Result
    }
    public Phase phase;

    public int stagenum;        //�X�e�[�W�i���o�[
    public string gamescene;    //�Ō�Ƀv���C�����Q�[���V�[��

    // �X�e�[�W���Ƃ̃Q�[�W�ő�l�ݒ�
    // stage0�̎��ŏ�0
    public int[] stageGaugeMaxValues = { 6, 8, 10 }; // ��F�X�e�[�W���ŕς����悤��

    private void Awake()
    {
        //������
        if (stage == null) stage = this;
        else if (stage != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        phase = Phase.Title;
        DontDestroyOnLoad(this.gameObject);
        Cursor.visible = false;
    }

    public void SetStageNum(int num)
    {
        stagenum = num;
    }

    private void OnDestroy()
    {
        //�V�[�����ׂ��Ƃ��Ƀ������������
        if (stage == this) stage = null;
    }

    // Update is called once per frame
    void Update()
    {
        //��펞�ɃJ�[�\���̕\�� or ��\��������
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (Cursor.visible) Cursor.visible = false;
            else if (!Cursor.visible) Cursor.visible = true;
        }
    }
}
