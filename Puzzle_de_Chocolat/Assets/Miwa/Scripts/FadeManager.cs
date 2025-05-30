using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class FadeManager : MonoBehaviour
{
    [Header("フェードパネル")] public Image FadePanel;
    [Header("フェード速度")] public float FadeTaime = 1.0f;//Fadeにかける時間の設定
    private bool isFading = false;
     
    public static FadeManager instance { get; private set; }
    
    void Awake()
    {
        if (instance ==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (FadePanel !=null)
        {
            Color panelColor = FadePanel.color;
            FadePanel.alphaHitTestMinimumThreshold = 1f;
            FadePanel.color = panelColor;

            FadePanel.gameObject.SetActive(true);
            StaretFadein();
        }
    }
    
    public void StaretFadein()
    {
        if (isFading) return;
        isFading = true; //fadeが開始
        FadePanel.DOFade(endValue: 1f, duration: 0f).OnComplete(() =>
        {
            
            isFading = false;  //fadeが終了

            FadePanel.gameObject.SetActive(false);
        });

    }
    //fadooutしてシーンをロードする
    public void StartFadeoutAndLoadScene(string game)
    {
        if (isFading) return;
        isFading = true; //fadeが開始
        FadePanel.DOFade(endValue: 0f, duration: 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("game");
            isFading = false;
        });

    }

    public bool IsFading()
    {
        return isFading;
    }
}
