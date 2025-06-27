using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private InputManager _inputManager;
    private Rigidbody _playerRb;
    private Vector3 _moveDir;


    private void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _playerRb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (_playerRb != null)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            // ī�޶� �ٶ󺸴� ������ ���Ϸ� Ʋ�������� �� ������ y���� 0���� ����
            forward.y = 0f;
            right.y = 0f;

            _moveDir = (right * _inputManager.inputVector.x + forward * _inputManager.inputVector.y).normalized;
            _playerRb.MovePosition(transform.position + _moveDir * _moveSpeed * Time.deltaTime);
        }
    }
}
