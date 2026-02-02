using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextChoice : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> m_Texts;
    [SerializeField] private Color m_SelectedColor = Color.yellow;
    [SerializeField] private Toggle m_Toggle;

    private List<Color> _defaultColors = new List<Color>();

    private void Awake()
    {
        foreach (var text in m_Texts)
            _defaultColors.Add(text.color);

        m_Toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
            EnableEffects();
        else
            DisableEffects();
    }

    public void EnableEffects()
    {
        m_Toggle.isOn = true;

        for (int i = 0; i < m_Texts.Count; i++)
        {
            m_Texts[i].color = m_SelectedColor;
            m_Texts[i].fontStyle |= FontStyles.Underline;
        }
    }

    public void DisableEffects()
    {
        m_Toggle.isOn = false;

        for (int i = 0; i < m_Texts.Count; i++)
        {
            m_Texts[i].color = _defaultColors[i];
            m_Texts[i].fontStyle &= ~FontStyles.Underline;
        }
    }
}
