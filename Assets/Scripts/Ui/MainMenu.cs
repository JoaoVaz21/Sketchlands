using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        SceneManager.LoadScene(1);
    }
    public void OnSoundTogled(bool sound)
    {
        Debug.Log("Sound toggled: " + sound);
        //TODO interact with sound manager
    }
}
