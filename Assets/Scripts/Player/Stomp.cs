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
        if (other.gameObject.CompareTag("WeakPoint"))
        {
            other.gameObject.GetComponent<EnemyReceiveDamage>().ReceiveDamage();
        }    
    }
    
}
