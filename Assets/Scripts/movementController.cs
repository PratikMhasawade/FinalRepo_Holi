using UnityEngine;

namespace Shubham_Holi.Scripts
{
    public class movementController : MonoBehaviour
    {
        private Rigidbody _rb;
        Vector3 _moveVector;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

  
        public void PlayerMoveKeyboard(float moveSpeed , Vector2 moveVector)
        {
            _moveVector.x = moveVector.x;
            _moveVector.z = moveVector.y;
            
            _moveVector.Normalize();
            _rb.transform.Translate(_moveVector * (moveSpeed * Time.deltaTime));
            Physics.SyncTransforms();   
        }
    }
}
