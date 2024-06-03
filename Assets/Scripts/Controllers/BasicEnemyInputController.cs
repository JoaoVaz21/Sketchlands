using UnityEngine;

public class BasicEnemyInputController : MonoBehaviour,IInputController
{
    [SerializeField] private bool startRight;
    [SerializeField] private float timeInSameDirection;
    private float _direction;
    private float _currentTime;

    private void Awake()
    {
        _direction = startRight ? 1 : -1;
        _currentTime = 0;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > timeInSameDirection)
        {
            _direction = -_direction;
            _currentTime = 0;
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
}