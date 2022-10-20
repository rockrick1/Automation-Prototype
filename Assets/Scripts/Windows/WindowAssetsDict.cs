using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Windows
{
    [CreateAssetMenu(menuName = "Windows/WindowAssetsDict", order = 'W')]
    public class WindowAssetsDict : ScriptableObject
    {
        [SerializeField] List<WindowAssetData> _windows;

        public GameObject GetWindowOfType(WindowType type)
        {
            var data = _windows.Find(data => data.Type == type);
            return data?.Window ?? null;
        }
    }

    [Serializable]
    public class WindowAssetData
    {
        public WindowType Type;
        public GameObject Window;
    }
}