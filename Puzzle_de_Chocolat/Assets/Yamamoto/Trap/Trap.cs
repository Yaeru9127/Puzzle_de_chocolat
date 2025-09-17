using UnityEngine;

public class Trap : MonoBehaviour
{
    //トラップの種類
    //今後、増減する可能性があるのでenumで設定
    public enum Type
    {
        FrischeSahne,       //生クリーム
        Test
    }
    public Type type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPosition();
    }

    /// <summary>
    /// 初期位置を設定する関数
    /// </summary>
    private void SetPosition()
    {
        //Rayで自身がいるマスを探す
        Vector3 origin = this.gameObject.transform.position;
        origin.z -= 1;
        Collider2D[] col = Physics2D.OverlapPointAll(origin);

        //ヒットした中からマスを探す
        foreach (Collider2D hit in col)
        {
            Tile tile = hit.gameObject.GetComponent<Tile>();
            if (tile == null) continue; //マススクリプトをもってなかったら次へ

            //マスを見つけたら
            if (tile != null)
            {
                //位置の調整
                Vector3 pos = tile.transform.position;
                pos.z = -3;
                this.gameObject.transform.position = pos;

                //デバッグ
                //Debug.Log($"mass : {tile.gameObject.transform.position}");
                //Debug.Log($"trap : {this.gameObject.transform.position}");
                break;
            }
        }
    }

    /// <summary>
    /// 以下、トラップを踏んだ時の処理
    /// </summary>

    //生クリーム
    public void CaseFrischeSahne()
    {
        Debug.Log("in CaseFrischeSahne()");
    }
}
