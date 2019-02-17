using System.Collections.Generic;
using UnityEngine;

public class resalto : MonoBehaviour
{
    [SerializeField] GameObject padre;
    public static resalto Instance { set; get; }
    public GameObject highlightsprefab;
    [SerializeField] private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }
    private GameObject GetHighlightsObjets()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);
        try
        {
            
            go = highlights[0];
        }
        catch (System.Exception)
        {
            go = Instantiate(highlightsprefab);
            highlights.Add(go);
            throw;
        }
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
        go.transform.SetParent(padre.transform);
        go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        go.transform.localPosition = new Vector3(x* boarmanager.Instance.Tile_Size() + boarmanager.Instance.Tile_Offset(), 0.05f, y*boarmanager.Instance.Tile_Size() + boarmanager.Instance.Tile_Offset());

    }
    public void hideseleccion()
    {
        try
        {
            foreach (GameObject go in highlights)
                go.SetActive(false);
        }
        catch (System.Exception)
        {

            Debug.Log("paso algo xD");
        }
       
    }

}
