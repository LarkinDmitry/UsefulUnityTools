using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KAKuBCE.UsefulUnityTools
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action Press { get; set; }
        public Action Release { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
#endif
            Press?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
#endif
            Release?.Invoke();
        }
    }
}