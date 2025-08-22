using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager stage { get; private set; }

    private CursorController cc;

    public enum Phase
    {
        Title,
        Game,
        Result
    }
    public Phase phase;

    public int stagenum;    //�X�e�[�W�i���o�[

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
        cc = CursorController.cc;
        phase = Phase.Title;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetStageNum(int i)
    {
        stagenum = i;
    }

    private void OnDestroy()
    {
        //�V�[�����ׂ��Ƃ��Ƀ������������
        if (stage == this) stage = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
