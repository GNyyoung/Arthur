using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class UINavigation
    {
        private static readonly Stack<UIView> HistoryStack = new Stack<UIView>();
        private static readonly Dictionary<string, UIView> UIViews = new Dictionary<string, UIView>();

        public static UIView Push(string viewName)
        {
            if (UIViews.TryGetValue(viewName, out var view) == true)
            {
                return Push(view);
            }
            else
            {
                Debug.LogWarning($"viewName 매개변수와 일치하는 View가 존재하지 않습니다 : {viewName}");
                return null;
            }
        }

        public static UIView Push(UIView view)
        {
            if (HistoryStack.Contains(view) == true)
            {
                PopTo(view);
            }
            else
            {
                view.Show();
                HistoryStack.Push(view);    
            }

            return view;
        }

        public static UIView Pop()
        {
            var view = HistoryStack.Pop();
            view.Hide();

            return view;
        }

        public static UIView PopTo(string viewName)
        {
            if (UIViews.TryGetValue(viewName, out var view) == true)
            {
                return PopTo(view);
            }
            else
            {
                Debug.LogWarning($"viewName 매개변수와 일치하는 View가 존재하지 않습니다 : {viewName}");
                return null;
            }
        }

        public static UIView PopTo(UIView view)
        {
            UIView previousView = null;
            while (HistoryStack.Count > 0)
            {
                previousView = HistoryStack.Pop();
                previousView.Hide();
                if (previousView == view)
                {   
                    break;
                }
            }

            return previousView;
        }

        public static void AddView(UIView view)
        {
            Debug.Log(view.gameObject.name);
            UIViews.Add(view.gameObject.name, view);
        }

        public static UIView PeekStack()
        {
            if (HistoryStack.Count == 0)
            {
                return null;
            }
            else
            {
                return HistoryStack.Peek();   
            }
        }

        public static UIView GetView(string viewName)
        {
            return UIViews[viewName];
        }
    }
}