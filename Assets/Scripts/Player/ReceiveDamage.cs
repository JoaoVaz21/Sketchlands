using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class ReceiveDamage : MonoBehaviour
    {
        private bool _dying = false;
        private void OnCollisionEnter2D(Collision2D other)
        {

            if (other.gameObject.CompareTag("Enemy") && !_dying)
            {
                _dying = true;
                this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                this.gameObject.GetComponent<CharacterMovementController>().SetCantMove(true);
                this.gameObject.GetComponent<Animator>().ResetTrigger("Revive");
                this.gameObject.GetComponent<Animator>().SetTrigger("Death");
                StartCoroutine(AnimationEnd());
            }
        }

        private IEnumerator AnimationEnd()
        {
           yield return new WaitForSeconds(1.5f);
            CheckpointManager.Instance.RestartLevel();
            yield return new WaitForSeconds(0.5f);

            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            this.gameObject.GetComponent<CharacterMovementController>().SetCantMove(false);
            this.gameObject.GetComponent<Animator>().SetTrigger("Revive");
            this.gameObject.GetComponent<Animator>().ResetTrigger("Death");
            _dying = false;


        }
    }
}