using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BallonController : MonoBehaviour
{
    public GameObject BallPrefab;

    // Reference to the slot for holding picked item.
    [SerializeField]
    private Transform slot;

    [Header("Throw")]
    // Velocity which which object will be thrown.
    [SerializeField]
    private Vector3 throwVelocity = new Vector3(0, 0, 5);

    /// <summary>
    /// Event class which will be displayed in the inspector.
    /// </summary>
    [System.Serializable]
    public class LocationChanged : UnityEvent<Vector3, Vector3> { }

    [Space]

    // Event for location change. Used to update ballistic trajectory.
    public LocationChanged OnLocationChanged;

    /// <summary>
    /// Method called every frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Invoke("Shoot", 1f);
        }


        // Broadcast location change
        OnLocationChanged?.Invoke(slot.position, slot.rotation * throwVelocity);
    }
    
    /// <summary>
    /// Method for picking up item.
    /// </summary>
    /// <param name="item">Item.</param>

    private void Shoot()
    {
        GameObject temp = Instantiate(BallPrefab, slot.transform.position, slot.transform.rotation);
        temp.transform.SetParent(null);
        //temp.GetComponent<Rigidbody>().isKinematic = false;
        temp.GetComponent<Rigidbody>().velocity = slot.rotation * throwVelocity;
    }
}
