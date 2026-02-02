using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Baner : MonoBehaviour
{
    [SerializeField] private Image _dot;

    private const float ActiveScale = 1f;
    private const float InactiveScale = 0f;

    private static readonly Color ActiveColor = Color.white;
    private static readonly Color InactiveColor = new Color(1f, 1f, 1f, 0.5f);

    public void SetActive(bool active)
    {
        if (_dot == null) return;

        _dot.DOKill();
        _dot.transform.DOKill();

        if (active)
        {
            _dot.DOColor(ActiveColor, 0.2f);
            _dot.transform.DOScale(ActiveScale, 0.25f).SetEase(Ease.OutBack);
        }
        else
        {
            _dot.DOColor(InactiveColor, 0.2f);
            _dot.transform.DOScale(InactiveScale, 0.2f);
        }
    }
}
