using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class BallonCollisionDetection : MonoBehaviourPunCallbacks
{
    [HideInInspector] public string OwnerName = "";
    private void OnCollisionEnter(Collision collision)
    {
        /////////////////////////////////////////////
        if(collision.gameObject.tag == "Player"&& collision.gameObject.GetComponent<PhotonView>().Owner.NickName != OwnerName && !collision.gameObject.GetComponent<PhotonView>().IsMine && FindObjectOfType<Gamemanager>().Gamestate == Gamemanager.GamestateEnum.Gameplay)
        {
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<PhotonView>().RPC("Die", RpcTarget.AllBuffered);
            FindObjectOfType<Gamemanager>().SendData(collision.gameObject.GetComponent<PhotonView>().name, "death");
            FindObjectOfType<Gamemanager>().SendData(OwnerName, "kill");
        }
        else if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door" || collision.gameObject.tag == "Stairs")
        {
            gameObject.SetActive(false);
        }
    }
}
