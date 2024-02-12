using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PercentCounter : MonoBehaviour
{
    [SerializeField] private Image border;
    [SerializeField] private Image percentContainer;
    [SerializeField] private Image percentFill;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private TextMeshProUGUI shadowPercentText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelTextFill;
    [SerializeField] private Slider percentSlider;
    [SerializeField] private RectTransform shadowFill;
    [SerializeField] private Image fullPercentImage;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private float stepIncrement = 0.01f;

    private Coroutine animateRoutine;

    public void SetColor(Color color)
    {
        percentContainer.color = color;
        nextLevelText.color = color;
        border.color = color;
        percentFill.color = color;
        fullPercentImage.color = color;
    }

    public void SetLevel(int level)
    {
        currentLevelText.text = level.ToString("N0");
        nextLevelText.text = (level + 1).ToString("N0");
        nextLevelTextFill.text = nextLevelText.text;
    }

    public void SetValueSmooth(float value)
    {
        if (animateRoutine != null)
            StopCoroutine(animateRoutine);
        animateRoutine = StartCoroutine(AnimateValue(value));
    }

    public void SetValue(float value)
    {
        //value = (float)System.Math.Truncate(value * 100) / 100;
        // fast patch to avoid reaching 100% prematurely as Unity doesn't likes doubles.
        if (value < 1 && value > .99f) {
            value = .99f;
        }
        percentSlider.normalizedValue = value;
        percentText.text = value.ToString("P0");
    }

    public void SetShadowValue(float value)
    {
        if (value < 1 && value > .99f) {
            value = .99f;
        }
        shadowFill.anchorMax = new Vector2(value, 1);
        shadowPercentText.transform.parent.gameObject.SetActive(value > 0);
        shadowPercentText.text = value.ToString("P0");
    }

    private IEnumerator AnimateValue(float endValue)
    {
        float p = 0;
        float e = 0;
        var startValue = percentSlider.normalizedValue;
        var value = percentSlider.normalizedValue;
        while (p < 1) {
            p = e / (smoothTime * Time.timeScale);
            value = Mathf.Clamp01(Mathf.Lerp(startValue, endValue, p));
            SetValue(value);
            yield return null;
            e += Time.deltaTime;
        }
        if (value >= 1) {
            fullPercentImage.gameObject.SetActive(true);
        } else if (fullPercentImage.gameObject.activeSelf) {
            fullPercentImage.gameObject.SetActive(false);
        }

        animateRoutine = null;
    }
}
