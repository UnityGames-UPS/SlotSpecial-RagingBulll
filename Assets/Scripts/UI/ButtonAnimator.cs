using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Transform buttonTransform;
    private Button button;
    private Image buttonImage;
    private Vector3 originalScale;
    private Tween hoverTween;

    private void Awake()
    {
        buttonTransform = transform;
        originalScale = buttonTransform.localScale;
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            PressedAnimation();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            OnClickedAnimation();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            // Kill any existing hover tween to prevent conflicts
            hoverTween?.Kill();

            // Scale up the button image to 1.2 over 0.2 seconds
            hoverTween = buttonImage.rectTransform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutQuad);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            // Kill any existing hover tween to prevent conflicts
            hoverTween?.Kill();

            // Scale back the button image to original scale over 0.2 seconds
            hoverTween = buttonImage.rectTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
        }
    }

    private void PressedAnimation()
    {
        buttonTransform.DOScale(originalScale * 0.8f, 0.2f).SetEase(Ease.OutQuad);
    }

    private void OnClickedAnimation()
    {
        buttonTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
    }
}
