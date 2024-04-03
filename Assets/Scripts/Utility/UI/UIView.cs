using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Muks.PCUI
{

    public enum VisibleState
    {
        Disappeared, // 사라짐
        Disappearing, //사라지는 중
        Appeared, //나타남
        Appearing, //나타나는중
    }


    public abstract class UIView : MonoBehaviour, IPointerDownHandler
    {
        ///  <summary> Appeared, Disappeared일때 Show(), Hide()실행 가능</summary>
        public VisibleState VisibleState;

        /// <summary> UI창을 클릭했을때 실행될 대리자 </summary>
        public Action OnFocus;

        protected UINavigation _uiNav;

        protected RectTransform _rectTransform;


        public virtual void Init(UINavigation uiNav)
        {
            _uiNav = uiNav;
            _rectTransform = GetComponent<RectTransform>();
        }


        /// <summary>UI를 불러낼때 실행되는 함수</summary>
        public abstract void Show(Action onCompleted = null);


        /// <summary>UI를 끌때 실행되는 함수</summary>
        public abstract void Hide(Action onCompleted = null);


        /// <summary>해당 UI 창을 클릭하면 실행될 함수</summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            OnFocus?.Invoke();
        }
    }
}

