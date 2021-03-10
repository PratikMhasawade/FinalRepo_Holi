using UnityEngine;

namespace Shubham_Holi.Scripts
{
    public class CameraCollision : MonoBehaviour
    {
        public Transform referenceTransform;
        public float collisionOffset = 0.2f; //To prevent Camera from clipping through Objects

        Vector3 _defaultPos;
        Vector3 _directionNormalized;
        Transform _parentTransform;
        float _defaultDistance;

        // Start is called before the first frame update
        void Start()
        {
            _defaultPos = transform.localPosition;
            _directionNormalized = _defaultPos.normalized;
            _parentTransform = transform.parent;
            _defaultDistance = Vector3.Distance(_defaultPos, Vector3.zero);
        }

        // FixedUpdate for physics calculations
        void FixedUpdate()
        {
            Vector3 currentPos = _defaultPos;
            RaycastHit hit;
            Vector3 dirTmp = _parentTransform.TransformPoint(_defaultPos) - referenceTransform.position;
            if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, _defaultDistance))
            {
                currentPos = (_directionNormalized * (hit.distance - collisionOffset));
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 15f);
        }
    }
}