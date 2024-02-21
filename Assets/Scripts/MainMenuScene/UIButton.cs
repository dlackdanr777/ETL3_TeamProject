using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private RectTransform _rectTransform;
    public RectTransform RectTransform  => _rectTransform;

    public void Init(UnityAction onClicked)
    {
        _button.onClick.AddListener(onClicked);
        _rectTransform = GetComponent<RectTransform>();
    }
}
