using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour
{
    [Header("UI関連")]
    [SerializeField] private Image gaugeFillImage;

    [Tooltip("ゲージ残り回数を数字スプライトで表示するUI Image")]
    [SerializeField] private Image gaugeNumberDisplay;

    [SerializeField] private GameOverController gameOverController;
    [SerializeField] private Remainingaircraft remainingAircraft;

    [Header("ゲージ設定")]
    [SerializeField] private float increaseDuration = 0.5f;
    [SerializeField] private int defaultIncreaseAmount = 1;
    [SerializeField] private int maxValue = 10;

    private int currentValue = 0;
    private Queue<int> increaseQueue = new Queue<int>();
    private bool isIncreasing = false;

    void Start()
    {
        UpdateGaugeNumberDisplay();
    }

    public void OnObjectDestroyed()
    {
        OnObjectDestroyed(defaultIncreaseAmount);
    }

    public void OnObjectDestroyed(int increaseAmount)
    {
        increaseQueue.Enqueue(increaseAmount);
        if (!isIncreasing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isIncreasing = true;

        while (increaseQueue.Count > 0)
        {
            int amount = increaseQueue.Dequeue();
            yield return StartCoroutine(IncreaseGaugeSmoothly(amount));
        }

        isIncreasing = false;
    }

    private IEnumerator IncreaseGaugeSmoothly(int amount)
    {
        if (gaugeFillImage == null)
            yield break;

        int oldValue = currentValue;
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);

        float startFill = (float)oldValue / maxValue;
        float endFill = (float)currentValue / maxValue;

        float elapsed = 0f;
        while (elapsed < increaseDuration)
        {
            elapsed += Time.deltaTime;
            gaugeFillImage.fillAmount = Mathf.Lerp(startFill, endFill, elapsed / increaseDuration);
            yield return null;
        }

        gaugeFillImage.fillAmount = endFill;

        UpdateGaugeNumberDisplay();

        if (remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        if (currentValue >= maxValue && gameOverController != null)
        {
            gameOverController.ShowGameOver();
        }
    }

    private void UpdateGaugeNumberDisplay()
    {
        if (gaugeNumberDisplay == null || remainingAircraft == null)
            return;

        int remaining = Mathf.Clamp(maxValue - currentValue, 0, 99);
        Sprite sprite = remainingAircraft.GetNumberSprite(remaining);

        if (sprite != null)
        {
            gaugeNumberDisplay.sprite = sprite;
            gaugeNumberDisplay.gameObject.SetActive(true);
        }
        else
        {
            gaugeNumberDisplay.gameObject.SetActive(false);
        }
    }
}