using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardhightlights : MonoBehaviour
{
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
                    go.transform.position = new Vector3(i+0.5f, 0.05f, j+0.5f);

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
