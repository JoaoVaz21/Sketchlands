using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class WallCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayer;
        public Action CollidedWithWall;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & collisionLayer) != 0)
            {
                CollidedWithWall?.Invoke();
            }
        }
    
    }
}