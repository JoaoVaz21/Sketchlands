using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class WallCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayer;
        public Action CollidedWithGround;
        

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((collision.gameObject.layer & (1 << collisionLayer)) != 0)
            {
                CollidedWithGround?.Invoke();
            }

        }
    }
}