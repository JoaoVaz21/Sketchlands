using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class Goal : MonoBehaviour
{
    [SerializeField] private ColliderNotifier goalLock;

    [SerializeField] private ColliderNotifier key;
    [SerializeField] private int nextScene;
    [SerializeField] private Animator lockAnimationController;
    [SerializeField] private string textOnKeyAdquired;

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
            lockAnimationController.SetTrigger("Open");
            DialogManager.Instance.ActivateDialogBox(textOnKeyAdquired);
            StartCoroutine(DeactivateDialogBoxWithDelay());
       
        }
    }
    private IEnumerator DeactivateDialogBoxWithDelay()
    {

        yield return new WaitForSeconds(2);

        DialogManager.Instance.DeactivateDialogBox();
    }

    private void OnLockCollision(GameObject otherCollider)
    {
        if (otherCollider.CompareTag("Player") && _hasReachedLock)
        {
            UiManager.Instance.Fade(1);
            SceneManager.LoadScene(nextScene);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
