﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public List<WheelAxle> wheelAxleList; 
    public VehicleSettings vehicleSettings;
    [SerializeField] private float _ackermanFactor;
    private Rigidbody _rigidbody;
    private float _startSpeed = 2;
    private float _maxSpeed = 3.5f;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = vehicleSettings.mass;
        _rigidbody.drag = vehicleSettings.drag;
        _rigidbody.centerOfMass = vehicleSettings.centerOfMass;
        _rigidbody.velocity = new Vector3(0, 0, _startSpeed);
    }

    private void FixedUpdate()
    {
        if (_rigidbody.velocity.z > _maxSpeed)
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _maxSpeed);

        float motorTorque = vehicleSettings.motorTorque;
        float steering = vehicleSettings.steeringAngle * Input.GetAxis("Horizontal");

        foreach (WheelAxle wheelAxle in wheelAxleList)
        {
            if (wheelAxle.steering)
            {
                float sign = -1f;
                float factor = 1f;

                if (Mathf.Approximately(sign, Mathf.Sign(wheelAxle.wheelColliderLeft.steerAngle)))
                    factor = _ackermanFactor;

                wheelAxle.wheelColliderLeft.steerAngle = steering * factor;

                sign = 1f;
                factor = 1f;

                if (Mathf.Approximately(sign, Mathf.Sign(wheelAxle.wheelColliderRight.steerAngle)))
                    factor = _ackermanFactor;

                wheelAxle.wheelColliderRight.steerAngle = steering * factor;
            }

            if (wheelAxle.motor)
            {
                wheelAxle.wheelColliderLeft.motorTorque = motorTorque;
                wheelAxle.wheelColliderRight.motorTorque = motorTorque;
            }

            ApplyWheelVisuals(wheelAxle.wheelColliderLeft, wheelAxle.wheelMeshLeft);
            ApplyWheelVisuals(wheelAxle.wheelColliderRight, wheelAxle.wheelMeshRight);
        }
    }

    private void ApplyWheelVisuals(WheelCollider wheelCollider, GameObject wheelMesh)
    {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        Quaternion realRotation = rotation * Quaternion.Inverse(wheelCollider.transform.parent.rotation) *
                                  transform.rotation;

        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = realRotation;
    }
}