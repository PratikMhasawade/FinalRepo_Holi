using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Photon.Pun;
namespace Shubham_Holi.Scripts
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ColorProjectile : MonoBehaviour
    {
        [HideInInspector]public string OwnerName = "";
        private Vector3 direction = Vector3.zero;
        private float moveSpeed = 30f;
       [FormerlySerializedAs("myTrail")] [SerializeField] internal TrailRenderer[] myTrails;
       [SerializeField] internal ParticleSystem myParticles;

       //Being used From PichkariController
        internal void SetupProjectile(Vector3 shootDirection, Vector3 startPos, Color trailStartColor, Color trailEndColor)
        {
            transform.position = startPos;
            shootDirection.y -= 0.2f; //MAKE-DO-CODE, NEED TO FIX IT PROPERLY
            direction = shootDirection;
            
            foreach (var VARIABLE in myTrails)
            {
                VARIABLE.startColor = trailStartColor;
                VARIABLE.endColor = trailEndColor;
                
            }

        }

        private void Update()
        {
            if (direction != Vector3.zero)
            {
                transform.Translate(direction.normalized * (Time.deltaTime * moveSpeed));
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            Collider tempColl = other.GetComponent<Collider>();
            if (tempColl.gameObject.CompareTag("Player")&& tempColl.gameObject.GetComponent<PhotonView>().Owner.NickName != OwnerName && !tempColl.gameObject.GetComponent<PlayerScript>().IsPlayerDead && FindObjectOfType<Gamemanager>().Gamestate == Gamemanager.GamestateEnum.Gameplay)
            {
                //Debug.LogError(tempColl.gameObject.GetComponent<PhotonView>().name + " hit.");
                object[] content = new object[] { 10f, OwnerName};
                tempColl.gameObject.GetComponent<PhotonView>().RPC("TakingDamage", RpcTarget.AllBuffered,content);
                //if (tempColl.gameObject.GetComponent<PlayerScript>().IsPlayerDead)
                //{
                //    //Debug.LogError(OwnerName + " Killed "+ tempColl.gameObject.GetComponent<PhotonView>().name);
                //    FindObjectOfType<Gamemanager>().SendData(OwnerName, "kill");
                //}
            }
        }

    }
}
