using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private InkManager inkManager;

    [SerializeField] private Slider inkSlider;
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


    
}
