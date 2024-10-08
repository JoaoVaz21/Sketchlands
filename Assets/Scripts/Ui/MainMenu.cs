using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioClip levelClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartClicked()
    {
        AudioManager.Instance.ChangeMusic(1,levelClip);
        StartCoroutine(DelayedLoadScene(0.5f));
    }
    private IEnumerator DelayedLoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);

    }
}
