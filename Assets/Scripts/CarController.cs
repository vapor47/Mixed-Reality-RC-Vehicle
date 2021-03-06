﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour {

    public List<AxleInfo> axleInfos;
    public float maxSpeed;
    public float maxSteeringAngle;
    public int maxJumps;
    public int jumpForce;
    public int boostForce;
    private int jumpsLeft;

    private float motor;
    private float steering;

    Rigidbody rb;

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        DropCar(5);
    }

    public void Update()
    {
        motor = maxSpeed * (Input.GetAxis("Accelerate") - Input.GetAxis("Decelerate"));
        steering = maxSteeringAngle * Input.GetAxis("LeftJoystickX");
    }

    public void FixedUpdate()
    {
        if (Input.GetButton("Boost"))
        {
            Boost();
        }

        if (Input.GetButtonDown("Jump"))
        {
            // if not upside down, jump
            // else, flip rightside up
            Jump();
        }

        // Car is grounded (all four wheels colliding with something)
        if (isGrounded())
        {

            jumpsLeft = maxJumps;
            if (Input.GetButtonDown("Jump"))
            {
                // if not upside down, jump
                // else, flip rightside up
                //Jump();
            }
        }

        // Car is airborne
        else
        {
            if (Input.GetAxis("LeftJoystickX") != 0)
            {
                if (Input.GetButton("Slide"))
                {
                    Roll();
                }
                else
                {
                    Yaw();
                }
            }
            if (Input.GetAxis("LeftJoystickY") != 0)
            {
                Pitch();
            }
        }
        /*
         *      ground
         * drift
         * driving and boost normal
         * steering rotates wheels, like normal
         * if upside down, jump should reset the cars position to be upright (jump and tilt 180)
         * 
         *      airborne
         * tilt
         * driving does nothing, boost still should move car
         * steering will rotate car body(Y axis), rather than the wheels
         * vertical joystick should rotate car on its X axis
         * 
         * if (jumpPressed && jumpsLeft > 0)
         *     Jump();
         *          
         *      if (
        */


        //rb.velocity += transform.forward * motor * Time.fixedDeltaTime;
        //Yaw();
        //Debug.Log(motor + " " + rb.velocity.sqrMagnitude);
        Debug.Log(steering);
        
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    private void Jump()
    {
        //forcemode.impulse
        if (jumpsLeft > 0)
        {
            jumpsLeft--;
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        Debug.Log(jumpsLeft);
    }

    // Resets car position by flipping over if it is unable to get back to a drivable position
    private void FlipOver()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        GetComponent<Rigidbody>().MoveRotation(new Quaternion(0, 0, 0, 0));
    }

    private void Boost()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * boostForce, ForceMode.Force);
    }

    private void Roll()
    {
        transform.Rotate(Vector3.back, Input.GetAxis("LeftJoystickX"));
    }

    // Tilt nose up/down
    private void Pitch()
    {
        transform.Rotate(Vector3.left, Input.GetAxis("LeftJoystickY"));
    }

    // Horizontal rotation
    private void Yaw()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("LeftJoystickX"));
    }

    private bool isGrounded()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (!(axleInfo.leftWheel.isGrounded && axleInfo.rightWheel.isGrounded))
                return false;
        }
        //Debug.Log("Grounded");
        return true;
    }

    IEnumerator DropCar(float time)
    {
        yield return new WaitForSeconds(time);
    }
}