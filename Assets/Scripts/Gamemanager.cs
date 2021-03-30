using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Shubham_Holi.Scripts;
using ExitGames.Client.Photon;
using UnityEngine.UI;
public class Gamemanager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]
    GameObject PlayerPrefab;
    [SerializeField]
    GameObject PlayerListPanel;
    [SerializeField]
    GameObject PlayerNamePrefab;
    [SerializeField]
    GameObject PlayerNameHolder;
    public GameObject OwnPlayerObj;

    public List<PlayerInfo> Playerlist = new List<PlayerInfo>();
    private readonly byte IncrementKillsEvent = 0;
    private readonly byte TimerEvent = 1;
    public readonly byte GameStarted = 2;
    private readonly byte StateChangeEvent = 3;
    public TextMeshProUGUI KillTextOfOwner;
    List<GameObject> VisualInfoArray = new List<GameObject>();
    int lobbyTimerValue = 5;
    int GameTimerValue = 60;
    Coroutine LobbyTimerCo = null;
    Coroutine GameTimerCo = null;
    [HideInInspector] public TextMeshProUGUI LobbyTimeText;
    [HideInInspector] public TextMeshProUGUI GameTimeText;
    bool IslobbyTimerOn = false;
    bool IsGameTimerOn = false;
    public enum GamestateEnum { None, Gameplay, Gameover }
    [HideInInspector] public GamestateEnum Gamestate = GamestateEnum.None;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(Random.Range(-8.0f, 8.0f), 1f, Random.Range(-5.0f, 5.0f)), Quaternion.identity);
            KillTextOfOwner = player.GetComponent<PlayerScript>().KillsText;
            CreatePreviouslyConnectedPlayerNames();
        }
    }
    IEnumerator UpdateLobbyTimer()
    {
        IslobbyTimerOn = true;
        while (true)
        {           
            SendTimerData(lobbyTimerValue, "Lobby");
            if (lobbyTimerValue <=0)
            {
                IslobbyTimerOn = false;
                StopCoroutine(LobbyTimerCo);
            }
            yield return new WaitForSeconds(1f);
            lobbyTimerValue--;
        }
        
    }
    IEnumerator UpdateGameTimer()
    {
        IsGameTimerOn = true;
        while (true)
        {
            SendTimerData(GameTimerValue, "InGame");
            if (GameTimerValue <= 0)
            {
                IsGameTimerOn = false;
                StopCoroutine(GameTimerCo);  
            }
            yield return new WaitForSeconds(1f);
            GameTimerValue--;
        }

    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == IncrementKillsEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            string PName = (string)data[0];
            string Action = (string)data[1];
            if(Action == "kill")
            {
                IncrementKillsInPlayerList(PName);
            }
            else if (Action == "death")
            {
                IncrementDeathsInPlayerList(PName);
            }
        }
        else if(photonEvent.Code == TimerEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            int TimerValue = (int)data[0];
            string TypeOfTimer = (string)data[1];
            if(TypeOfTimer == "Lobby")
            {
                UpdateLobbyTimerFn(TimerValue);
            }
            else if (TypeOfTimer == "InGame")
            {
                UpdateGameTimerFn(TimerValue);
            }

        }
        else if(photonEvent.Code == StateChangeEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            string GameState = (string)data[0];
            ChangeGamestate(GameState);
        }
    }
    void UpdateLobbyTimerFn(int _value)
    {
        lobbyTimerValue = _value;
        //Debug.LogError("Waiting for players : "+ _value);
        if (!LobbyTimeText.gameObject.activeSelf && _value >0)
        {
            LobbyTimeText.gameObject.SetActive(true);
        }
        LobbyTimeText.text = "Waiting for players : " + _value;
        if(_value==0)
        {
            LobbyTimeText.gameObject.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                //print("UpdateGameTimer started");
                GameTimerCo = StartCoroutine("UpdateGameTimer");
                SendGameStateData("Gameplay");
                object[] content = new object[] { };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GameStarted, content, raiseEventOptions, SendOptions.SendUnreliable);
            }
        }
    }
    void UpdateGameTimerFn(int _value)
    {
        GameTimerValue = _value;
        //print("_value : "+ _value);
        //Debug.LogError("Game Time : " + _value);
        if (!GameTimeText.gameObject.activeSelf && _value > 0)
        {
            GameTimeText.gameObject.SetActive(true);
        }
        GameTimeText.text = "GameTime : " + _value;
        if (_value == 0)
        {
            GameTimeText.text = "TimeUp";
            if (PhotonNetwork.IsMasterClient)
            {
                SendGameStateData("Gameover");
            }
                
        }
    }
    public void ChangeGamestate(string StateName)
    {
        print("StateName : " + StateName);
        if(StateName == "Gameover")
        {
            Gamestate = GamestateEnum.Gameover;
            CallGameOver();
        }
        else if(StateName == "Gameplay")
        {
            Gamestate = GamestateEnum.Gameplay;
        }
    }
    public void CallGameOver()
    {
        StartCoroutine("GameOver");
    }
    IEnumerator GameOver()
    {
        yield return null;
        PlayerListPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Networking");
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(!PlayerListPanel.activeSelf)
            {
                PlayerListPanel.SetActive(true);
            }
            else
            {
                PlayerListPanel.SetActive(false);
            }
        }
    }
    void CreatePlayerNameInList(string playerN)
    {
        GameObject Playername = Instantiate(PlayerNamePrefab);
        Playername.transform.SetParent(PlayerNameHolder.transform);
        Playername.gameObject.name = playerN;
        Playername.GetComponent<TextMeshProUGUI>().text = playerN + "               0               0";
        VisualInfoArray.Add(Playername);
        PlayerInfo playerInfo = new PlayerInfo
        {
            name = playerN,
            kills = 0,
            deaths = 0
        };
        Playerlist.Add(playerInfo);
    }
    void CreatePreviouslyConnectedPlayerNames()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject Playername = Instantiate(PlayerNamePrefab);
            Playername.transform.SetParent(PlayerNameHolder.transform);
            Playername.gameObject.name = player.NickName;
            Playername.GetComponent<TextMeshProUGUI>().text = player.NickName + "               0               0";
            VisualInfoArray.Add(Playername);
            PlayerInfo playerInfo = new PlayerInfo
            {
                name = player.NickName,
                kills = 0,
                deaths = 0
            };
            Playerlist.Add(playerInfo);
        }
    }
    void UpdateStatsUI(string _Name,int _Kills,int Deaths)
    {
        for(int i=0;i< VisualInfoArray.Count;i++)
        {
            if(VisualInfoArray[i].name == _Name)
            {
                VisualInfoArray[i].GetComponent<TextMeshProUGUI>().text = _Name + "               " + _Kills + "               " + Deaths;
            }
        }
    }
    void RemovePlayerFromUI(string _Name)
    {
        for (int i = 0; i < VisualInfoArray.Count; i++)
        {
            if (VisualInfoArray[i].name == _Name)
            {
                VisualInfoArray.Remove(VisualInfoArray[i]);
                Destroy(VisualInfoArray[i]);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Debug.LogError("Player left : " + otherPlayer.NickName) ;
        RemovePlayerFromUI(otherPlayer.NickName);
    }
    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.LocalPlayer.NickName + " entered the " + PhotonNetwork.CurrentRoom.Name);
        
    }
    public override void OnLeftRoom()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if(IslobbyTimerOn)
            {
                StopCoroutine("UpdateLobbyTimer");
            }
            else if(IsGameTimerOn)
            {
                StopCoroutine("UpdateGameTimer");
            }           
            TransferMasterClientTag();
        }
    }
    
    void TransferMasterClientTag()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.NickName != PhotonNetwork.LocalPlayer.NickName)
            {
                PhotonNetwork.SetMasterClient(player);
                break;
            }
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(newMasterClient.NickName == PhotonNetwork.LocalPlayer.NickName)
        {
            //print("OnMasterClientSwitched");
            if(lobbyTimerValue >0)
            {
                LobbyTimerCo = StartCoroutine("UpdateLobbyTimer");
            }
            else if(GameTimerValue>0)
            {
                GameTimerCo = StartCoroutine("UpdateGameTimer");
            }           
        }
    }
    bool ShouldStartLobbytimer = true;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && ShouldStartLobbytimer)
        {
            LobbyTimerCo = StartCoroutine("UpdateLobbyTimer");
            ShouldStartLobbytimer = false;
            
        }
        print(newPlayer.NickName + " entered the " + PhotonNetwork.CurrentRoom.Name + " player count : " + PhotonNetwork.CurrentRoom.PlayerCount);
        CreatePlayerNameInList(newPlayer.NickName);
    }
    
    public void SendData(string _name, string KOD)
    {
        //Debug.LogError("Name : " + _name +" Event : "+ KOD);
        //print("Name : " + _name + " Event : " + KOD);
        object[] content = new object[] { _name, KOD };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(IncrementKillsEvent, content, raiseEventOptions, SendOptions.SendUnreliable);
    }
    public void SendTimerData(int Timer, string type)
    {
        //Debug.LogError("Name : " + _name +" Event : "+ KOD);
        //print("Name : " + _name + " Event : " + KOD);
        object[] content = new object[] { Timer, type };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(TimerEvent, content, raiseEventOptions, SendOptions.SendUnreliable);      
    }
    public void SendGameStateData(string _state)
    {
        object[] content = new object[] { _state};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(StateChangeEvent, content, raiseEventOptions, SendOptions.SendUnreliable);
    }
    public void IncrementKillsInPlayerList(string _Name)
    {
        for (int i = 0; i < Playerlist.Count; i++)
        {
            if (Playerlist[i].name == _Name)
            {
                Playerlist[i].kills += 1;
                UpdateStatsUI(Playerlist[i].name, Playerlist[i].kills, Playerlist[i].deaths);
                //Debug.LogError("Name : " + Playerlist[i].name + " Kills : " + Playerlist[i].kills);
                //print("Name : " + Playerlist[i].name + " Kills : " + Playerlist[i].kills);
                if (Playerlist[i].name == PhotonNetwork.LocalPlayer.NickName)
                {
                    KillTextOfOwner.text = "KILLS: " + Playerlist[i].kills;
                }
            }
        }        
    }
    public void IncrementDeathsInPlayerList(string _Name)
    {
        //Debug.LogError("IncrementDeathsInPlayerList called");
        for (int i = 0; i < Playerlist.Count; i++)
        {
            if (Playerlist[i].name == _Name)
            {
                Playerlist[i].deaths += 1;
                UpdateStatsUI(Playerlist[i].name, Playerlist[i].kills, Playerlist[i].deaths);
                //Debug.LogError("Name : " + Playerlist[i].name + " deaths : " + Playerlist[i].deaths);
                //print("Name : " + Playerlist[i].name + " deaths : " + Playerlist[i].deaths);
            }
        }
        

    }

}
public class PlayerInfo
{
    public string name;
    public int kills;
    public int deaths;
}
