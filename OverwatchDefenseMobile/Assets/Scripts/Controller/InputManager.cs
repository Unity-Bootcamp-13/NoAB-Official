using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    internal Vector2 inputVector;

    public void OnMove(InputValue Input)
    {
        inputVector = Input.Get<Vector2>();
    }
}
