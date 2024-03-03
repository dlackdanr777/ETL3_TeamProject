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


        [Tooltip("�� Ŭ�������� ������ UIView�� �ִ� ��")]
        [SerializeField] private ViewDicStruct[] _uiViews;

        /// <summary> ViewDicStruct���� ������ Name�� Key��, UIView�� ������ �����س��� ��ųʸ� </summary>
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
            //uiViewList�� ����� ���� ��ųʸ��� ����
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


        /// <summary>�̸��� �޾� �ش��ϴ� UIView�� �����ִ� �Լ�</summary>
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


        /// <summary>��Ŀ������ UI�� �ݴ� �Լ�</summary>
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


        /// <summary> viewName�� Ȯ���� �ش� UI�� �ݴ� �Լ�</summary>
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


        /// <summary>���� �ֻ�ܿ� ��ġ�� UIView�� �����ϴ� �Լ�</summary>
        public UIView GetTopView()
        {
            if (_activeViewList.Count == 0)
                return null;

            return _activeViewList.First.Value;
        }


        /// <summary>�����ִ� UI�� VisibleState�� Ȯ�� �� bool���� �����ϴ� �Լ�</summary>
        private bool ViewsVisibleStateCheck()
        {
            foreach (UIView view in _viewDic.Values)
            {
                if (view.VisibleState == VisibleState.Disappearing || view.VisibleState == VisibleState.Appearing)
                {
                    Debug.Log("UI�� �����ų� ������ �� �Դϴ�.");
                    return false;
                }
            }

            return true;
        }
    }
}

