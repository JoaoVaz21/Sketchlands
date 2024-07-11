using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVelocitySettable
{
    void SetVelocity(Vector2 velocity);
}
public class Arrow : MonoBehaviour, IVelocitySettable
{
    [SerializeField] private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVelocity(Vector2 velocity)
    {
      _rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ink"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer"))
        {
            Destroy(this.gameObject);
        }
    }
}
