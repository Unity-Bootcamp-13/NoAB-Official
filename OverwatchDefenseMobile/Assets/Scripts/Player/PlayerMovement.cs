using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody _playerRb;

    private Vector3 _moveDir;
    private Vector2 _inputVector;


    private void FixedUpdate()
    {
        _playerRb.linearVelocity = _moveDir * _moveSpeed;
    }


    public void OnMove(InputValue Input)
    {
        _inputVector = Input.Get<Vector2>();

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        // 카메라가 바라보는 방향이 상하로 틀어져있을 수 있으니 y값은 0으로 만듦
        forward.y = 0f;
        right.y = 0f;

        _moveDir = (right * _inputVector.x + forward * _inputVector.y).normalized;
    }
}
