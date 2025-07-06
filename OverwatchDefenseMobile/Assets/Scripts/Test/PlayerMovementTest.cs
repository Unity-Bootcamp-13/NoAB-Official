using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private CharacterController characterController;
    internal Vector3 MoveDir;
    private Vector2 _inputVector;
    internal float gravity = 1f;
    private float y_velocity = 0;
       
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
                
        Vector3 motion = (MoveDir * moveSpeed + Vector3.down * y_velocity) * Time.deltaTime;
        characterController.Move(motion);
    }

    public void OnMove(InputValue input)
    {
        _inputVector = input.Get<Vector2>();
    }
}