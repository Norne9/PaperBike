using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour, IUI {
    public float Score {
        set => distanceText.text = $"Distance:  {value:F1}m";
    }

    [SerializeField] private Image wheelieImage;
    [SerializeField] private Image gameOverImage;
    [SerializeField] private TextMeshProUGUI distanceText;
    
    public void ShowWheelie() {
        var rectTransform = wheelieImage.rectTransform;
        rectTransform.localEulerAngles = Vector3.forward * Random.Range(-20f, 20f);
        rectTransform.localScale = Vector3.zero;
        DOTween.Sequence()
            .Append(wheelieImage.DOColor(Color.white, 0.2f))
            .Insert(0f, rectTransform.DOScale(Vector3.one, 0.2f))
            .Insert(0f, rectTransform.DOLocalRotate(Vector3.forward * Random.Range(-20f, 20f), 0.2f))
            .AppendInterval(1f)
            .Append(wheelieImage.DOColor(new Color(1f, 1f, 1f, 0f), 1f));
    }

    public void ShowGameOver(UnityAction onDone) {
        var rectTransform = gameOverImage.rectTransform; 
        rectTransform.anchoredPosition = new Vector2(0f, 200f);
        DOTween.Sequence()
            .Append(rectTransform.DOAnchorPos(new Vector2(0f, -50f), 1f).SetEase(Ease.OutBounce))
            .AppendInterval(1f)
            .Append(rectTransform.DOAnchorPos(new Vector2(0f, 200f), 1f).SetEase(Ease.InElastic))
            .OnComplete(() => onDone());
    }
}
