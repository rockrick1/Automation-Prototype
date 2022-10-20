using Assets.Scripts.Windows;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Player
{
    public class PlayerController : Dependable
    {
        [SerializeField] PlayerMovementController _playerMovementController;
        [SerializeField] PlayerInventoryController _playerInventoryController;
        [SerializeField] Camera _playerCamera;

        public Camera PlayerCamera => _playerCamera;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                var windowManager = DependencyResolver.Instance.Resolve<WindowManager>();
                if (windowManager.IsWindowOpen(WindowType.Inventory))
                {
                    windowManager.CloseWindowOfType(WindowType.Inventory);
                }
                else
                {
                    windowManager.ShowWindow(WindowType.Inventory, null);
                }
            }
        }
    }
}