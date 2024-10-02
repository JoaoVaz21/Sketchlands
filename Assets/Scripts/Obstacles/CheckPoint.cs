using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator _animator;
    private bool _passed = false;
    // Start is called before the first frame update
    void Start()
    {
        this._animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Checkpoint collision");
        if(collision.gameObject.CompareTag("Player") && !_passed)
        {
            Debug.Log("Checkpoint collision - trigger animation");

            _passed = true;
            _animator.SetTrigger("Activate");
            CheckpointManager.Instance.SetCheckout(collision.gameObject.transform.position);
        }
        
    }
}
