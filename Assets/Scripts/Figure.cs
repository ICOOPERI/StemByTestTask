using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Figure : MonoBehaviour
{
    public int id { get; private set; }

    public FigureType currentType { get; private set; }

    [Space]
    [Header("Parameters")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Image outlineImage;
    [SerializeField] Image animalImage;
    [SerializeField] Button button;
    [SerializeField] GameObject squareCollider;
    [SerializeField] GameObject circleCollider;
    [SerializeField] GameObject heartCollider;

    public void initialize(int id, FigureType type, Sprite bg, Sprite outline, Sprite animal, Color color)
    {
        this.id = id;
        squareCollider.SetActive(false);
        circleCollider.SetActive(false);
        heartCollider.SetActive(false);

        currentType = type;
        backgroundImage.sprite = bg;
        outlineImage.sprite = outline;
        animalImage.sprite = animal;

        switch (currentType)
        {
            case FigureType.Square:
                squareCollider.SetActive(true);
                break;
            case FigureType.Circle:
                circleCollider.SetActive(true);
                break;
            case FigureType.Heart:
                heartCollider.SetActive(true);
                break;
        }        

        outlineImage.color = color;
    }

    public void addToTake()
    {
        StartCoroutine(MoveToBarAndAdd());
    }

    IEnumerator MoveToBarAndAdd()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = BarHolder.getPositionForChild();

        float duration = 0.5f;
        float elapsed = 0f;

        var rb = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        squareCollider.SetActive(false);
        circleCollider.SetActive(false);
        heartCollider.SetActive(false);
        transform.rotation = Quaternion.identity;

        button.onClick.RemoveAllListeners();

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        BarHolder.Instance.AddFigureToBar(this);
    }
}
