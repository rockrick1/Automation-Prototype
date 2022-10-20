using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Windows
{
    public class InventoryWindowController : MonoBehaviour, IWindowController
    {
        public void OnClose()
        {
        }

        public void Show(Action close, Dictionary<string, object> args)
        {
        }
    }
}