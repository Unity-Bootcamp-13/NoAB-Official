using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody playerRb;

    internal Vector3 MoveDir;
    private Vector2 _inputVector;
    private float _initMoveSpeed = 6f;

    private void Awake()
    {
        moveSpeed = _initMoveSpeed;
    }

    private void FixedUpdate()
    {
        playerRb.linearVelocity = MoveDir * moveSpeed;
    }


    public void OnMove(InputValue Input)
    {
        _inputVector = Input.Get<Vector2>();

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        // ī�޶� �ٶ󺸴� ������ ���Ϸ� Ʋ�������� �� ������ y���� 0���� ����
        forward.y = 0f;
        right.y = 0f;

        MoveDir = (right * _inputVector.x + forward * _inputVector.y).normalized;
    }
}
