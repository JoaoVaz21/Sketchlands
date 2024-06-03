using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;

public class Stomp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("stomp entered collision");
        if (other.gameObject.CompareTag("WeakPoint"))
        {
            Debug.Log("stomp entered collision with weak point");
            other.gameObject.GetComponent<EnemyReceiveDamage>().ReceiveDamage();
        }    
    }
    
}
