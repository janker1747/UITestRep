using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private RectTransform _content;

    [Header("Prefabs")]
    [SerializeField] private GalleryItemView _itemPrefab;

    [Header("Popups")]
    [SerializeField] private PopupController _popupController;

    [Header("Settings")]
    [SerializeField] private int _totalImages = 66;
    [SerializeField] private string _baseUrl = "http://data.ikppbb.com/test-task-unity-data/pics/";

    private readonly List<GalleryItemView> _items = new();
    private GalleryItemView view;
    private GalleryTab _currentTab = GalleryTab.All;

    private const float LoadPadding = 20f;

    private const float MinCellSize = 460f;
    private const int MinColumns = 2;
    private const int MaxColumns = 3;

    private void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(OnScroll);
        StartCoroutine(SetupAndWaitForLayout());
    }

    private IEnumerator SetupAndWaitForLayout()
    {
        yield return new WaitForEndOfFrame();

        ApplyGridResponsive();
        Build();
    }

    private void OnDisable()
    {
        _scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    public void SelectAll() => SetTab(GalleryTab.All);
    public void SelectOdd() => SetTab(GalleryTab.Odd);
    public void SelectEven() => SetTab(GalleryTab.Even);

    private void SetTab(GalleryTab tab)
    {
        if (_currentTab == tab) return;
        _currentTab = tab;
        Build();
    }

    private void Build()
    {
        foreach (var item in _items)
            Destroy(item.gameObject);

        _items.Clear();

        for (int i = 1; i <= _totalImages; i++)
        {
            if (!PassesFilter(i))
                continue;

            bool isPremium = i % 4 == 0;
            string url = _baseUrl + i + ".jpg";

            view = Instantiate(_itemPrefab, _content);
            view.Init(i, url, isPremium, OnItemClicked);
            _items.Add(view);
        }

        Canvas.ForceUpdateCanvases();
        OnScroll(Vector2.zero);
    }

    private bool PassesFilter(int index)
    {
        return _currentTab switch
        {
            GalleryTab.All => true,
            GalleryTab.Odd => index % 2 == 1,
            GalleryTab.Even => index % 2 == 0,
            _ => true
        };
    }

    private void OnScroll(Vector2 _)
    {
        float viewportTop = _scrollRect.content.anchoredPosition.y;
        float viewportBottom = viewportTop + _scrollRect.viewport.rect.height;

        foreach (var item in _items)
        {
            RectTransform rt = (RectTransform)item.transform;
            float itemTop = -rt.anchoredPosition.y;
            float itemBottom = itemTop - rt.rect.height;

            bool visible =
                itemBottom < viewportBottom + LoadPadding &&
                itemTop > viewportTop - LoadPadding;

            if (visible)
                item.LoadIfNeeded();
            else
                item.UnloadIfNeeded();
        }
    }

    private void OnItemClicked(bool isPremium, Sprite sprite)
    {
        if (isPremium)
            _popupController.ShowPremium();
        else
            _popupController.ShowImage(sprite);
    }


    private void ApplyGridResponsive()
    {
        Canvas.ForceUpdateCanvases();

        float screenWidth = Screen.width;

        int columns = screenWidth < 1440f ? 2 : 3;

        Vector2 spacing = _grid.spacing;

        if (columns == 3)
        {
            spacing.x = 15;
        }
        else
        {
            spacing.x = 150;

        }

        _grid.spacing = spacing;
        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = columns;
    }
}
