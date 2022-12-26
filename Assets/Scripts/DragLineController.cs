using UnityEngine;

public class DragLineController : MonoBehaviour
{
    #region --- Members ---

    private LineRenderer _lineRenderer;
    private PlayerController _playerController;

    #endregion Members


    #region --- Mono Methods ---

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (_playerController.IsDragging)
        {
            if (!_lineRenderer.enabled)
            {
                _lineRenderer.enabled = true;
            }
            
            _lineRenderer.SetPosition(1, _playerController.transform.position);
        }
        else if(!_playerController.IsDragging && _lineRenderer.enabled)
        {
            _lineRenderer.enabled = false;
        }
    }

    #endregion Mono Methods
}
