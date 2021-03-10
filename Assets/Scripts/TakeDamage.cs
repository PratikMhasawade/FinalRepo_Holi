using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Shubham_Holi.Scripts;
public class TakeDamage : MonoBehaviour
{
    [SerializeField]
    Image HealthBar;
    public float health = 100;
    [SerializeField]PlayerScript _PlayerScriptObj;
    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
    }
    public void ResetHealth()
    {
        health = 100;
        HealthBar.fillAmount = health / 100;
    }
    [PunRPC]
    public void TakingDamage(float _damage)
    {
        health -= _damage;
        HealthBar.fillAmount = health / 100;
        if(health == 0 )
        {
            Die();
        }
        //Debug.Log("health: " + health);
    }
    [PunRPC]
    public void Die()
    {
        _PlayerScriptObj.Deaths++;
        _PlayerScriptObj.SetKillsAndDeathText();
        _PlayerScriptObj.IsPlayerDead = true;
        _PlayerScriptObj.PlayerUI.SetActive(false);
        _PlayerScriptObj.CharacterModel.SetActive(false);
        _PlayerScriptObj.RespawnAfterDeath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
