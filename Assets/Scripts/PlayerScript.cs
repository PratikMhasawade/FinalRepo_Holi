using System;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

namespace Shubham_Holi.Scripts
{
    public class PlayerScript : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private AnimController myAnim = null;
        private PlayerCameraController cameraController = null;
        private PichkariController pichkariController = null;
        private movementController MovementController = null;
        private PlayerStats myStats = null;

        [SerializeField] private bool isCharacterInAnimation;
        [SerializeField] private GameObject Crosshair;

        [FormerlySerializedAs("KeycodeMove")] [Header("CONTROL INPUTS / PC")]
        [SerializeField] private KeyCode keycodeMoveForward;
        [SerializeField] private KeyCode keycodeMoveBack;
        [SerializeField] private KeyCode keyboardMoveLeft;
        [SerializeField] private KeyCode keyboardMoveRight;
        [SerializeField] private KeyCode keycodeShoot;
        [SerializeField] private KeyCode keycodeBalloon;
        /// <Pratik>
        public int Deaths = 0;
        public int Kills = 0;
        public bool IsPlayerDead = false;
        public GameObject CharacterModel;
        public GameObject PlayerUI;
        public TextMeshProUGUI KillsText;
        public TextMeshProUGUI DeathsText;
        /// </Pratik>
        [Header("GAME SETTINGS")]
        [SerializeField] private float moveSpeed;
        Vector2 moveVector = Vector2.zero;
        public string Username = "";
        [SerializeField]TextMeshProUGUI LobbyTimerText;
        [SerializeField] TextMeshProUGUI GameTimerText;
        
        private void Awake()
        {
			if (photonView.IsMine)
            {
                //gameObject.GetComponent<movementController>().enabled = true;
                myAnim = GetComponent<AnimController>();
                //gameObject.GetComponent<PlayerCameraController>().enabled = true;
                cameraController = GetComponent<PlayerCameraController>();
                //gameObject.GetComponentInChildren<AudioListener>().enabled = true;
                //gameObject.GetComponent<AnimController>().enabled = true;
                
                MovementController = GetComponent<movementController>();
                FindObjectOfType<Gamemanager>().OwnPlayerObj = gameObject;
                FindObjectOfType<Gamemanager>().LobbyTimeText = LobbyTimerText;
                FindObjectOfType<Gamemanager>().GameTimeText = GameTimerText;
                Username = PhotonNetwork.LocalPlayer.NickName;
            }
            else
            {
                gameObject.GetComponent<movementController>().enabled = false;
                gameObject.GetComponent<BallonController>().enabled = false;
                gameObject.GetComponent<PlayerCameraController>().playerCameraParent.gameObject.SetActive(false);
                gameObject.GetComponent<PlayerCameraController>().enabled = false;
                gameObject.GetComponent<AnimController>().enabled = false;
                //GetComponent<PichkariController>().enabled = false;
                PlayerUI.SetActive(false);
                // gameObject.GetComponentInChildren<AudioListener>().enabled = false;
            }
            FindObjectOfType<Gamemanager>().LobbyTimeText.gameObject.SetActive(true);
            pichkariController = GetComponent<PichkariController>();
            myStats = GetComponent<PlayerStats>();
            //Cursor.visible = false;
        }
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == FindObjectOfType<Gamemanager>().GameStarted)
            {
                transform.position = new Vector3(UnityEngine.Random.Range(-8.0f, 8.0f), 1f, UnityEngine.Random.Range(-5.0f, 5.0f));
            }
        }
        private void Update()
        {
            AnimCalls();
            ManageCrosshair();
            if (Input.GetKeyDown(keycodeShoot))
            {
				if(photonView.IsMine)
                {
                    gameObject.GetComponent<PhotonView>().RPC("Shoot", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName.ToString());
                }
            }
        }
        private void FixedUpdate()
        {
			if(MovementController!=null)
				MovementController.PlayerMoveKeyboard(moveSpeed, moveVector);
        }


        private void AnimCalls()
        {
            
            if (Input.GetKey(keycodeMoveForward))
            {
                if(moveVector.y < 1f)
                    moveVector.y += 0.1f;
            } 
            else if (Input.GetKey(keycodeMoveBack))
            {
                if(moveVector.y > -1f)
                    moveVector.y -= 0.1f;
            }
            else
            {
                moveVector.y = Mathf.MoveTowards(moveVector.y, 0, Time.deltaTime * 10); // Smoothly Goes To Default State
            }


            if (Input.GetKey(keyboardMoveRight))
            {
                moveVector.x = 1;
            } 
            else if (Input.GetKey(keyboardMoveLeft))
            {
                moveVector.x = -1;
            }
            else
            {
                moveVector.x = 0;
            }

            if (!isCharacterInAnimation)
            {
                if (myAnim != null)
                {
                    myAnim.MoveState(moveVector);
                    myAnim.ShootState(Input.GetKey(keycodeShoot));
                    myAnim.MoveBackState(Input.GetKey(keycodeMoveBack));
                }
                    
            }
            
            if (Input.GetKeyUp(keycodeBalloon))
            {
                if(photonView.IsMine)
                {
                    //gameObject.GetComponent<PhotonView>().RPC("PlayThrowAnim", RpcTarget.AllBuffered);
                    myAnim.SetTrigger("Throw");
                }
                    
            }
            
        }
        public void PlayThrowAnim()
        {
            //myAnim.("Throw");
        }
        public void ManageCrosshair()
        {
            if (Input.GetKey(keycodeMoveForward))
            {
                Crosshair.transform.localScale = new Vector3(0.01f, y: 0.01f);
            }
            else
            {
                Crosshair.transform.localScale = new Vector3(0.005f, y: 0.005f);
            }
        }
		public void RespawnAfterDeath()
        {
            StartCoroutine("Respawn");
        }
        IEnumerator Respawn()
        {
            transform.position = new Vector3(UnityEngine.Random.Range(-8.0f, 8.0f), 1f, UnityEngine.Random.Range(-5.0f, 5.0f));
            if(photonView.IsMine)
            {
                PlayerUI.SetActive(true);
            }           
            CharacterModel.SetActive(true);
            GetComponent<TakeDamage>().ResetHealth();
            yield return new WaitForSeconds(2f);
            IsPlayerDead = false;
        }
        public void SetDeathText()
        {

            //KillsText.text = "KILLS: " + Kills;
            DeathsText.text = "DEATHS: " + Deaths;
        }
        


    }
}
