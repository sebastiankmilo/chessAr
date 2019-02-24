using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Sample.Auth;
public class settingplayer : MonoBehaviour
{
    [SerializeField] Text token;
    [SerializeField] InputField blanco;
    [SerializeField] InputField negro;
    string white { get; set; }
    string Black { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        //token.text = UIHandler.instance.usuario.CurrentUser.UserId;// ProviderId;
    }

    // Update is called once per frame
    void OnGUI()
    {
        white = blanco.text;
        Black = negro.text;
    }
    public void Jugar ()
    {

    }
    public void CambiarToken()
    {

    }
}
