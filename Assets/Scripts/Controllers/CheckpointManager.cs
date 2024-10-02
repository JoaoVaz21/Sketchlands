using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; set; }

    private Vector3 _currentCheckpoint;
    [SerializeField] private GameObject _player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _currentCheckpoint = _player.transform.position;
    }

    public void SetCheckout(Vector3 checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }

    public void RestartLevel()
    {
        UiManager.Instance.Fade(1);
        StartCoroutine(setPlayerPosition(0.3f));
    }
    private IEnumerator setPlayerPosition(float delay)
        {
            yield return new WaitForSeconds(delay);
        _player.transform.position = _currentCheckpoint;

    }

}
