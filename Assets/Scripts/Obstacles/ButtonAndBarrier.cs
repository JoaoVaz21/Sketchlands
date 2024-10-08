using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Obstacles
{
    public class ButtonAndBarrier : MonoBehaviour
    {
        [SerializeField] private GameObject barrier;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private float velocity = 0.005f;
        [SerializeField] private Color offColor;
        [SerializeField] private Color onColor;
        [SerializeField] private AudioSource buttonAudioSource;
        private Vector3 _startPosition;
        private List<Collider2D> _objectcolliding = new List<Collider2D>();
        private bool _isPressed = false;
        private void Awake()
        {
            this._startPosition = barrier.transform.localPosition;
        }

        private void Update()
        {

                barrier.transform.localPosition = Vector3.Lerp (barrier.transform.localPosition, _isPressed?targetPosition:_startPosition, velocity*Time.deltaTime);
            

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) return;
            if (_objectcolliding.Count == 0)
            {
                buttonAudioSource.Play();
            }
            _objectcolliding.Add(other);
            _isPressed = true;
            GetComponent<SpriteRenderer>().color = onColor;
        }

   
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) return;
            _objectcolliding.Remove(other);
            if (_objectcolliding.Count == 0)
            {
                _isPressed = false;
                GetComponent<SpriteRenderer>().color = offColor;
            }
        }
    }
}
