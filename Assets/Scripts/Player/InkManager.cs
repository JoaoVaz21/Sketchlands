using System;
using System.Collections;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Player
{
    public class InkManager : MonoBehaviour
    {
        [SerializeField] private float minDistance = 0.05f;
        [SerializeField] private float lineThickness = 0.1f;
        [SerializeField] private GameObject drawableObject; 
        [SerializeField] private int maxInk = 100;
        [SerializeField] private float maxDistanceToDrain = 10f;
        [SerializeField] private LayerMask drainLayer;
        [SerializeField] private SpriteRenderer penRenderer;
        [SerializeField] private SpriteRenderer drainLimitRenderer;
        [SerializeField] private float distancePerInk =2;
        private Mesh _mesh;
        private DrawableObject _currentDrawing;
        private Vector3 _lastMousePosition;
        private bool _drawing;
        private bool _draining;
        private RaycastHit2D _currentDrainingHit;
        private int _currentInk;
        private float _currentDistanceBetweenInkTick;
        public int CurrentInk
        {
            get => _currentInk;
            private set
            {
                _currentInk = value;
                InkChanged?.Invoke(_currentInk);
            }
        }

        //Events
        public event Action<int> InkChanged;

        private void Awake()
        {
            this._currentInk = maxInk;
            StartCoroutine(RefreshInk());
        }

        private void FixedUpdate()
        {
            UpdateMesh();    
            UpdateDrain();
        }

        private IEnumerator RefreshInk()
        {
            while (true)
            {
                if (_currentInk < maxInk && !_drawing)
                {
                    CurrentInk++;
                }
                yield return new WaitForSeconds(0.02f);


            }
        }

        #region Draw forms


        private void OnDrawingCollision()
        {
            _drawing = false;
            //TODO animate drawing failed
            _currentDrawing.CollidedWhileCreating-=OnDrawingCollision;
            _currentDrawing.CompleteMesh();



        }
        
        public void Draw(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                Debug.Log("draw started");
                if (CurrentInk > 0)
                {
                    _currentDrawing = Instantiate(drawableObject, Vector3.zero, Quaternion.identity)
                        .GetComponent<DrawableObject>();
                    _lastMousePosition = InputHelper.GetMousePosition();
                    _currentDrawing.InitializeDrawing(_lastMousePosition);
                    _currentDistanceBetweenInkTick = 0;
                    _currentDrawing.CollidedWhileCreating += OnDrawingCollision;
                    _drawing = true;
                }
            }
            else if (callbackContext.performed)
            {
    
            }else if (callbackContext.canceled)
            {
                if (_drawing)
                {
                    _drawing = false;
                    _currentDrawing.CollidedWhileCreating -= OnDrawingCollision;
                    _currentDrawing.CompleteMesh();
                    _currentDrawing = null;
                }
            }
       
        }

        private void UpdateMesh()
        {
            if (_drawing)
            {
                if (CurrentInk > 0)
                {
                    var currentMousePosition = InputHelper.GetMousePosition();
                    if (Vector3.Distance(currentMousePosition, _lastMousePosition) < minDistance) return;
                    var distanceVector = currentMousePosition - _lastMousePosition;
                    _currentDistanceBetweenInkTick += distanceVector.magnitude;
                    _currentDrawing.AddVerticesToMesh(currentMousePosition, distanceVector,
                        lineThickness);
                    _lastMousePosition = currentMousePosition;
                    if (_currentDistanceBetweenInkTick> distancePerInk)
                    {
                        var inkUsed = (int) Math.Floor(_currentDistanceBetweenInkTick / distancePerInk);
                        _currentDrawing.InkUsed+=inkUsed;
                        CurrentInk-=inkUsed;
                        _currentDistanceBetweenInkTick -= distancePerInk * inkUsed;
                    }
                }
                else
                {
                    _drawing = false;
                    _currentDrawing.CompleteMesh();
                }
            }
        }
    #endregion

    #region Drain Ink

    private void UpdateDrain()
    {
        if (_draining)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, InputHelper.GetMousePosition()-transform.position, maxDistanceToDrain, drainLayer);
            _currentDrainingHit = hit;
            if (hit.collider != null)
            {
                if (!penRenderer.enabled)
                {
                    penRenderer.enabled = true;
                }
                penRenderer.gameObject.transform.position =_currentDrainingHit.point;
            }
            else
            {
                if (penRenderer.enabled)
                {
                    penRenderer.enabled = false;
                }
            }
        }
    }
    public void Drain(InputAction.CallbackContext callbackContext)
    {

        if (callbackContext.performed)
        {
            _draining = true;
            drainLimitRenderer.enabled = true;
        }
        else if (callbackContext.canceled)
        {
            Debug.Log("position to ink from: " + transform.position);
            if (_currentDrainingHit.collider != null)
            {
                Destroy(_currentDrainingHit.collider.gameObject);
            }
            penRenderer.enabled = false;
            drainLimitRenderer.enabled = false;
            _draining = false;
        }
    }


    #endregion
    }
}
