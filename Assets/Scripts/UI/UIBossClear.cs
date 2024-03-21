using Muks.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBossClear : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private BossController _boss;

    [Space]
    [Header("Animation Option")]
    [SerializeField] private RectTransform _target;
    [SerializeField] private CanvasGroup _canvasGroup;


    private Vector2 _tmpPos;

    private void Awake()
    {
        _tmpPos = _target.anchoredPosition;
        _mainMenuButton.onClick.AddListener(OnButtonClicked);
        gameObject.SetActive(false);

        _boss.OnHpMin += Show;
    }


    public void Show()
    {
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

        Tween.TransformMove(gameObject, transform.position, 5f, TweenMode.Constant, () =>
        {
            GameManager.Instance.UnLockCursor();

            Tween.CanvasGroupAlpha(_canvasGroup.gameObject, 1, 0.3f, TweenMode.Constant, () =>
            {
                _canvasGroup.blocksRaycasts = true;
            });

            Vector2 startPos = _tmpPos + new Vector2(0, -50);
            _target.anchoredPosition = startPos;
            Tween.RectTransfromAnchoredPosition(_target.gameObject, _tmpPos, 0.3f);

            _boss.OnHpMin -= Show;
        });     
    }


    private void OnButtonClicked()
    {
        LoadingSceneManager.LoadScene("Title Screen");
    }

}
