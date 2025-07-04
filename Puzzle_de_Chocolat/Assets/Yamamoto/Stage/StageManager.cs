using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager stage { get; private set; }

    public int stagenum;    //�X�e�[�W�i���o�[

    private void Awake()
    {
        //������
        if (stage == null) stage = this;
        else if (stage != null) Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
