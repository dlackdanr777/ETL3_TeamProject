using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Muks.PCUI
{

    public enum VisibleState
    {
        Disappeared, // �����
        Disappearing, //������� ��
        Appeared, //��Ÿ��
        Appearing, //��Ÿ������
    }


    public abstract class UIView : MonoBehaviour, IPointerDownHandler
    {
        ///  <summary> Appeared, Disappeared�϶� Show(), Hide()���� ����</summary>
        public VisibleState VisibleState;

        /// <summary> UIâ�� Ŭ�������� ����� �븮�� </summary>
        public Action OnFocus;

        protected UINavigation _uiNav;

        protected RectTransform _rectTransform;


        public virtual void Init(UINavigation uiNav)
        {
            _uiNav = uiNav;
            _rectTransform = GetComponent<RectTransform>();
        }


        /// <summary>UI�� �ҷ����� ����Ǵ� �Լ�</summary>
        public abstract void Show(Action onCompleted = null);


        /// <summary>UI�� ���� ����Ǵ� �Լ�</summary>
        public abstract void Hide(Action onCompleted = null);


        /// <summary>�ش� UI â�� Ŭ���ϸ� ����� �Լ�</summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            OnFocus?.Invoke();
        }
    }
}

