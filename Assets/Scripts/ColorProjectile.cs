using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Shubham_Holi.Scripts
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ColorProjectile : MonoBehaviour
    {
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
        
    }
}
