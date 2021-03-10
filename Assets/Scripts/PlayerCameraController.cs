using UnityEngine;

namespace Shubham_Holi.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCameraController : MonoBehaviour
    {
        public Transform playerCameraParent;
        public float lookSpeed = 2.0f;
        public float lookXLimitMin = 60.0f;
        public float lookXLimitMax = 60.0f;
        Vector2 _rotation = Vector2.zero;

        [HideInInspector]
        public bool canMove = true;

        void Start()
        {
            _rotation.y = transform.eulerAngles.y;
        }

        void Update()
        {
            if (canMove)
            {
                _rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
                _rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
                _rotation.x = Mathf.Clamp(_rotation.x, lookXLimitMin, lookXLimitMax);
                playerCameraParent.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
                transform.eulerAngles = new Vector2(0, _rotation.y);
            }
        }
    }
}
