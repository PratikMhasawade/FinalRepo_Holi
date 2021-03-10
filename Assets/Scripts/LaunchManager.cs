using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class LaunchManager : MonoBehaviourPunCallbacks
{
    public enum GameMode {Death,Team}
    public GameMode gameMode;
    public GameObject GameModeSelectionPanel;
    public InputField PlayerNameInputField;
    public Text ErrorMessage;
    public GameObject PlayerListPanel;
    public GameObject PlayerListParent;
    public GameObject PlayerListTextPrefab;
    [HideInInspector] public string PlayerName = "";
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        ConnectToPhotonServer();
    }
    void Update()
    {

    }
    void CreatePlayerList()
    {

    }
    public void OnDeathModeClicked()
    {
        if(PlayerNameInputField.text != "")
        {
            print("Death Mode selected...");
            ErrorMessage.text = "";
            gameMode = GameMode.Death;
            PhotonNetwork.LocalPlayer.NickName = PlayerNameInputField.text;
            GameModeSelectionPanel.SetActive(false);
            JoinRandomRoom();
        }
        else
        {
            ErrorMessage.text = "Please enter player name...";
        }
        
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void CreateAndJoinRoom()
    {
        if (gameMode == GameMode.Death)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;
            options.IsOpen = true;
            options.IsVisible = true;
            string RoomName = "Room" + (Random.Range(1, 10000)).ToString();
            PhotonNetwork.CreateRoom(RoomName, options, TypedLobby.Default);
        }
    }
    public void OnTeamModeClicked()
    {
        if (PlayerNameInputField.text != "")
        {
            print("Team Mode selected...");
            ErrorMessage.text = "";
            gameMode = GameMode.Team;
            PhotonNetwork.LocalPlayer.NickName = PlayerNameInputField.text;
            GameModeSelectionPanel.SetActive(false);
            JoinRandomRoom();
        }
        else
        {
            ErrorMessage.text = "Please enter player name...";
        }
    }
    void ConnectToPhotonServer()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }
    public override void OnConnectedToMaster()
    {
        print("Connected to server...");
        GameModeSelectionPanel.SetActive(true);
    }
    public override void OnConnected()
    {
        print("Connected to internet...");
    }
    public override void OnCreatedRoom()
    {
        print("Room Created Successfully...");
        print("Room Name : " + PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Room Creation failed : "+message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print(message);
        CreateAndJoinRoom();
    }
    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.LocalPlayer.NickName + " entered the " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("Demo");

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer.NickName + " entered the " + PhotonNetwork.CurrentRoom.Name +" player count : "+PhotonNetwork.CurrentRoom.PlayerCount);
    }
    // Update is called once per frame

}
