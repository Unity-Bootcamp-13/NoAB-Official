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
        // PC�� ������
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
            // ��ƽ ��ġ�� �е� ���η� ����
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

            // ��ƽ ��ġ�� ���� �÷��̾� �̵�
            Vector3 stickDeltaPos = _stick.position - _stickStartPos;

            if (_player != null)
            {
                _player.OnStickChanged(stickDeltaPos);
            }
        }
        else // ��ġ�е� ��ư ���� �÷��̾� ����
        {            
            _player.OnStickChanged(Vector2.zero);
        }
    }


    private void HandleTouchInputMobile()
    {
        // ����� ��ġ �����ؾ���



    }
}