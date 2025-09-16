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

    public int stagenum;        //ステージナンバー
    public string gamescene;    //最後にプレイしたゲームシーン

    // ステージごとのゲージ最大値設定
    // stage0の時最初0
    public int[] stageGaugeMaxValues = { 6, 8, 10 }; // 例：ステージ事で変えれるように

    private void Awake()
    {
        //初期化
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
        //シーンを跨ぐときにメモリから消す
        if (stage == this) stage = null;
    }

    // Update is called once per frame
    void Update()
    {
        //非常時にカーソルの表示 or 非表示させる
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (Cursor.visible) Cursor.visible = false;
            else if (!Cursor.visible) Cursor.visible = true;
        }
    }
}
