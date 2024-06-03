using UnityEngine;
using UnityEngine.InputSystem;

namespace Helpers
{
    public static class InputHelper
    {
        public static Vector3 GetMousePosition()
        {

            if (Camera.main != null)
            {
                //TODO this should use the new input system instead of directly checking the mouse
                var mousePoint = Mouse.current.position.ReadValue();
           
                var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePoint.x,mousePoint.y,Camera.main.nearClipPlane));
                return mousePosition;
            }

            return Vector3.zero;
        }
    }
}