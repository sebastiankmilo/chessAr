using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Sample.Auth;
public class settingplayer : MonoBehaviour
{
    [SerializeField] Text tablero;
    [SerializeField] Toggle blanco;
    [SerializeField] Toggle negro;
    [SerializeField] Text mensaje;
    [SerializeField] Button play;
    public static settingplayer Instances { get; set; }
    //[SerializeField] string color;
    public string Color { get; private set; }
    public string TableroName { get { return tablero.text; }  }
    [SerializeField] bool listo=false;
    public bool Listo { get { return listo; } set { listo = value; } }
    // Start is called before the first frame update
    void Start()
    {
        //token.text = UIHandler.instance.usuario.CurrentUser.UserId;// ProviderId;
        Instances=this;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if(mensaje.text== "Transaction complete. ")
        {
            mensaje.text = "Transaction complete...";
        }
        
        if (!negro.isOn)
        {
            if (!blanco.isOn)
            {
                Color = "";
            }
        }
        if (listo)
        {
            if (!play.gameObject.activeSelf)
            {
                play.gameObject.SetActive(true);
            }
            
        }
        else
        {
            if (play.gameObject.activeSelf)
            {
                play.gameObject.SetActive(false);
            }
        }

        
        
    }
    public void Negro()
    {
        if (negro.isOn)
        {
            Color = "negro";
            blanco.isOn = false;
        }
    }
    public void Blanco() {
        if (blanco.isOn)
        {
            Color = "blanco";
            negro.isOn = false;
        }
    }
    public void Jugar ()
    {

    }
    public void SignOut()
    {
        Debug.Log("saliendo");
        UIHandler.instance.SignOut();
    }
    

}
