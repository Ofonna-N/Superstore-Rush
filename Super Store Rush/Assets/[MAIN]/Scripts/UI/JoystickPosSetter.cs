using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

namespace SuperStoreRush
{
    public class JoystickPosSetter : MonoBehaviour
    {
        [SerializeField]
        private OnScreenStick joystick;

        [SerializeField]
        private RectTransform joystickHolder;

      /*  private void Start()
        {
            joystickHolder = joystick.GetComponent<RectTransform>();
        }*/

        public void OnPointerDown(BaseEventData eventData)
        {
            var data = (eventData as PointerEventData);
            joystickHolder.position = data.pressPosition;
            joystick.OnPointerDown(data);
        }

        public void OnDrag(BaseEventData eventData)
        {
            var data = (eventData as PointerEventData);
            joystick.OnDrag(data);
        }

        public void OnPointerUp(BaseEventData eventData)
        {
            var data = (eventData as PointerEventData);
            joystick.OnPointerUp(data);
        }
    }
}
