using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using Firebase.Sample.Database;

public class jugadores : MonoBehaviour
{
    public static jugadores player;
    [SerializeField] InputField jugador;
    [SerializeField] GameObject botonNegro;
    [SerializeField] GameObject botonBlanco;
    [SerializeField] DatabaseHandler datos;
    bool movimiento=false;
    int turno = 0;
    public bool remoto { get; set; }
    [SerializeField] bool Remoto;
    public Dictionary<string, Vector2> blanco = new Dictionary<string, Vector2>
            {
                {"white king(Clone)",new Vector2(3,0)},
                {"white queen(Clone)",new Vector2(4,0)},
                {"white rook(Clobe)1",new Vector2(0,0)},
                {"white bishop(Clone)1",new Vector2(2,0)},
                {"white knight(clone)1",new Vector2(1,0)},
                {"white rook(Clobe)2",new Vector2(7,0)},
                {"white bishop(Clone)2",new Vector2(5,0)},
                {"white knight(clone)2",new Vector2(6,0)},
                {"white pawn(Clone)1",new Vector2(0,1)},
                {"white pawn(Clone)2",new Vector2(1,1)},
                {"white pawn(Clone)3",new Vector2(2,1)},
                 {"white pawn(Clone)4",new Vector2(3,1)},
                {"white pawn(Clone)5",new Vector2(4,1)},
                {"white pawn(Clone)6",new Vector2(5,1)},
                {"white pawn(Clone)7",new Vector2(6,1)},
                 {"white pawn(Clone)8",new Vector2(7,1)}
            };
    public Dictionary<string, Vector2> negro = new Dictionary<string, Vector2>
            {
                {"black king(Clone)",new Vector2(3,0)},
                {"black queen(Clone)",new Vector2(4,0)},
                {"black rook(Clobe)1",new Vector2(0,0)},
                {"black bishop(Clone)1",new Vector2(2,0)},
                {"black knight(clone)1",new Vector2(1,0)},
                {"black rook(Clobe)2",new Vector2(7,0)},
                {"black bishop(Clone)2",new Vector2(5,0)},
                {"black knight(clone)2",new Vector2(6,0)},
                {"blackpawn(Clone)1",new Vector2(0,1)},
                {"blackpawn(Clone)2",new Vector2(1,1)},
                {"blackpawn(Clone)3",new Vector2(2,1)},
                 {"blackpawn(Clone)4",new Vector2(3,1)},
                {"blackpawn(Clone)5",new Vector2(4,1)},
                {"blackpawn(Clone)6",new Vector2(5,1)},
                {"blackpawn(Clone)7",new Vector2(6,1)},
                 {"blackpawn(Clone)8",new Vector2(7,1)}
    }; 


    public void actualizar (string nombre,bool esblanco, Vector2 posicion)
    {

        if (esblanco == true)
        {
            blanco[nombre]=posicion;
            Debug.Log(blanco[nombre]);
        }
        else
        {
            negro[nombre] = posicion;
        }
    }
    public void enviar(Vector2 seleccion,Vector2 movimiento)
    {
        Debug.Log("seleccion "+seleccion+ "movimiento"+movimiento);
    }
    void Awake()
    {
        player = this;
    }
    private void Start()
    {
        datos.StartListener2(settingplayer.Instances.TableroName);
    }
    void Update()
    {
        if (settingplayer.Instances.Color == "negro")
        {
            remoto = true;
            Remoto = remoto;
            botonNegro.SetActive(true);
            botonBlanco.SetActive(false);

        }
        else if(settingplayer.Instances.Color == "blanco")
        {
            remoto = false;
            Remoto = remoto;
            botonBlanco.SetActive(true);
            botonNegro.SetActive(false);
        }
        else
        {
            botonBlanco.SetActive(false);
            botonNegro.SetActive(false);
        }
    }


}
