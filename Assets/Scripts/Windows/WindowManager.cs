using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Windows
{
    public enum WindowType
    {
        Inventory,
        Belt,
        Assembler
    }

    public class WindowManager : Dependable
    {
        [SerializeField] WindowAssetsDict _windowAssetsDict;

        Dictionary<WindowType, GameObject> _windowStack;

        protected override void Start()
        {
            base.Start();
            _windowStack = new Dictionary<WindowType, GameObject>();
        }

        public void ShowWindow(WindowType type, Dictionary<string, object> args)
        {
            var windowObject = _windowAssetsDict.GetWindowOfType(type);
            GameObject go = Instantiate(windowObject);
            var windowController = go.GetComponent<IWindowController>();

            if (windowController == null)
            {
                Debug.LogError("Window doesn't have a proper controller! User IWindowController");
                return;
            }


            windowController.Show(() => CloseWindow(type, go, windowController), args);
            _windowStack.Add(type, go);
        }

        void CloseWindow(WindowType type, GameObject go, IWindowController controller)
        {
            controller.OnClose();
            _windowStack.Remove(type);
            Destroy(go);
        }

        public bool IsWindowOpen(WindowType type)
        {
            return _windowStack.ContainsKey(type);
        }

        internal void CloseWindowOfType(WindowType type)
        {
            if (!IsWindowOpen(type)) return;

            GameObject go = _windowStack[type];
            IWindowController controller = go.GetComponent<IWindowController>();
            CloseWindow(type, go, controller);
        }
    }
}