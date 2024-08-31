using Assets.Scripts.Enemies;
using UnityEngine;

public class BasicEnemyInputController : MonoBehaviour,IInputController
{
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private WallCheck wallCheck;
    [SerializeField] private float distanceThreshold = 0.1f;
    private float _direction;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        _direction = (endPosition - _startPosition).x > 0 ? 1 : -1;
        wallCheck.CollidedWithWall += OnWallCollided;
    }

    private void FixedUpdate()
    {
        var distance = Mathf.Abs((endPosition.x - transform.localPosition.x));
        if (distance <= distanceThreshold)
        {
            _direction = -_direction;
            var aux = _startPosition;
            _startPosition = endPosition;
            endPosition = aux;
        }
    }

    public bool RetrieveJumpInput()
    {
        return false;
    }

    public bool RetrieveJumpHeld()
    {
        return false;
    }

    public float RetrieveMoveInput()
    {
        return _direction;
    }
    private void OnWallCollided()
    {
        _direction = -_direction;
        var aux = _startPosition;
        _startPosition = endPosition;
        endPosition = aux;
    }

    private void OnDestroy()
    {
        wallCheck.CollidedWithWall -= OnWallCollided;
    }
}