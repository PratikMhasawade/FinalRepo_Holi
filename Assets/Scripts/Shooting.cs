using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Shubham_Holi.Scripts;
public class Shooting : MonoBehaviour
{
    [SerializeField]
    Camera FPSCamera;
    public float fireRate = 0.1f;
    float FireTimer;
    // Start is called before the first frame update
    void Start()
    {
        // Checking A Comment
    }

    // Update is called once per frame
    void Update()
    {
        if(FireTimer<fireRate)
        {
            FireTimer += Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && FireTimer>fireRate)
        {
            //print("In Fire1 pressed");
            //FireTimer = 0.0f;
            //RaycastHit hit;
            //Ray ray = FPSCamera.ViewportPointToRay(new Vector3(0.5f,0.5f));
            ////Debug.DrawRay(transform.position, forward, Color.green);
            //if (Physics.Raycast(ray,out hit,100))
            //{
            //    Debug.Log(hit.collider.gameObject.name);
            //    if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine && !hit.collider.gameObject.GetComponent<PlayerScript>().IsPlayerDead)
            //    {
            //        Debug.Log(hit.collider.gameObject.name);
            //        print(hit.collider.gameObject.GetComponent<PhotonView>().name + " hit.");
            //        hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakingDamage",RpcTarget.AllBuffered,10f);
            //        if(hit.collider.gameObject.GetComponent<PlayerScript>().IsPlayerDead)
            //        {                 
            //            FindObjectOfType<Gamemanager>().OwnPlayerObj.GetComponent<PlayerScript>().Kills++;
            //            FindObjectOfType<Gamemanager>().OwnPlayerObj.GetComponent<PlayerScript>().SetKillsAndDeathText();
            //        }
            //    }
                
            //}

        }
    }
}
