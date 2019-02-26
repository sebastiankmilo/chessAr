// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Firebase.Sample.Database {
    using Firebase;
    using Firebase.Database;
    using Firebase.Unity.Editor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Firebase.Sample.Auth;

    // Handler for UI buttons on the scene.  Also performs some
    // necessary setup (initializing the firebase app, etc) on
    // startup.
    public class DatabaseHandler : MonoBehaviour {

        [SerializeField] Text blanco;
        [SerializeField] Text negro;
        [SerializeField] Text Tablero;
        [SerializeField] Text debuglog;
        ArrayList leaderBoard = new ArrayList();
     

        public GUISkin fb_GUISkin;

        private const int MaxScores = 10;
        private string logText = "";
        private string email = "";
        private int score = 100;
        [SerializeField] private string playerName = "";
        private string color = "";
        private string tableroNAme = "";
        protected bool UIEnabled = true;

        const int kMaxLogSize = 16382;
        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

        // When the app starts, check to make sure that we have
        // the required dependencies to use Firebase, and if not,
        // add them if possible.
        protected virtual void Start() {
            leaderBoard.Clear();
            leaderBoard.Add("Firebase Top " + MaxScores.ToString() + " Scores");

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                InitializeFirebase();
            } else {
                Debug.LogError(
                "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
            });
        }

        // Initialize the Firebase database:
        protected virtual void InitializeFirebase() {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            // NOTE: You'll need to replace this url with your Firebase App's database
            // path in order for the database connection to work correctly in editor.
            app.SetEditorDatabaseUrl("https://ajedrez-23.firebaseio.com/");
            if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
            StartListener();
        }

        protected void StartListener() {
            FirebaseDatabase.DefaultInstance
            .GetReference("Leaders").OrderByChild("score")
            .ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
                if (e2.DatabaseError != null) {
                Debug.LogError(e2.DatabaseError.Message);
                return;
                }
                Debug.Log("Received values for Leaders.");
                string title = leaderBoard[0].ToString();
                leaderBoard.Clear();
                leaderBoard.Add(title);
                if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
                foreach (var childSnapshot in e2.Snapshot.Children) {
                    if (childSnapshot.Child("score") == null
                    || childSnapshot.Child("score").Value == null) {
                    Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                    break;
                    } else {
                    Debug.Log("Leaders entry : " +
                    childSnapshot.Child("email").Value.ToString() + " - " +
                    childSnapshot.Child("score").Value.ToString());
                    leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
                    + "  " + childSnapshot.Child("email").Value.ToString());

                    }
                }
                }
            };
        }

        // Exit if escape (or back, on mobile) is pressed.
        protected virtual void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            }
        }

        // Output text to the debug log text field, as well as the console.
        public void DebugLog(string s) {
            Debug.Log(s);
            debuglog.text = s;
            logText = s;// + "\n";

            /*while (logText.Length > kMaxLogSize) {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
                
            }*/
        }
        public void DebugLog(string s,bool mensaje)
        {
            Debug.Log(s);
            if (mensaje) { debuglog.text = s; }
            logText = s;// + "\n";

            /*while (logText.Length > kMaxLogSize) {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
                
            }*/
        }


        // A realtime database transaction receives MutableData which can be modified
        // and returns a TransactionResult which is either TransactionResult.Success(data) with
        // modified data or TransactionResult.Abort() which stops the transaction with no changes.
        TransactionResult AddScoreTransaction(MutableData mutableData) {
            List<object> leaders = mutableData.Value as List<object>;

            if (leaders == null) {
            leaders = new List<object>();
            } else if (mutableData.ChildrenCount >= MaxScores) {
            // If the current list of scores is greater or equal to our maximum allowed number,
            // we see if the new score should be added and remove the lowest existing score.
            long minScore = long.MaxValue;
            object minVal = null;
            foreach (var child in leaders) {
                if (!(child is Dictionary<string, object>))
                continue;
                long childScore = (long)((Dictionary<string, object>)child)["score"];
                if (childScore < minScore) {
                minScore = childScore;
                minVal = child;
                }
            }
            // If the new score is lower than the current minimum, we abort.
            if (minScore > score) {
                return TransactionResult.Abort();
            }
            // Otherwise, we remove the current lowest to be replaced with the new score.
            leaders.Remove(minVal);
            }

            // Now we add the new score as a new entry that contains the email address and score.
            Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
            newScoreMap["score"] = score;
            newScoreMap["email"] = email;
            leaders.Add(newScoreMap);

            // You must set the Value to indicate data at that location has changed.
            mutableData.Value = leaders;
            return TransactionResult.Success(mutableData);
        }
        TransactionResult ReadPlayerNameTransaction(MutableData mutableData)
        {
           
            return TransactionResult.Success(mutableData);
        }

        
        



        public void AddScore() {
            score = 20;//.Parse(negro.text);
            email = "bartolo";//blanco.text;
            if (score == 0 || string.IsNullOrEmpty(email)) {
            DebugLog("invalid score or email.");
            return;
            }
            DebugLog(String.Format("Attempting to add score {0} {1}",
            email, score.ToString()));

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sebastian");

            DebugLog("Running Transaction...");
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.
            reference.RunTransaction(AddPlayerTransaction)
            .ContinueWith(task => {
                if (task.Exception != null) {
                DebugLog(task.Exception.ToString());
                } else if (task.IsCompleted) {
                DebugLog("Transaction complete.");
                }
            });
        }
        public void AddPlayer()
        {
            playerName = UIHandler.instance.usuario.CurrentUser.UserId;
            color = settingplayer.Instances.Color;
            tableroNAme = settingplayer.Instances.TableroName;
            if (string.IsNullOrEmpty(playerName))
            {
                DebugLog("nombre de jugador invalido");
                return;
            }
            else if (string.IsNullOrEmpty(color))
            {
                DebugLog("no ha elejido un color");
                return;
            }
            else if (string.IsNullOrEmpty(tableroNAme))
            {
                DebugLog("nombre de tablero invalido");
                return;
            }
            DebugLog(String.Format("Attempting to add a el jugador {0} que ha elegido las fichas {1} a el tablero de juego: {2}",
            playerName, color, tableroNAme),false);

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(playerName);

            DebugLog("Running Transaction...",false);
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.
            reference.RunTransaction(AddPlayerTransaction)
            .ContinueWith(task => {
                if (task.Exception != null)
                {
                    DebugLog(task.Exception.ToString()+" el error se presento en Addplayer");
                }
                else if (task.IsCompleted)
                {
                    //settingplayer.Instances.Listo = true;
                    //DebugLog("Transaction complete. ",true);
                    AddTableroDeJuego();
                    
                    
                }
            });
        }
        TransactionResult AddPlayerTransaction(MutableData mutableData)
        {
            Dictionary<string, object> leaders = mutableData.Value as Dictionary<string, object>;

            if (leaders == null)
            {
                leaders = new Dictionary<string, object>(); ;
            }
            // Now we add the new score as a new entry that contains the email address and score.
            Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
            newScoreMap["color"] = color;
            newScoreMap["NombreTablero"] = tableroNAme;
            leaders = newScoreMap;

            // You must set the Value to indicate data at that location has changed.
            mutableData.Value = leaders;
            return TransactionResult.Success(mutableData);
        }
        public void AddTableroDeJuego()
        {
            tableroNAme = settingplayer.Instances.TableroName;
            if (string.IsNullOrEmpty(tableroNAme))
            {
                DebugLog("nombre de tablero invalido");
                return;
            }
            DebugLog(String.Format("Creando el tablero de juego: {0}", tableroNAme), false);

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(tableroNAme);

            DebugLog("Running Transaction...", false);
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.
            reference.RunTransaction(AddTableroTransaction)
            .ContinueWith(task => {
                if (task.Exception != null)
                {
                    Debug.Log(task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    settingplayer.Instances.Listo = true;
                    Debug.Log("el jugador blanco es: "+task.Result.Child("blanco").Value.ToString());

                }
            });
        }
        TransactionResult AddTableroTransaction(MutableData mutableData)
        {
            Dictionary<string, object> leaders = mutableData.Value as Dictionary<string, object>;
            string _color = color;
            string _playerName;
            //inicializo un tablero de juegos, si no existe;
            if (leaders == null)
            {
                leaders = new Dictionary<string, object>();
                leaders["blanco"] = "";
                leaders["negro"] = "";
                leaders["posxOld"] = 0;
                leaders["posyOld"] = 0;
                leaders["posxNew"] = 0;
                leaders["posyNew"] = 0;

            }

            /*DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("bartolo");
            IDebugLog("Running Transaction...");
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.
            reference.RunTransaction(ReadPlayerNameTransaction)
            .ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    Debug.Log(/*task.Exception.ToString()+///"El error se presento cuando voy a leer los datos del jugador");
                    _color = "";
                }
                else if (task.IsCompleted) //accedo a leer los datos del jugador en la nube
                {
                    Debug.Log("Transaction complete. " + "leer datos del jugador");
                    _color = task.Result.Child("color").Value.ToString();//leo el color que tiene asignado este usuario en la nube
                    Debug.Log("color: " + _color + " " + string.IsNullOrEmpty(_color).ToString());
                }
            });
            reference.GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    _color = task.Result.Child("color").Value.ToString();
                    // Do something with snapshot...
                }
            });*/
            Debug.Log("busqueda: " +leaders[color]+ "blanco" + _color);
           if (string.IsNullOrEmpty(leaders[_color].ToString()))//accedo a el valor de color de este usuario y verifico si este color no esta ocupado en el tablero de juego
            {
                if ((string)leaders[Otrocolor(_color)] != playerName) //rectifico que la sala, el otro color no esta ocupado por este jugador
                {
                    leaders[_color] = UIHandler.instance.usuario.CurrentUser.UserId;
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Configurado Sala de Juego"));
                    Debug.Log(_color + " : " + leaders[_color]);



                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("El usuario ya tiene asignado el color " + Otrocolor(_color)+" en la sala de juego "+tableroNAme+" así que se procede a cambiar de color"));
                    leaders[Otrocolor(_color)] = "";
                    leaders[_color] = playerName;
                    //return TransactionResult.Abort();
                }
            }
            else
            {
                if (leaders[_color].ToString()==playerName)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Ud ya eligio el color: "+_color+" en esta sala de juego"));
                }
                else if ((string)leaders[Otrocolor(_color)] == playerName)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Ud ya eligio el color: " + Otrocolor(_color)));
                }
                else if (string.IsNullOrEmpty((string)leaders[Otrocolor(_color)]))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Esta sala solo tiene libre el color " + Otrocolor(_color)));
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("no hay espacio para más jugadores"+" En la sala "+tableroNAme));
                    return TransactionResult.Abort();
                }
                

            }
            // You must set the Value to indicate data at that location has changed.
            mutableData.Value = leaders;
            return TransactionResult.Success(mutableData);
        }

        public void playGame()
        {
            tableroNAme = settingplayer.Instances.TableroName;
            if (string.IsNullOrEmpty(tableroNAme))
            {
                DebugLog("nombre de tablero invalido");
                return;
            }
            DebugLog(String.Format("Creando el tablero de juego: {0}", tableroNAme), false);

            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(tableroNAme);

            DebugLog("Running Transaction...", false);
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.
            reference.RunTransaction(playGameTransaction)
            .ContinueWith(task => {
                if (task.Exception != null)
                {
                    Debug.Log(task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    settingplayer.Instances.Listo = true;
                    Debug.Log("el jugador blanco es: " + task.Result.Child("blanco").Value.ToString());

                }
            });
        }
        TransactionResult playGameTransaction(MutableData mutableData)
        {
            Dictionary<string, object> leaders = mutableData.Value as Dictionary<string, object>;
            string _color = color;
            string _playerName;
            //inicializo un tablero de juegos, si no existe;
            if (leaders == null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("no se ha creado la sala de juego " + tableroNAme));
                return TransactionResult.Abort();

            }

            
            Debug.Log("busqueda: " + leaders[color] + "blanco" + _color);
            if (string.IsNullOrEmpty(leaders[_color].ToString()))//accedo a el valor de color de este usuario y verifico si este color no esta ocupado en el tablero de juego
            {
                if ((string)leaders[Otrocolor(_color)] != playerName) //rectifico que la sala, el otro color no esta ocupado por este jugador
                {
                    leaders[_color] = UIHandler.instance.usuario.CurrentUser.UserId;
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Configurado Sala de Juego"));
                    Debug.Log(_color + " : " + leaders[_color]);



                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("El usuario ya tiene asignado el color " + Otrocolor(_color) + " en la sala de juego " + tableroNAme + " así que se procede a cambiar de color"));
                    leaders[Otrocolor(_color)] = "";
                    leaders[_color] = playerName;
                    //return TransactionResult.Abort();
                }
            }
            else
            {
                if (leaders[_color].ToString() == playerName)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Ud ya eligio el color: " + _color + " en esta sala de juego"));
                }
                else if ((string)leaders[Otrocolor(_color)] == playerName)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Ud ya eligio el color: " + Otrocolor(_color)));
                }
                else if (string.IsNullOrEmpty((string)leaders[Otrocolor(_color)]))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("Esta sala solo tiene libre el color " + Otrocolor(_color)));
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(IDebugLog("no hay espacio para más jugadores" + " En la sala " + tableroNAme));
                    return TransactionResult.Abort();
                }


            }
            // You must set the Value to indicate data at that location has changed.
            mutableData.Value = leaders;
            return TransactionResult.Success(mutableData);
        }


        public IEnumerator IDebugLog(string s)
        {
           debuglog.text=s;
            yield return null;
        }
        public string Otrocolor (string color)
        {
            if (color=="blanco")
            {
                return "negro";
            }
            else if (color=="negro")
            {
                return "blanco";
            }
            return "color erroneo";
        }

    }


}