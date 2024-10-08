using Assets.Scripts.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private LayerMask ignoreLayerMask;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSource;
    private Animator _animator;
    private Rigidbody2D _rb;
    private RhynoState _state;
    private float _currentCharge;
    private float _direction = 0;
    private Vector3 _startScale;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _state = RhynoState.Stopped;
        wallCheck.CollidedWithWall += OnCollidedWithWall;
        _startScale = this.transform.localScale;

    }



    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position - new Vector3(distanceToCheck / 2, 0, 0), this.transform.position + new Vector3(distanceToCheck / 2, 0, 0), Color.red, 2, false);

        var hitsLeft = Physics2D.RaycastAll(this.transform.position, Vector2.left, distanceToCheck/2);
        var filteredHitsLeft = hitsLeft.Where(hit => ((1 << hit.collider.gameObject.layer) & ignoreLayerMask) == 0).ToArray();

        var hitsRight = Physics2D.RaycastAll(this.transform.position, Vector2.right, distanceToCheck / 2);
        var filteredHitsRight= hitsRight.Where(hit => ((1 << hit.collider.gameObject.layer) & ignoreLayerMask) == 0).ToArray();

        if ((filteredHitsLeft.Count() >0 && filteredHitsLeft[0].collider.gameObject.CompareTag("Player")) || (filteredHitsRight.Count() > 0 && filteredHitsRight[0].collider.gameObject.CompareTag("Player")))
        {
            var hit =(filteredHitsLeft.Count() > 0 && filteredHitsLeft[0].collider.gameObject.CompareTag("Player")) ? filteredHitsLeft[0] : filteredHitsRight[0];
            Vector3 scale = _startScale;
            if (_state != RhynoState.Running)
            {
                if (hit.collider.transform.position.x < transform.position.x)
                {
                    // Player is to the left; face left
                    scale.x = -scale.x;
                    _direction = -1;
                }
                else
                {
                    _direction = 1;
                }
                transform.localScale = scale;
            }

            switch (_state)
            {
                case RhynoState.Stopped:
                    _state = RhynoState.Charging;
                    _animator.SetBool("Charging", true);
                    audioSource.clip = clips[0];
                    audioSource.Play();
                    _currentCharge = 0;
                    break;
                case RhynoState.Charging:
                    _currentCharge += Time.deltaTime;
                    if(_currentCharge >= chargingTime)
                    {
                        _state = RhynoState.Running;
                        audioSource.clip = clips[1];
                        audioSource.Play();
                        _animator.SetBool("Running", true);

                    }
                    break;

            }
        }
        else
        {
            if(_state == RhynoState.Charging)
            {
                _state = RhynoState.Stopped;
                audioSource.Stop();
                _animator.SetBool("Charging", false);

            }
        }

    }
    private void FixedUpdate()
    {
        if(_state == RhynoState.Running)
        {
            _rb.velocity = new Vector2(velocity * _direction, _rb.velocity.y);
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
            _animator.SetBool("Charging", false);
            this._animator.SetBool("Running", false);
            _rb.velocity = Vector2.zero;
            audioSource.Stop();
            _state = RhynoState.Stopped;
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
