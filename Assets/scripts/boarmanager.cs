using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Sample.Database;

public class boarmanager : MonoBehaviour
{
    [SerializeField] GameObject padre;
    public static boarmanager Instance { set; get; }
    private bool[,] allowedmoves { set; get; }  
    public chessman [,] chessmans { set; get; } //holds information about the positions of figures on the board
    [SerializeField] private chessman selectedchessman; 

    [SerializeField] private const float TILE_SIZE = 1.25f; // tamaño de la valdosa
    [SerializeField] private const float TITLE_OFFSET = 0.625f; // la mitad de la valdosa
    [SerializeField] DatabaseHandler datos;
    public  float Tile_Size() { return TILE_SIZE; }
    public float Tile_Offset() { return TITLE_OFFSET; }
    // que se mestre la valdosa seleccionada
    [SerializeField] private int selectionx = 0;
    [SerializeField] private int selectiony = 0;
    public List<GameObject> chessmanPrefabs;
    [SerializeField] private List<GameObject> activeChessman=new List<GameObject>(); //list of the type GameObject, used to keep track of all characters that are currently alive.
    //private Quaternion oretation = Quatenion.Euler(0,180,0);
    int n = 1;
    int t = 1;
    int k = 1;
    int b = 1;
    public bool iswhiteturn = true;

    #region MONOBEHAVIOUR_METHODS
    private void Start()
    {
        Instance = this;
        SpawnAllChessman();
        Screen.orientation = ScreenOrientation.LandscapeLeft;

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
    #endregion
    #region PUBLIC_METHODS
    public void ok()
    {

        if (selectionx >= 0 && selectiony >= 0)
        {
            if (selectedchessman == null)
            {
                // seleccion de ficha
                Debug.Log("Se seleciono la ficha");
                selectchessman(selectionx, selectiony);
            }
            else
            {
                //move chessman
                movechessman(selectionx, selectiony);
                Debug.Log("Se Movio la ficha");
            }
        }
    }
    public void up()
    {
        if (selectiony < 7)
        {
            selectiony = selectiony + 1;
            resalto.Instance.hideseleccion();
        }
    }
    public void down()
    {
        if (selectiony > 0)
        {
            selectiony = selectiony - 1;
            resalto.Instance.hideseleccion();
        }
    }
    public void left()
    {
        if (selectionx > 0)
        {
            selectionx = selectionx - 1;
            resalto.Instance.hideseleccion();
        }
    }
    public void right()
    {
        if (selectionx < 7)
        {
            selectionx = selectionx + 1;
            resalto.Instance.hideseleccion();
        }
    }

    #endregion
    #region PRIVATE_METHODS
    public void selectchessman(int x,int y)
    {
        if (chessmans[x, y] == null)
        {
            Debug.Log("Chessmans es nulo");
            return;
        }
        if (chessmans[x, y].iswhite != iswhiteturn)
        {
            Debug.Log("No es el turno de la ficha");
            return;
        }
        if (chessmans[x, y].iswhite != jugadores.player.remoto) //remoto false, el jugador local es blanco
        {

            bool hasatleastonemove = false;
            allowedmoves = chessmans[x, y].possiblemove();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (allowedmoves[i, j])
                        hasatleastonemove = true;
            selectedchessman = chessmans[x, y];
            boardhightlights.Instance.highlightallowedmoves(allowedmoves);
            Debug.Log("Se ha elegido una ficha");
        }



    }
    
    public void movechessman(int x, int y)
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
            datos.move(selectedchessman.Currentx, selectedchessman.Currenty,x,y);
            chessmans[selectedchessman.Currentx, selectedchessman.Currenty] = null;
            selectedchessman.transform.localPosition = GetTileCenter(x, y);
            selectedchessman.setposition(x, y);
            chessmans[x, y] = selectedchessman;
            iswhiteturn = !iswhiteturn;
        }

        boardhightlights.Instance.hidehighlights();
        selectedchessman = null;
    }
    
    private void UpdateSelection()
    {
        //arriba
        if (Input.GetKeyDown(KeyCode.UpArrow)) { up(); }

        //abajo
        if (Input.GetKeyDown(KeyCode.DownArrow)) { down(); }
        //derecha
        if (Input.GetKeyDown(KeyCode.RightArrow)) { right(); }
        //left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {left();}
    }
    private void SpawnChesman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        if (go.name.Contains("pawn"))
        {
            go.name = go.name + n.ToString();
            n++;
            if (n == 9)
            {
                n = 1;
            }
        }
        if (go.name.Contains("rook"))
        {
            go.name = go.name + t.ToString();
            t++;
            if (t == 3)
            {
                t = 1;
            }
        }
        if (go.name.Contains("bishop"))
        {
            go.name = go.name + b.ToString();
            b++;
            if (b == 3)
            {
                b = 1;
            }
        }
        if (go.name.Contains("knight"))
        {
            go.name = go.name + k.ToString();
            k++;
            if (k == 3)
            {
                k = 1;
            }
        }
        go.transform.SetParent(padre.transform);
        go.transform.localPosition = GetTileCenter(x, y);
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
        origin.x += ((TILE_SIZE * x) + TITLE_OFFSET)*1f;
        origin.z += ((TILE_SIZE * y) + TITLE_OFFSET)*1f;
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
    #endregion

}
