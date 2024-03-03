using System;
using System.Collections.Generic;
using UnityEngine;

namespace Muks.PCUI
{
    public class UINavigation : MonoBehaviour
    {
        [Serializable]
        public struct ViewDicStruct
        {
            [Tooltip("Key")]
            public string Name;

            [Tooltip("Value")]
            public UIView UIView;
        }


        [Tooltip("이 클래스에서 관리할 UIView를 넣는 곳")]
        [SerializeField] private ViewDicStruct[] _uiViews;

        /// <summary> ViewDicStruct에서 설정한 Name을 Key로, UIView를 값으로 저장해놓는 딕셔너리 </summary>
        private Dictionary<string, UIView> _viewDic = new Dictionary<string, UIView>();

        private LinkedList<UIView> _activeViewList = new LinkedList<UIView>();

        public int Count => _activeViewList.Count;


        private void Start()
        {
            Init();
        }


        private void Init()
        {
            _viewDic.Clear();
            //uiViewList에 저장된 값을 딕셔너리에 저장
            for (int i = 0, count = _uiViews.Length; i < count; i++)
            {
                string name = _uiViews[i].Name;
                UIView uiView = _uiViews[i].UIView;
                _viewDic.Add(name, uiView);

                uiView.Init(this);

                uiView.OnFocus += () =>
                {
                    _activeViewList.Remove(uiView);
                    _activeViewList.AddFirst(uiView);
                    uiView.transform.SetAsLastSibling();
                };

               uiView.gameObject.SetActive(false);
            }
        }


        /// <summary>이름을 받아 해당하는 UIView를 열어주는 함수</summary>
        public void Show(string viewName, Action onCompleted = null)
        {
            if (_viewDic.TryGetValue(viewName, out UIView uiView))
            {
                if (!ViewsVisibleStateCheck())
                    return;

                if (!_activeViewList.Contains(uiView))
                {
                    _activeViewList.AddFirst(uiView);
                    uiView.Show(onCompleted);
                }
                else
                {
                    _activeViewList.Remove(uiView);
                    _activeViewList.AddFirst(uiView);
                }

                uiView.transform.SetAsLastSibling();
            }
        }


        /// <summary>포커스중인 UI를 닫는 함수</summary>
        public void Hide(Action onCompleted = null)
        {
            if (!ViewsVisibleStateCheck())
                return;

            if (_activeViewList.First == null)
                return;

            _activeViewList.First.Value.Hide(onCompleted);
            _activeViewList.RemoveFirst();

            if (_activeViewList.First == null)
                return;

            _activeViewList.First.Value.transform.SetAsLastSibling();
        }


        /// <summary> viewName을 확인해 해당 UI를 닫는 함수</summary>
        public void Hide(string viewName, Action onCompleted = null)
        {
            if (_viewDic.TryGetValue(viewName, out UIView uiView))
            {
                if (!ViewsVisibleStateCheck())
                    return;

                if (!_activeViewList.Contains(uiView))
                    return;

                _activeViewList.Remove(uiView);
                uiView.Hide(onCompleted);
            }
        }


        /// <summary>현재 최상단에 위치한 UIView를 리턴하는 함수</summary>
        public UIView GetTopView()
        {
            if (_activeViewList.Count == 0)
                return null;

            return _activeViewList.First.Value;
        }


        /// <summary>열려있는 UI의 VisibleState를 확인 후 bool값을 리턴하는 함수</summary>
        private bool ViewsVisibleStateCheck()
        {
            foreach (UIView view in _viewDic.Values)
            {
                if (view.VisibleState == VisibleState.Disappearing || view.VisibleState == VisibleState.Appearing)
                {
                    Debug.Log("UI가 열리거나 닫히는 중 입니다.");
                    return false;
                }
            }

            return true;
        }
    }
}

