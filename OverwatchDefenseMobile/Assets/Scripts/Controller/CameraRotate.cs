using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _rotateSensitivity;

    private Dictionary<int, Vector2> _lastTouchPos = new Dictionary<int, Vector2>();
    private float _rotateHorizontal;
    private float _rotateVertical;


    private void Update()
    {
        foreach (var touch in Touchscreen.current.touches)
        {
            int id = touch.touchId.ReadValue();
            Vector2 _currentTouchPos = touch.position.ReadValue();

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended ||
                touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                _lastTouchPos.Remove(id);
                continue;
            }

            if (_currentTouchPos.x < Screen.width / 3f)
                continue;

            if (!_lastTouchPos.ContainsKey(id))
            {
                _lastTouchPos[id] = _currentTouchPos;
                continue;
            }

            Vector2 _deltaPos = _currentTouchPos - _lastTouchPos[id];
            _lastTouchPos[id] = _currentTouchPos;

            _rotateHorizontal += _deltaPos.x * _rotateSensitivity;
            _rotateVertical -= _deltaPos.y * _rotateSensitivity;
            _rotateVertical = Mathf.Clamp(_rotateVertical, -40f, 40f);

            _playerTransform.eulerAngles = new Vector3(0f, _rotateHorizontal, 0f);
            transform.localEulerAngles = new Vector3(_rotateVertical, 0f, 0f);
        }
    }
}
