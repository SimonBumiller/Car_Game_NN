///Code taken from Samuel Arzt - Applying EANNs
/// <link> https://github.com/ArztSamuel/Applying_EANNs </link>

using System;
using UnityEngine;
using UnityEngine.Assertions;

public class CarMovement : MonoBehaviour
{
    private const float MaxVel = 20f;
    private const float Acceleration = 16f;
    private const float Friction = 10f;
    private const float TurnSpeed = 100;

    private double horizontalInput, verticalInput; //Horizontal = engine force, Vertical = turning force

    private Quaternion Rotation;

    private bool UserControlled;

    public float Velocity;

    public void Reset()
    {
        enabled = true;
    }

    private void FixedUpdate()
    {
        if (UserControlled) ReadInput();

        ApplyInput();
        ApplyVelocity();
        ApplyFriction();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (OnHitWall != null)
            OnHitWall();
    }

    public event Action OnHitWall;

    public void SetIsUserControlled(bool enabled)
    {
        UserControlled = enabled;
    }

    public void Die()
    {
        Velocity = 0F;
        enabled = false;
    }

    private void ReadInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void ApplyInput()
    {
        //Car can only accelerate further if velocity is lower than engineForce * MAX_VEL
        var canAccelerate = false;
        if (verticalInput < 0)
            canAccelerate = Velocity > verticalInput * MaxVel;
        else if (verticalInput > 0)
            canAccelerate = Velocity < verticalInput * MaxVel;

        //Set velocity
        if (canAccelerate)
        {
            Velocity += (float)verticalInput * Acceleration * Time.deltaTime;

            //Cap velocity
            if (Velocity > MaxVel)
                Velocity = MaxVel;
            else if (Velocity < -MaxVel)
                Velocity = -MaxVel;
        }

        //Set rotation
        Rotation = transform.rotation;
        Rotation *= Quaternion.AngleAxis((float)-horizontalInput * TurnSpeed * Time.deltaTime, new Vector3(0, 0, 1));
    }

    private void ApplyVelocity()
    {
        var direction = new Vector3(0, 1, 0);
        transform.rotation = Rotation;
        direction = Rotation * direction;

        var coefficient = Velocity * Time.deltaTime;
        transform.position += direction * coefficient;
    }

    private void ApplyFriction()
    {
        if (verticalInput == 0)
        {
            if (Velocity > 0)
            {
                Velocity -= Friction * Time.deltaTime;
                if (Velocity < 0)
                    Velocity = 0;
            }
            else if (Velocity < 0)
            {
                Velocity += Friction * Time.deltaTime;
                if (Velocity > 0)
                    Velocity = 0;
            }
        }
    }

    public void SetInputs(float[] inputs)
    {
        Assert.AreEqual(inputs.Length, 2, "Input Length must be 2: Rotation and engine power.");

        horizontalInput = inputs[0];
        verticalInput = inputs[1];
    }
}