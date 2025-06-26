using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    private Vector3 _startMousePosition;
    private const float ROTATE_SPEED = 5.0f;
    private float _screenCenterPosition;
    private float _rotHorizontal;
    private float _rotVertical;
    private bool _isDragging = false;


    private void Start()
    {
        _screenCenterPosition = Screen.width / 2;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > _screenCenterPosition)
            {
                _startMousePosition = Input.mousePosition;
                _isDragging = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector3 delta = Input.mousePosition - _startMousePosition;
            _startMousePosition = Input.mousePosition;

            _rotHorizontal += delta.x * ROTATE_SPEED * Time.deltaTime;
            _rotVertical += delta.y * ROTATE_SPEED * Time.deltaTime;
            _rotVertical = Mathf.Clamp(_rotVertical, -45, 45);

            _playerTransform.eulerAngles = new Vector3(0, _rotHorizontal, 0);
            this.transform.localEulerAngles = new Vector3(-_rotVertical, 0, 0);
        }
    }
}