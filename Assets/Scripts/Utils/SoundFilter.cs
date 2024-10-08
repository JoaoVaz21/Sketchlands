using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFilter : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform player;
    [SerializeField]public float distanceThreshold = 5f;
    private float _maxSound;
    void Start()
    {
        // Find the player GameObject by tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;  // Get the Transform of the player
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the player is tagged 'Player'.");
        }

        audioSource = GetComponent<AudioSource>();
        _maxSound = audioSource.volume;
    }

    void Update()
    {
        if (player == null) return;  // Ensure the player is found

        float distance = Vector2.Distance(transform.position, player.position);
        audioSource.volume = distance > distanceThreshold ? 0 : _maxSound;
    }
}
