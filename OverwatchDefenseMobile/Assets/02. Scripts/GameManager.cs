using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal float PlayTime = 150;

    private void Update()
    {
        PlayTime -= Time.deltaTime;
    }
}
