using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarHolder : MonoBehaviour
{
    public static BarHolder Instance { get; private set; }

    [SerializeField] int maxSlots;
    [SerializeField] List<Figure> figuresInBar;

    Transform barParent;
    Vector3 targetPos;
    float gridSpacing;
    float gridCellSize;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        barParent = gameObject.transform;

        RectTransform rect = Instance.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        targetPos = (corners[0] + corners[1]) / 2f;
        targetPos.x += GetComponent<GridLayoutGroup>().padding.left;
        gridCellSize = GetComponent<GridLayoutGroup>().cellSize.x;
        gridSpacing = GetComponent<GridLayoutGroup>().spacing.x;
    }

    public static Vector3 getPositionForChild()
    {
        int count = Instance.figuresInBar.Count;
        float add = 0;
        Vector3 result = Instance.targetPos;
        add += Instance.gridSpacing;
        for (int i = 0; i < count; i++)
        {
            add += Instance.gridCellSize;
            add += Instance.gridSpacing;
        }
        result.x += add;
        return result;
    }

    public void AddFigureToBar(Figure fig)
    {
        fig.transform.SetParent(barParent, false);
        FigureInitializer.removeFromSpawnedFigures(fig.gameObject);
        figuresInBar.Add(fig);

        CheckBarState();
    }

    void CheckBarState()
    {
        for (int i = 0; i < figuresInBar.Count - 2; i++)
        {
            var a = figuresInBar[i];
            for (int j = i + 1; j < figuresInBar.Count - 1; j++)
            {
                var b = figuresInBar[j];
                for (int k = j + 1; k < figuresInBar.Count; k++)
                {
                    var c = figuresInBar[k];
                    if (IsSame(a, b, c))
                    {
                        RemoveTriple(a, b, c);
                    }
                }
            }
        }

        if (figuresInBar.Count >= maxSlots)
        {
            TransferText.text = "Вы проиграли";
            SceneManager.LoadScene("Result");
        }

        if (FigureInitializer.getFiguresCount() == 0)
        {
            TransferText.text = "Вы выиграли";
            SceneManager.LoadScene("Result");
        }
    }

    bool IsSame(Figure a, Figure b, Figure c)
    {
        return a.id == b.id && b.id == c.id;
    }

    void RemoveTriple(Figure a, Figure b, Figure c)
    {
        figuresInBar.Remove(a);
        figuresInBar.Remove(b);
        figuresInBar.Remove(c);

        Destroy(a.gameObject);
        Destroy(b.gameObject);
        Destroy(c.gameObject);
    }
}
