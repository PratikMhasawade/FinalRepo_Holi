using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class BallonCollisionDetection : MonoBehaviourPunCallbacks
{
    private void OnCollisionEnter(Collision collision)
    {
        /////////////////////////////////////////////
        if(collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<PhotonView>().RPC("Die", RpcTarget.AllBuffered);
        }
        else if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Door" || collision.gameObject.tag == "Stairs")
        {
            gameObject.SetActive(false);
        }
    }
}
