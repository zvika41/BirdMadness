using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region --- Const ---

    private static readonly int CloseEyes = Animator.StringToHash("CloseEyes");

    #endregion Const
    
    
    #region --- Serialize Fields ---

    [SerializeField] private float launchForce;
    [SerializeField] private float maxDragDistance;

    #endregion Serialize Fields
    
    
    #region --- Members ---

    private Rigidbody2D _rigidBody;
    private Camera _camera;
    private Vector2 _startPosition;
    private Vector2 _mousePosition;
    private Animator _animator;
    private bool _isDragging;

    #endregion Members


    #region --- Properties ---

    public bool IsDragging => _isDragging;

    #endregion Properties


    #region --- Mono Methods ---

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    private void Start()
    {
        _startPosition = _rigidBody.position;
        SetKinematic(shouldUseKinematic: true);
    }

    #endregion Mono Methods
    

    #region --- On Mouse Click Events Handler ---

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        
        if (_rigidBody.isKinematic)
        {
           SetKinematic(shouldUseKinematic: false);
        }
        
        AddForceToPlayerMovement();
    }

    private void OnMouseDrag()
    {
        HandleMousePosition();
        _animator.SetTrigger(CloseEyes);
    }

    #endregion On Mouse Click Events Handler


    #region --- Private Methods ---

    private void SetKinematic(bool shouldUseKinematic)
    {
        _rigidBody.isKinematic = shouldUseKinematic;
    }

    private void AddForceToPlayerMovement()
    {
        Vector2 direction = GetPlayerDirection();
        _rigidBody.AddForce(direction * launchForce);
    }
    
    private Vector2 GetPlayerDirection()
    {
        Vector2 currentPosition = _rigidBody.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        return direction;
    }

    private void HandleMousePosition()
    {
        _mousePosition = GetMouseCurrentPosition();
        HandleMouseDirection(_mousePosition);

        if (_mousePosition.x > _startPosition.x)
        {
            _mousePosition.x = _startPosition.x;
        }

        _rigidBody.position = _mousePosition;
    }

    private Vector3 GetMouseCurrentPosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private float GetDistance(Vector2 desiredPosition)
    {
        return Vector2.Distance(desiredPosition, _startPosition);
    }

    private void HandleMouseDirection(Vector2 mousePosition)
    {
        float distance = GetDistance(mousePosition);

        if (!(distance > maxDragDistance)) return;
        
        Vector2 direction = mousePosition - _startPosition;
        direction.Normalize();
        _mousePosition = _startPosition + (direction * maxDragDistance);
    }
    
    private IEnumerator ResetPlayerPositionAfterDelay()
    {
        yield return new WaitForSeconds(3);
        
        _rigidBody.position = _startPosition;
        _rigidBody.velocity = Vector2.zero;
    }

    #endregion Private Methods


    #region --- On Collision Events Handler ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetPlayerPositionAfterDelay());
        SetKinematic(shouldUseKinematic: true);
    }

    #endregion On Collision Events Handler
}
