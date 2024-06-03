using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Obstacles
{
    public class ButtonAndBarrier : MonoBehaviour
    {
        [SerializeField] private GameObject barrier;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private float velocity = 0.005f;
        private Vector3 _startPosition;

        private bool _isPressed = false;
        private void Awake()
        {
            this._startPosition = barrier.transform.localPosition;
        }

        private void Update()
        {

                barrier.transform.localPosition = Vector3.Lerp (barrier.transform.localPosition, _isPressed?targetPosition:_startPosition, velocity);
            

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isPressed = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _isPressed = false;
        }
    }
}
