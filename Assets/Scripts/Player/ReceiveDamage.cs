using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class ReceiveDamage : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("player entered collision");

            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("player entered with enemy");
                //TODO animation
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}