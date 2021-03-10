using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class Gamemanager : MonoBehaviourPunCallbacks
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
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(Random.Range(-8.0f,8.0f),1f, Random.Range(-5.0f, 5.0f)),Quaternion.identity);
            CreatePreviouslyConnectedPlayerNames();
            //CreatePlayerNameInList(PhotonNetwork.LocalPlayer.NickName);
        }
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
        if(Input.GetKeyDown(KeyCode.A))
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                print(player.GetType());
            }
        }
    }
    void CreatePlayerNameInList(string playerN)
    {
        //foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject Playername = Instantiate(PlayerNamePrefab);
            Playername.transform.SetParent(PlayerNameHolder.transform);
            Playername.GetComponent<TextMeshProUGUI>().text = playerN;
            //print(playerN+" dddd");
        }
    }
    void CreatePreviouslyConnectedPlayerNames()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject Playername = Instantiate(PlayerNamePrefab);
            Playername.transform.SetParent(PlayerNameHolder.transform);
            Playername.GetComponent<TextMeshProUGUI>().text = player.NickName;
        }
    }
    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.LocalPlayer.NickName + " entered the " + PhotonNetwork.CurrentRoom.Name);

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer.NickName + " entered the " + PhotonNetwork.CurrentRoom.Name + " player count : " + PhotonNetwork.CurrentRoom.PlayerCount);
        CreatePlayerNameInList(newPlayer.NickName);
    }
    
}
