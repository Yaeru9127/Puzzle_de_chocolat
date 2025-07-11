using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager stage { get; private set; }

    public int stagenum;    //ステージナンバー

    private void Awake()
    {
        //初期化
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
        //シーンを跨ぐときにメモリから消す
        if (stage == this) stage = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
