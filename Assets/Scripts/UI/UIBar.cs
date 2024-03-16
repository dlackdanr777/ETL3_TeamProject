using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    private enum BarType
    {
        Decrease,
        Increase
    }

    [Header("Components")]
    [SerializeField] private Image _bar;
    [SerializeField] private Image _backgroundBar;

    [Space]
    [Header("Animation Option")]
    [SerializeField] private float _totalDuration;

    [Space]
    [Header("Option")]
    [Tooltip("�������� �����ϴ°�, ä�����°� Ȯ��")]
    [SerializeField] private BarType _barType;


    private float _duration;


    public void Update()
    {
        UpdateBar();
    }




    /// <summary></summary>
    public void SetBar(float maxValue, float currentValue)
    {
        if (_barType == BarType.Decrease)
        {
            _backgroundBar.fillAmount = _bar.fillAmount;
            _bar.fillAmount = currentValue / maxValue;
        }

        else if (_barType == BarType.Increase)
        {
            _bar.fillAmount = _backgroundBar.fillAmount;
            _backgroundBar.fillAmount = currentValue / maxValue;
        }

        _duration = 0;
    }

    //�ִϸ��̼��� �������� �ʰ� Bar ���¸� ������Ʈ �ϴ� �Լ�
    public void SetBarAfterNoAnime(float maxValue, float currentValue)
    {
        _bar.fillAmount = currentValue / maxValue;
        _backgroundBar.fillAmount = currentValue / maxValue;
    }


    private void UpdateBar()
    {
        if (_barType == BarType.Decrease)
        {
            if (_bar.fillAmount != _backgroundBar.fillAmount)
            {
                _duration += Time.deltaTime;
                float percent = _duration / _totalDuration;
                percent = percent * percent * (3f - 2f * percent);

                _backgroundBar.fillAmount = Mathf.Lerp(_backgroundBar.fillAmount, _bar.fillAmount, percent);
            }

            else
            {
                _duration = 0;
            }
        }

        else if (_barType == BarType.Increase)
        {
            if (_bar.fillAmount != _backgroundBar.fillAmount)
            {
                _duration += Time.deltaTime;
                float percent = _duration / _totalDuration;
                percent = percent * percent * (3f - 2f * percent);

                _bar.fillAmount = Mathf.Lerp(_bar.fillAmount, _backgroundBar.fillAmount, percent);
            }

            else
            {
                _duration = 0;
            }
        }

    }
}
