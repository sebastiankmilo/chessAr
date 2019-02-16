using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boarmanager : MonoBehaviour
{
    [SerializeField] GameObject padre;
    public static boarmanager Instance { set; get; }
    private bool[,] allowedmoves { set; get; }  
    public chessman [,] chessmans { set; get; }
    private chessman selectedchessman;

    private const float TILE_SIZE = 1.0f; // tamaño de la valdosa
    private const float TITLE_OFFSET = 0.5f; // la mitad de la valdosa
    // que se mestre la valdosa seleccionada
    private int selectionx = 0;
    private int selectiony = 0;
    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman=new List<GameObject>();
    //private Quaternion oretation = Quatenion.Euler(0,180,0);

    public bool iswhiteturn = true;
    private void Start()
    {
        Instance = this;
        SpawnAllChessman();

    }
    private void Update()
    {
        UpdateSelection();
        Drawchessboard();
        resalto.Instance.seleccion(selectionx, selectiony);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ok();
        }

    }
    public void ok()
    {

        if (selectionx >= 0 && selectiony >= 0)
        {
            if (selectedchessman == null)
            {
                // seleccion de ficha
                selectchessman(selectionx, selectiony);
            }
            else
            {
                //move chessman
                //movechessman(selectionx, selectiony);
            }
        }
    }
    private void selectchessman(int x,int y)
    {
        if (chessmans[x, y] == null)
            return;
        if (chessmans[x, y].iswhite != iswhiteturn)
            return;
        bool hasatleastonemove = false;
        allowedmoves = chessmans[x, y].possiblemove();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedmoves[i, j])
                    hasatleastonemove = true;
        selectedchessman = chessmans[x, y];
        boardhightlights.Instance.highlightallowedmoves(allowedmoves);


    }
    /*
    private void movechessman(int x, int y)
    {
        if (allowedmoves[x, y])
        {
            chessman c = chessmans[x, y];

            if (c != null && c.iswhite != iswhiteturn)
            {
                // capture pieza
                // si es el rey
                if (c.GetType() == typeof(king))
                {
                    EndGame();
                    return;
                }
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            chessmans[selectedchessman.Currentx, selectedchessman.Currenty] = null;
            selectedchessman.transform.position = GetTileCenter(x, y);
            selectedchessman.setposition(x, y);
            chessmans[x, y] = selectedchessman;
            iswhiteturn = !iswhiteturn;
        }

        boardhightlights.Instance.hidehighlights();
        selectedchessman = null;
    }
    */
    private void UpdateSelection()
    {
        //arriba
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectiony < 7)
            {
                selectiony = selectiony + 1;
                resalto.Instance.hideseleccion();
            }
                
        }

        //abajo
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectiony > 0)
            {
                selectiony = selectiony - 1;
                resalto.Instance.hideseleccion();
            }
               
        }
        //derecha
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectionx < 7)
            {
                selectionx = selectionx + 1;
                resalto.Instance.hideseleccion();
            }

                
        }
        //left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectionx > 0)
            {
                selectionx = selectionx - 1;
                resalto.Instance.hideseleccion();
            }
                
        }
    }
    private void SpawnChesman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y), Quaternion.identity) as GameObject;

        go.transform.SetParent(transform);
        chessmans[x, y] = go.GetComponent<chessman>();
        chessmans[x, y].setposition(x,y);
        activeChessman.Add(go);
    }
    private void SpawnAllChessman()
    {
        activeChessman = new List<GameObject>();
        chessmans = new chessman[8, 8];
        // fichas blancas

        //king
        SpawnChesman(0, 3, 0);
        //queen
        SpawnChesman(1, 4, 0);
        //rooks
        SpawnChesman(2, 0, 0);
        SpawnChesman(2, 7, 0);
        //bishops
        SpawnChesman(3, 2, 0);
        SpawnChesman(3, 5, 0);
        //knights
        SpawnChesman(4, 1, 0);
        SpawnChesman(4, 6, 0);
        //pawns
        for (int i = 0; i < 8; i++)
            SpawnChesman(5, i, 1);

        // fichas negras

        //king
        SpawnChesman(6, 4, 7);
        //queen
        SpawnChesman(7, 3, 7);
        //rooks
        SpawnChesman(8, 0, 7);
        SpawnChesman(8, 7, 7);
        //bishops
        SpawnChesman(9, 2, 7);
        SpawnChesman(9, 5, 7);
        //knights
        SpawnChesman(10, 1, 7);
        SpawnChesman(10, 6, 7);
        //pawns
        for (int i = 0; i < 8; i++)
            SpawnChesman(11, i, 6);
    }
    private Vector3 GetTileCenter(int x,int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TITLE_OFFSET;
        origin.z += (TILE_SIZE * y) + TITLE_OFFSET;
        return origin;
    }
    private void Drawchessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heigthLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            //lineas verticales
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                //lineas horizontales
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heigthLine);
            }
        }
        //dibujo la seleccion
        if(selectionx>=0 && selectiony >= 0)
        {

            Debug.DrawLine(
                Vector3.forward * selectiony + Vector3.right * selectionx,
                Vector3.forward * (selectiony + 1) + Vector3.right * (selectionx + 1));
            Debug.DrawLine(
                Vector3.forward * (selectiony+1) + Vector3.right * selectionx,
                Vector3.forward * selectiony + Vector3.right * (selectionx + 1));
        }
    }
    private void EndGame()
    {
        if (iswhiteturn)
            Debug.Log("gana las fichas blancas");
        else
            Debug.Log("gana las fichas negras");
        foreach (GameObject go in activeChessman)
            Destroy(go);
        iswhiteturn = true;
        boardhightlights.Instance.hidehighlights();
        SpawnAllChessman();
    }

}
