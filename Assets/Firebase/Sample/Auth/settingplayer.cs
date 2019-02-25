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
    public static settingplayer Instances { get; set; }
    public string Color { get; private set; }
    public string TableroName { get { return tablero.text; }  }
    // Start is called before the first frame update
    void Start()
    {
        //token.text = UIHandler.instance.usuario.CurrentUser.UserId;// ProviderId;
        Instances=this;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (!negro.isOn)
        {
            if (!blanco.isOn)
            {
                Color = "";
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
    public void CambiarToken()
    {

    }
}
