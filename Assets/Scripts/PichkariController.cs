using System;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

namespace Shubham_Holi.Scripts
{
   public class PichkariController : MonoBehaviourPunCallbacks
   {
      [SerializeField] private Transform pichkariParent;
      [SerializeField] private Transform crosshair;
      [SerializeField] private Transform gunTurret;
      [SerializeField] private Animation splashFlowAnimation;
      [SerializeField] private ParticleSystem flowParticleSystem;
      [SerializeField] private ColorProjectile colorProjectile;
      [SerializeField] private ParticleSystem impactParticle;
      [SerializeField] private Color trailStartColor;
      [SerializeField] private Color trailEndColor;

      [PunRPC]
      internal void Shoot(string ownername)
      {
          RaycastHit hit;

          var forward = crosshair.forward;

          if (Physics.Raycast(crosshair.position, crosshair.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
          {
            //  Perform Shooting From Here Later
          }

          ColorProjectile newProjectile = Instantiate(colorProjectile);
          newProjectile.OwnerName = ownername;
          //Debug.LogError("newProjectile.OwnerName : " + newProjectile.OwnerName);
          newProjectile.SetupProjectile(forward.normalized,gunTurret.position,trailStartColor,trailEndColor);
          Destroy(newProjectile.gameObject,2f);
          var mainModule = flowParticleSystem.main;
          mainModule.startColor = trailStartColor;
          splashFlowAnimation.Play();
      }

      private void FixedUpdate()
      {
          var forward = crosshair.forward;
          Debug.DrawRay(gunTurret.position, (forward.normalized * 100), Color.black);
          pichkariParent.forward = -(forward.normalized * 100);
      }
   }
}
