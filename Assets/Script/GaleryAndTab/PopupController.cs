using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupController : MonoBehaviour
{
    [SerializeField] private Image _imagePopup;
    [SerializeField] private Image _premiumPopup;
    [SerializeField] private Image _popupImage;

    [Header("Anim")]
    [SerializeField] private float _fadeDuration = 0.18f;
    [SerializeField] private float _scaleDuration = 0.25f;
    [SerializeField] private Ease _openEase = Ease.OutBack;
    [SerializeField] private Ease _closeEase = Ease.InBack;

    private Tween _currentTween;

    public void ShowImage(Sprite sprite)
    {
        _popupImage.sprite = sprite;
        Show(_imagePopup);
    }

    public void ShowPremium()
    {
        Show(_premiumPopup);
    }

    public void CloseAll()
    {
        Hide(_imagePopup);
        Hide(_premiumPopup);
    }

    private void Show(Image popup)
    {
        KillTween();

        popup.gameObject.SetActive(true);
        popup.transform.localScale = Vector3.one * 0.85f;

        _currentTween = DOTween.Sequence()
            .Join(popup.transform.DOScale(1f, _scaleDuration).SetEase(_openEase));
    }

    private void Hide(Image popup)
    {
        if (!popup.gameObject.activeSelf)
            return;

        KillTween();

        _currentTween = DOTween.Sequence()
            .Append(popup.DOFade(0f, _fadeDuration))
            .Join(popup.transform.DOScale(0.85f, _scaleDuration).SetEase(_closeEase))
            .OnComplete(() => popup.gameObject.SetActive(false));
    }

    private void KillTween()
    {
        if (_currentTween != null && _currentTween.IsActive())
            _currentTween.Kill();
    }
}
