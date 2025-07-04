using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    private CharacterController _controller;
    internal Vector3 MoveDir;
    private Vector2 _inputVector;
    internal float gravity = 20f;
    private float y_velocity = 0;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        y_velocity += gravity * Time.deltaTime;


        // 입력에 따라 방향 계산
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        MoveDir = (right * _inputVector.x + forward * _inputVector.y).normalized;

        // CharacterController.Move 로 이동
        // 속도 * deltaTime를 곱해 프레임 독립적 처리
        Vector3 motion = MoveDir * moveSpeed * Time.deltaTime;
        _controller.Move(motion);
        _controller.Move(Vector3.down * y_velocity * Time.deltaTime);
    }

    public void OnMove(InputValue input)
    {
        _inputVector = input.Get<Vector2>();
    }
}