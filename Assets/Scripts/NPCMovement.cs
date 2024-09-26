using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class NPCMovement : MonoBehaviour
{
    private float _changeDirectionInterval = 5f;
    private float _speed = 0.8f;
    private Coroutine _changeDirectionCoroutine;
    private Vector3 _moveDirection;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private bool _canTrigger;
    private float _maxDeflectAngle = 60;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _transform.rotation = Quaternion.identity;
        _moveDirection = Vector3.zero;
        _canTrigger = true;

        Move();

        _changeDirectionCoroutine = StartCoroutine(ChangeDirectionWithInterval());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bumper bumper) || other.TryGetComponent(out VehicleCatchZone zone))
            _canTrigger = false;

        if (_canTrigger && (other.isTrigger || other.TryGetComponent(out Obstacle obstacle)))
            ReflectDirection(other);
    }

    public void Disable()
    {
        _rigidbody.velocity = Vector3.zero;

        StopCoroutine(_changeDirectionCoroutine);

        enabled = false;
    }

    private void Move()
    {
        _rigidbody.velocity = Vector3.zero;

        if (_moveDirection == Vector3.zero)
            _moveDirection = GetRandomDirection();

        _transform.rotation = Quaternion.LookRotation(_moveDirection);

        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    private void ReflectDirection(Collider otherCollider)
    {
        if (_changeDirectionCoroutine != null)
            StopCoroutine(_changeDirectionCoroutine);

        Vector3 position = _transform.position;

        Vector3 normal = otherCollider.ClosestPointOnBounds(position) - position;

        Vector3 newDirection = new Vector3(-normal.x, 0f, -normal.z).normalized;

        Quaternion deflection = Quaternion.Euler(0f, Random.Range(0f, _maxDeflectAngle), 0f);

        _moveDirection = deflection * newDirection;

        Move();

        if (gameObject.activeSelf)
            _changeDirectionCoroutine = StartCoroutine(ChangeDirectionWithInterval());
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
    }

    private IEnumerator ChangeDirectionWithInterval()
    {
        yield return new WaitForSeconds(_changeDirectionInterval);

        _moveDirection = GetRandomDirection();

        Move();
    }
}