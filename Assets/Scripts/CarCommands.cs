﻿using UnityEngine;

public class CarCommands : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;
    Rigidbody originalRigidbody;

    private void Start()
    {
        originalPosition = this.transform.localPosition;
        originalRotation = this.transform.localRotation;
        originalRigidbody = this.GetComponent<Rigidbody>();
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
    }

    void EnableGravity()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
    }

    void DisableGravity()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
    }

    void OnReset()
    {
        this.transform.localPosition = originalPosition;
        this.transform.localRotation = originalRotation;
    }
}