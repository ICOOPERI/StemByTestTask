using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] int targetFrameRate = 60;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
