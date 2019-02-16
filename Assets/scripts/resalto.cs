using System.Collections.Generic;
using UnityEngine;

public class resalto : MonoBehaviour
{
    public static resalto Instance { set; get; }
    public GameObject highlightsprefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }
    private GameObject GetHighlightsObjets()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);
        if (go == null)
        {
            go = Instantiate(highlightsprefab);
            highlights.Add(go);

        }
        return go;
    }
    public void seleccion(int x, int y)
    {
        GameObject go = GetHighlightsObjets();
        go.SetActive(true);
        go.transform.position = new Vector3(x + 0.5f, 0.05f, y + 0.5f);

    }
    public void hideseleccion()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }

}
