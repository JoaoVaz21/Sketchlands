using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Goal : MonoBehaviour
{
    [SerializeField] private ColliderNotifier goalLock;

    [SerializeField] private ColliderNotifier key;

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
            _hasReachedLock = true;
            Destroy(key.gameObject);
            //In the future this should be replaced by an animation
            goalLock.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    private void OnLockCollision(GameObject otherCollider)
    {
        if (otherCollider.CompareTag("Player") && _hasReachedLock)
        {
           //TODO LevelComplete
           Debug.Log("Level completed");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
