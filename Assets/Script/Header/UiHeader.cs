using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiHeader : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private ScrollRect _carousel;
    [SerializeField] private RectTransform _content;
    [SerializeField] private List<Baner> _baners;

    [Header("Settings")]
    [SerializeField] private float _autoScrollInterval = 5f;
    [SerializeField] private float _scrollDuration = 0.35f;

    private int _currentIndex;
    private Coroutine _autoScrollRoutine;
    private bool _isAnimating;

    private void OnEnable()
    {
        _carousel.onValueChanged.AddListener(OnScroll);
        SnapTo(0, instant: true);
        ScrollToNext();
        StartAutoScroll();
    }

    private void OnDisable()
    {
        _carousel.onValueChanged.RemoveListener(OnScroll);
        StopAutoScroll();
    }

    private void StartAutoScroll()
    {
        StopAutoScroll();
        _autoScrollRoutine = StartCoroutine(AutoScrollRoutine());
    }

    private void StopAutoScroll()
    {
        if (_autoScrollRoutine != null)
            StopCoroutine(_autoScrollRoutine);
    }

    private IEnumerator AutoScrollRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_autoScrollInterval);

        while (true)
        {
            yield return wait;
            ScrollToNext();
        }
    }

    private void ScrollToNext()
    {
        int next = (_currentIndex + 1) % _baners.Count;
        SnapTo(next, instant: false);
    }

    private void SnapTo(int index, bool instant)
    {
        _currentIndex = index;
        UpdateDots();

        float target = IndexToNormalized(index);

        _carousel.DOKill();

        if (instant)
        {
            _carousel.horizontalNormalizedPosition = target;
        }
        else
        {
            _isAnimating = true;
            DOTween.To(
                    () => _carousel.horizontalNormalizedPosition,
                    x => _carousel.horizontalNormalizedPosition = x,
                    target,
                    _scrollDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _isAnimating = false);
        }
    }

    private void OnScroll(Vector2 _)
    {
        if (_isAnimating)
            return;

        int nearest = GetNearestIndex();

        if (nearest == _currentIndex)
            return;

        _currentIndex = nearest;
        UpdateDots();
        StartAutoScroll(); 
    }

    private int GetNearestIndex()
    {
        float pos = _carousel.horizontalNormalizedPosition;
        return Mathf.Clamp(Mathf.RoundToInt(pos * (_baners.Count - 1)), 0, _baners.Count - 1);
    }

    private float IndexToNormalized(int index)
    {
        if (_baners.Count <= 1)
            return 0f;

        return (float)index / (_baners.Count - 1);
    }

    private void UpdateDots()
    {
        for (int i = 0; i < _baners.Count; i++)
            _baners[i].SetActive(i == _currentIndex);
    }
}
