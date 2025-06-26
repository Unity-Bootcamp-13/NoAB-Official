using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private const int playerSpeed = 10;

    private float inputHorizontal, inputVertical;
    public void OnStickChanged(Vector2 stickPos)
    {
        inputHorizontal = stickPos.x;
        inputVertical = stickPos.y;
    }

    private void Update()
    {
        Rigidbody playerRigidbody = GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            // 카메라가 바라보는 방향이 상하로 틀어져있을 수 있으니 y값은 0으로 만듦
            forward.y = 0f;
            right.y = 0f;

            Vector3 moveDir = (right * inputHorizontal + forward * inputVertical).normalized;
            playerRigidbody.MovePosition(transform.position + moveDir * playerSpeed * Time.deltaTime);
        }
    }
}
