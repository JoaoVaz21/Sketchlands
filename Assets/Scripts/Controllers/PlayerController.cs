
using UnityEngine;


    public class PlayerController : MonoBehaviour, IInputController
    {
        //TODO change this to use the new input system
        public bool RetrieveJumpInput()
        {
            return Input.GetButtonDown("Jump");
        }

        public bool RetrieveJumpHeld()
        {
            return Input.GetButton("Jump");
        }

        public float RetrieveMoveInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

