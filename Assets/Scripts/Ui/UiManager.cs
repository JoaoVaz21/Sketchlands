using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private InkManager inkManager;

    [SerializeField] private Slider inkSlider;
    [SerializeField] private Image fadeImage;

    public static UiManager Instance { get; set; }

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
    }

    // Start is called before the first frame update
    void Start()
    {
        inkManager.InkChanged += OnInkChanged;
        inkSlider.value = inkManager.CurrentInk;
    }

    private void OnInkChanged(int obj)
    {
        inkSlider.value = obj;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fade(float secondsToFade)
    {
        StartCoroutine(FadeImage(secondsToFade));
    }

    IEnumerator FadeImage(float secondsToFade )
    {

        for (float i = 0; i <= secondsToFade*0.2; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadeImage.color = new Color(fadeImage.color.r,fadeImage.color.g,fadeImage.color.b, i/(secondsToFade*0.2f));
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

        yield return new WaitForSeconds(secondsToFade*0.5f);
        for (float i = secondsToFade * 0.3f; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i / (secondsToFade * 0.3f));
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);

    }

}
