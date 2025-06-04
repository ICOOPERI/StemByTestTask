using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneReturn : MonoBehaviour
{
    public void returnToScene()
    {
        SceneManager.LoadScene("Main");
    }
}
