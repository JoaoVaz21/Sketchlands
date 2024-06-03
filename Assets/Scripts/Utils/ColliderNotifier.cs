using System;
using UnityEngine;

namespace Utils
{
    public class ColliderNotifier : MonoBehaviour
    {
        public event Action<GameObject> CollisionStarted ;
        public event Action<GameObject> CollisionEnded ;
        public event Action<GameObject> TriggerStarted ;
        public event Action<GameObject> TriggerEnded ;


        private void OnCollisionEnter2D(Collision2D other)
        {
            this.CollisionStarted?.Invoke(other.gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            this.CollisionEnded?.Invoke(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerStarted?.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerEnded?.Invoke(other.gameObject);

        }
    }
}