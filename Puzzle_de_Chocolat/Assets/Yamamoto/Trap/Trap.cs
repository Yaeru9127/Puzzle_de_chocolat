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
        
    }

    /// <summary>
    /// 以下、トラップを踏んだ時の処理
    /// </summary>
    
    //生クリーム
    public void CaseFrischeSahne()
    {
        Debug.Log("in CaseFrischeSahne()");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
