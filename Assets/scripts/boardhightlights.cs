using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardhightlights : MonoBehaviour
{
    [SerializeField] GameObject padre;
    public static boardhightlights Instance { set; get; }
    public GameObject highlightsprefab;
    private List<GameObject> highlights;
    private void Start()
    {
        Instance= this;
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
    public void highlightallowedmoves(bool[,]moves)
    {
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighlightsObjets();
                    go.SetActive(true);                    
                    go.transform.SetParent(padre.transform);
                    go.transform.localRotation= Quaternion.Euler(0, 0, 0);
                    go.transform.localPosition = new Vector3(i * boarmanager.Instance.Tile_Size() + boarmanager.Instance.Tile_Offset(), 0.05f, j * boarmanager.Instance.Tile_Size() + boarmanager.Instance.Tile_Offset());

                }
            }
        }
    }
    public void hidehighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
    }   
