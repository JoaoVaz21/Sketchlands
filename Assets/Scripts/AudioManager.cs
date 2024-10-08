using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{



    public static AudioManager Instance { get; set; }
    [SerializeField] private Toggle soundToggle;  // Reference to the UI toggle
    private AudioSource _musicSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            _musicSource = this.GetComponent<AudioSource>();
            _musicSource.Play();
            soundToggle.onValueChanged.AddListener(delegate { ToggleAudio(soundToggle.isOn); });

        }
        else
        {
            Destroy(this);
        }
    }
    private void ToggleAudio(bool soundOn)
    {
        if (soundOn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }
    public void ChangeMusic(float time,AudioClip clip)
    {
        StartCoroutine(FadeAndChangeMusic(time,clip));
    }
    IEnumerator FadeAndChangeMusic(float secondsToFade, AudioClip clip)
    {

        for (float i = 0; i <= secondsToFade * 0.5; i += Time.deltaTime)
        {
            // set color with i as alpha
            AudioListener.volume = i / (secondsToFade * 0.5f);
            yield return null;
        }
        AudioListener.volume = 0;
        _musicSource.clip = clip;
        _musicSource.Play();
        for (float i = secondsToFade * 0.5f; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            AudioListener.volume = i / (secondsToFade * 0.5f);
            yield return null;
        }
        AudioListener.volume =1;

    }
}
