
using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(IInputController))]
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float jumpPower = 3f;
        [SerializeField, Range(0f, 100f)] private float maxFallSpeed = 3f;
        [SerializeField, Range(0f, 100f)] private float fallAcceleration = 1.7f;
        [SerializeField, Range(0f, 100f)] private float jumpEndEarlyGravityModifier = 1.7f;
        [SerializeField, Range(0f, 1f)] private float coyoteTime = 0.15f;
        [SerializeField, Range(0f, 1f)] private float jumpBuffer = 0.2f;
        [SerializeField, Range(0f, -10f)] private float groundingForce=-1.5f;
        [FormerlySerializedAs("_maxSpeed")] [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float acceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float groundDeceleration=30f;
        [SerializeField, Range(0f, 100f)] private float airDeceleration=30f;
        [SerializeField, Range(0, 0.5f)] private float grounderDistance=-0.05f;
        [SerializeField] private LayerMask groundLayer;

        private IInputController _controller;
        private Rigidbody2D _rb;
        private Vector2 _velocity;
        private Vector2 _direction;

        private int _jumpPhase;
        private float _jumpSpeed;
        private float _time;
        private float _timeJumpWasPressed;

        private bool _jumpToDo;
        private bool _onGround;
        private bool _endedJumpEarly;
        private bool _bufferedJumpUsable;
        private bool _coyoteUsable;
        private float _frameLeftGrounded = float.MinValue;
        private CapsuleCollider2D _col;
        private bool _cachedQueryStartInColliders;
        private Animator _animator;
        private float _xScale;
        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + jumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_onGround && _time < _frameLeftGrounded + coyoteTime;


        // Start is called before the first frame update
        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _controller = GetComponent<IInputController>();
            _col = GetComponent<CapsuleCollider2D>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            _animator = GetComponent<Animator>();
            _xScale = transform.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            _time += Time.deltaTime;
            _direction.x = _controller.RetrieveMoveInput();
            if (_controller.RetrieveJumpInput())
            {
                _jumpToDo = true;
                _timeJumpWasPressed = _time;
            }
        }
        private void FixedUpdate()
        {
            _velocity = _rb.velocity;
            CheckCollisions();
            HandleJump();
            HandleDirection();
            HandleGravity();

            _rb.velocity = _velocity;
        }

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size*_col.transform.localScale.y, _col.direction, 0, Vector2.down, grounderDistance,groundLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, grounderDistance);

            // Hit a Ceiling
            if (ceilingHit) _velocity.y = Mathf.Min(0, _velocity.y);

            // Landed on the Ground
            if (!_onGround && groundHit)
            {
                _animator.SetBool("isJumping",false);
                _onGround = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
            }
            // Left the Ground
            else if (_onGround && !groundHit)
            {
                _onGround = false;
                _frameLeftGrounded = _time;
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }
        private void HandleJump()
        {
            if (!_endedJumpEarly && !_onGround && !_controller.RetrieveJumpHeld() && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToDo && !HasBufferedJump) return;

            if (_onGround || CanUseCoyote) ExecuteJump();

            _jumpToDo = false; 
        }
        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _velocity.y = jumpPower;
            _animator.SetBool("isJumping",true);

        }
        private void HandleGravity()
        {
            if (!_onGround )
            {
                var inAirGravity =fallAcceleration;
                if (_endedJumpEarly && _velocity.y > 0) inAirGravity *= jumpEndEarlyGravityModifier;
                _velocity.y = Mathf.MoveTowards(_velocity.y, -maxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        private void HandleDirection()
        {
            if (_direction.x == 0)
            {
                var deceleration = _onGround ? groundDeceleration : airDeceleration;
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {

                _velocity.x = Mathf.MoveTowards(_velocity.x, _direction.x * maxSpeed, acceleration * Time.fixedDeltaTime);
            }
            if (_direction.x != 0)
            {
                this.transform.localScale = new Vector3(_direction.x*_xScale, this.transform.localScale.y,
                    this.transform.localScale.z);
            }
            _animator.SetFloat("velocityX",Math.Abs(_direction.x));

        }

    }
