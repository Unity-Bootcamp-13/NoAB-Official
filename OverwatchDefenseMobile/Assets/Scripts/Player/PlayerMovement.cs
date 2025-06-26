using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private const int PLAYER_SPEED = 10;

    private float _inputHorizontal;
    private float _inputVertical;
    public void OnStickChanged(Vector2 stickPos)
    {
        _inputHorizontal = stickPos.x;
        _inputVertical = stickPos.y;
    }

    private void Update()
    {
        Rigidbody playerRigidbody = GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            // ī�޶� �ٶ󺸴� ������ ���Ϸ� Ʋ�������� �� ������ y���� 0���� ����
            forward.y = 0f;
            right.y = 0f;

            Vector3 moveDir = (right * _inputHorizontal + forward * _inputVertical).normalized;
            playerRigidbody.MovePosition(transform.position + moveDir * PLAYER_SPEED * Time.deltaTime);
        }
    }
}
