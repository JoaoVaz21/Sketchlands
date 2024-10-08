using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private ColliderNotifier goalLock;

    [SerializeField] private ColliderNotifier key;
    [SerializeField] private AudioSource keySoundSource;

    private bool _hasReachedLock = false;
    // Start is called before the first frame update

    void Start()
    {
        key.TriggerStarted += OnKeyCollision;
        goalLock.TriggerStarted += OnLockCollision;
    }

    private void OnKeyCollision(GameObject otherCollider)
    {
        if (otherCollider.CompareTag("Player") && !_hasReachedLock)
        {
            keySoundSource.Play();
            _hasReachedLock = true;
            key.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //In the future this should be replaced by an animation
            Destroy(goalLock.gameObject);
        }
    }
    private void OnLockCollision(GameObject otherCollider)
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
