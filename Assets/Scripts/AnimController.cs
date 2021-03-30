using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shubham_Holi.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class AnimController : MonoBehaviour
    {
        private Animator myAnim;
        public Animation gunTurretShootAnimation;
        private static readonly int GameStarted = Animator.StringToHash("GameStarted");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsShooting = Animator.StringToHash("IsShooting");
        private static readonly int IsMovingBackwards = Animator.StringToHash("IsMovingBackwards");
        private static readonly int Throw = Animator.StringToHash("Throw");
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");

        private void OnEnable()
        {
            myAnim = GetComponent<Animator>();
        }
        private void Start()
        {
            myAnim.SetTrigger(GameStarted);
        }
        public void SetTrigger(string triggerName)
        {
            myAnim.SetTrigger(triggerName);
        }
        public void MoveState(Vector2 blendState)
        {
            myAnim.SetBool(IsMoving, blendState != Vector2.zero);

            myAnim.SetFloat(X,blendState.x);
            myAnim.SetFloat(Y,blendState.y);
        }
        public void MoveBackState(bool state)
        {
            myAnim.SetBool(IsMovingBackwards,state);
        }
        public void ShootState(bool state)
        {
            myAnim.SetBool(IsShooting,state);

            if (state)
            {
                if (!gunTurretShootAnimation.isPlaying)
                {
                    gunTurretShootAnimation.Play();
                }
            }
        }
    }
}
