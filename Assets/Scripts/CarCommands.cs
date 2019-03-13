using UnityEngine;

public class CarCommands : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;

    private void Start()
    {
        originalPosition = this.transform.localPosition;
        originalRotation = this.transform.localRotation;
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        // If the car has no Rigidbody component, add one to enable physics.
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        else if (this.GetComponent<Rigidbody>().useGravity == false)
        {
            EnableGravity();
        }
    }

    void EnableGravity()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
    }

    void DisableGravity()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
    }

    void Reset()
    {
        this.transform.localPosition = originalPosition;
        this.transform.localRotation = originalRotation;
        this.GetComponent<GameObject>().GetComponentInChildren<WheelCollider>().motorTorque = 0;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}