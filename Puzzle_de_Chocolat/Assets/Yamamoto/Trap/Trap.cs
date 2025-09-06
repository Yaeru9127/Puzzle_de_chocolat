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
        //Rayで自身のマスを探す
        Vector3 origin = this.gameObject.transform.position;
        origin.z -= 1;
        Ray ray = new(origin, Vector3.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        //ヒットした中からマスを探す
        foreach (RaycastHit hit in hits )
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            if (tile == null) continue; //マススクリプトをもってなかったら次へ

            //マスを見つけたら
            if (tile != null)
            {
                //自身の位置を調整
                Vector3 pos = tile.transform.position;
                pos.z = -5;
                this.gameObject.transform.position = pos;
                Debug.Log(this.gameObject.transform.position);
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
