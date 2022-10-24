using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class MultiButton : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public UnityEvent LeftClick;
        [HideInInspector] public UnityEvent MiddleClick;
        [HideInInspector] public UnityEvent RightClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                LeftClick.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                MiddleClick.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClick.Invoke();
            }
        }
    }
}