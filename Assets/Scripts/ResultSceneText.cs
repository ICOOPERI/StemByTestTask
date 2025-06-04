using UnityEngine;
using UnityEngine.UI;

public class ResultSceneText : MonoBehaviour
{
    [SerializeField] Text Text;

    void Start()
    {
        Text.text = TransferText.text;
    }
}
