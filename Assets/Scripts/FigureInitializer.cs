using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureInitializer : MonoBehaviour
{
    [SerializeField] int countOfFigures;
    [SerializeField] List<Sprite> bgSprites;
    [SerializeField] List<Sprite> outlinesSprites;
    [SerializeField] List<Sprite> animalsSprites;
    [SerializeField] GameObject figurePrefab;
    [SerializeField] List<GameObject> spawnedFigures;
    [SerializeField] Transform spawnParent;

    int figureMultiple = 3;
    GridLayoutGroup grid;
    static FigureInitializer Instance;

    void Awake()
    {
        Instance = this;    
    }

    void Start()
    {
        grid = GetComponentInChildren<GridLayoutGroup>();
    }

    public void Spawn()
    {
        foreach(GameObject obj in spawnedFigures)
            Destroy(obj);
        spawnedFigures.Clear();

        grid.enabled = true;
        spawnFigures();
        StartCoroutine(ToggleGrid(false));
    }

    IEnumerator ToggleGrid(bool state)
    {
        yield return new WaitForEndOfFrame();
        grid.enabled = false;
    }

    public static int getFiguresCount()
    {
        return Instance.spawnedFigures.Count;
    }

    public static void removeFromSpawnedFigures(GameObject obj)
    {
        Instance.spawnedFigures.Remove(obj);
    }

    void spawnFigures()
    {
        List<Color> colors = GenerateColors(countOfFigures);

        List<FigureData> uniqueFigures = new();

        for (int i = 0; i < countOfFigures; i++)
        {
            int j = UnityEngine.Random.Range(0, bgSprites.Count);

            Sprite bg = bgSprites[j];
            Sprite outline = outlinesSprites[j];

            int a = UnityEngine.Random.Range(0, animalsSprites.Count);
            Sprite animal = animalsSprites[a];

            Color color = colors[i];

            FigureType type = (FigureType)Enum.GetValues(typeof(FigureType)).GetValue(j);

            uniqueFigures.Add(new FigureData(i, type, bg, outline, animal, color));
        }

        List<FigureData> finalList = new();
        for (int i = 0; i < figureMultiple; i++)
            finalList.AddRange(uniqueFigures);

        Shuffle(finalList);

        foreach (var fig in finalList)
        {
            GameObject obj = Instantiate(figurePrefab, spawnParent);
            obj.GetComponent<Figure>().initialize(fig.id, fig.type, fig.bg, fig.outline, fig.animal, fig.color);
            spawnedFigures.Add(obj);
        }
    }

    List<Color> GenerateColors(int count)
    {
        List<Color> list = new();
        for (int i = 0; i < count; i++)
            list.Add(UnityEngine.Random.ColorHSV(0, 1, 0.7f, 1, 0.7f, 1));
        return list;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

}

public struct FigureData
{
    public int id;
    public FigureType type;
    public Sprite bg;
    public Sprite outline;
    public Sprite animal;
    public Color color;    

    public FigureData(int id, FigureType type, Sprite bg, Sprite outline, Sprite animal,  Color color)
    {
        this.id = id;
        this.type = type;
        this.bg = bg;
        this.outline = outline;
        this.animal = animal;
        this.color = color;
    }
}

public enum FigureType
{
    Square,
    Heart,
    Circle    
}

