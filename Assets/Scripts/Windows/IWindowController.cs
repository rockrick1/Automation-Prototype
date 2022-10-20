using System;
using System.Collections.Generic;

namespace Assets.Scripts.Windows
{
    public interface IWindowController
    {
        public void Show(Action close, Dictionary<string, object> args);
        public void OnClose();
    }
}