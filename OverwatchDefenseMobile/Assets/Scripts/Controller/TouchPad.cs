using System;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    [SerializeField] private RectTransform _stick;
    [SerializeField] private RectTransform _touchPadBackground;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private const float MAX_DRAG_RADIUS = 120.0f;

    private Vector3 _touchPadInitialPos;
    private Vector3 _stickStartPos;
    private bool _isButtonPressed;


    private void Start()
    {
        _stickStartPos = _stick.position;
        _touchPadInitialPos = _touchPadBackground.position;
        _isButtonPressed = false;
    }


    private void Update()
    {               
        // PC나 에디터
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE
        HandleTouchInput(Input.mousePosition);
#else
        HandleTouchInputMobile();
#endif
    }


    public void ButtonDown()
    {
        _touchPadBackground.position = Input.mousePosition;
        _stickStartPos = Input.mousePosition;
        _isButtonPressed = true;
    }


    public void ButtonUp()
    {
        _isButtonPressed = false;
        _touchPadBackground.position = _touchPadInitialPos;
        _stick.anchoredPosition = Vector2.zero;
    }


    private void HandleTouchInput(Vector3 mousePosition)
    {
        if (_isButtonPressed)
        {
            // 스틱 위치를 패드 내부로 제한
            Vector3 fingerDeltaPos = mousePosition - _stickStartPos;

            if (fingerDeltaPos.sqrMagnitude > MAX_DRAG_RADIUS * MAX_DRAG_RADIUS)
            {
                fingerDeltaPos.Normalize();

                _stick.position = _stickStartPos + fingerDeltaPos * MAX_DRAG_RADIUS;
            }
            else
            {
                _stick.position = mousePosition;
            }

            // 스틱 위치에 따라 플레이어 이동
            Vector3 stickDeltaPos = _stick.position - _stickStartPos;

            if (_player != null)
            {
                _player.OnStickChanged(stickDeltaPos);
            }
        }
        else // 터치패드 버튼 떼면 플레이어 정지
        {            
            _player.OnStickChanged(Vector2.zero);
        }
    }


    private void HandleTouchInputMobile()
    {
        // 모바일 터치 구현해야함



    }
}