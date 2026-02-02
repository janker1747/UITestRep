using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GalleryItemView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _premiumBadge;
    [SerializeField] private Button _button;


    private int _index;
    private bool _isPremium;
    private string _url;
    private bool _isLoaded;

    public Image _LoadImage => _image;

    public void Init(int index, string url, bool isPremium, System.Action<bool, Sprite> onClick)
    {
        _index = index;
        _url = url;
        _isPremium = isPremium;

        _premiumBadge.SetActive(isPremium);
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onClick?.Invoke(_isPremium, _image.sprite));
    }

    public void LoadIfNeeded()
    {
        if (_isLoaded) return;
        StartCoroutine(LoadImage());
    }

    public void UnloadIfNeeded()
    {
        if (!_isLoaded)
            return;

        _image.sprite = null;
        _isLoaded = false;
    }

    private IEnumerator LoadImage()
    {
        _isLoaded = true;

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                yield break;

            Texture2D tex = DownloadHandlerTexture.GetContent(request);
            _image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
        }
    }
}
