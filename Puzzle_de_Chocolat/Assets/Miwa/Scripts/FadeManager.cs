using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class FadeManager : MonoBehaviour
{
    [Header("�t�F�[�h�p�l��")] public Image FadePanel;
    [Header("�t�F�[�h���x")] public float FadeTaime = 1.0f;//Fade�ɂ����鎞�Ԃ̐ݒ�
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
        isFading = true; //fade���J�n
        FadePanel.DOFade(endValue: 1f, duration: 0f).OnComplete(() =>
        {
            
            isFading = false;  //fade���I��

            FadePanel.gameObject.SetActive(false);
        });

    }
    //fadoout���ăV�[�������[�h����
    public void StartFadeoutAndLoadScene(string game)
    {
        if (isFading) return;
        isFading = true; //fade���J�n
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
