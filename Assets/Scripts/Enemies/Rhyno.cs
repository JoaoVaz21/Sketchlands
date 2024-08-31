using Assets.Scripts.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RhynoState
{
    Stopped,
    Charging,
    Running
}

public class Rhyno : MonoBehaviour
{
    [SerializeField] float chargingTime = 2;
    [SerializeField] float velocity = 5;
    [SerializeField] float distanceToCheck = 10;
    [SerializeField] private WallCheck wallCheck;
    [SerializeField] private LayerMask layerToHit;
    private Animator _animator;
    private Rigidbody2D _rb;
    private RhynoState _state;
    private float _currentCharge;
    private float _direction = 0;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _state = RhynoState.Stopped;
        wallCheck.CollidedWithWall += OnCollidedWithWall;

    }



    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.Raycast(this.transform.position - new Vector3(distanceToCheck/2,0,0), Vector2.right, distanceToCheck,layerToHit);
        Debug.DrawLine(this.transform.position - new Vector3(distanceToCheck / 2, 0, 0), this.transform.position + new Vector3(distanceToCheck / 2, 0, 0), Color.red,2,false);

        if (hit && hit.collider.gameObject.CompareTag("Player"))
        {
            Vector3 scale = transform.localScale;
            if (_state != RhynoState.Running)
            {
                if (hit.collider.transform.position.x < transform.position.x)
                {
                    // Player is to the left; face left
                    scale.x = -Mathf.Abs(scale.x);
                    _direction = -1;
                }
                else
                {
                    // Player is to the right; face right
                    scale.x = Mathf.Abs(scale.x);
                    _direction = 1;
                }
            }
            transform.localScale = scale;
 
            switch (_state)
            {
                case RhynoState.Stopped:
                    Debug.Log("Changing state to charging");
                    _state = RhynoState.Charging;
                    _animator.SetBool("Charging", true);
                    _currentCharge = 0;
                    break;
                case RhynoState.Charging:
                    _currentCharge += Time.deltaTime;
                    if(_currentCharge >= chargingTime)
                    {
                        _state = RhynoState.Running;
                        _animator.SetBool("Running", true);
                        Debug.Log("Changing state to running");

                    }
                    break;

            }
        }
        else
        {
            if(_state == RhynoState.Charging)
            {
                _state = RhynoState.Stopped;
                _animator.SetBool("Charging", false);
                Debug.Log("Changing state to stop");

            }
        }

    }
    private void FixedUpdate()
    {
        if(_state == RhynoState.Running)
        {
            _rb.velocity = new Vector2(velocity * _direction, _rb.velocity.y);
            Debug.Log("rhybi velocity = " + velocity * _direction);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    private void OnCollidedWithWall()
    {
        if (_state == RhynoState.Running)
        {
            this._animator.SetBool("Destroy", true);
            _rb.velocity = Vector2.zero;

            //TODO this should be after the animation
            Destroy(this.gameObject); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("ink"))
        {
            Destroy(collision.gameObject);
        }
    }
}
