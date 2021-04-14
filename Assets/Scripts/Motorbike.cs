using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Motorbike : MonoBehaviour, IMotorbike {
    // Props
    public float Distance { get; private set; } = 0f;
    public float PositionX => transform.position.x;

    // Settings
    [SerializeField] private float maxSpeed = 1000f,
        accelSpeed = 500f,
        breakSpeed = 300f,
        autoBrake = 10f,
        upperForce = 50f,
        wheelieTime = 1f,
        angularVelocityCap = 200f;

    [SerializeField] private Wheel rearWheel, frontWheel;
    [SerializeField] private Transform centerOfMass;

    // Vars
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private float _speed = 0f;
    private float _offGroudTime = 0f;
    private bool _active = false;
    private float _lastPosition = 0f;

    // DI
    private IInput _input;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(IInput input, SignalBus signalBus) {
        _signalBus = signalBus;
        _input = input;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        _transform = transform;
        Respawn();
    }

    private void Update() {
        if (!_active) return;

        var throttle = _input.Throttle;
        var motorOn = false;
        if (throttle < 0) {
            _speed = Mathf.MoveTowards(_speed, -maxSpeed / 10f,
                Time.smoothDeltaTime * breakSpeed);
            _rigidbody.angularVelocity -= upperForce * Time.smoothDeltaTime;
            motorOn = true;
        } else if (throttle > 0) {
            _speed = Mathf.MoveTowards(_speed, maxSpeed,
                Time.smoothDeltaTime * accelSpeed);
            _rigidbody.angularVelocity += upperForce * Time.smoothDeltaTime;
            motorOn = true;
        } else {
            _speed = Mathf.MoveTowards(_speed, 0,
                Time.smoothDeltaTime * autoBrake);
            motorOn = false;
        }

        _rigidbody.angularVelocity = Mathf.Clamp(_rigidbody.angularVelocity,
            -angularVelocityCap, angularVelocityCap);

        if (frontWheel.OnGround) {
            _offGroudTime = 0f;
        } else {
            if (_offGroudTime < wheelieTime && _offGroudTime + Time.smoothDeltaTime > wheelieTime) {
                _signalBus.Fire(new WheelieSignal());
            }

            _offGroudTime += Time.smoothDeltaTime;
        }

        Distance += Mathf.Max(0f, _transform.position.x - _lastPosition);
        _lastPosition = Mathf.Max(_lastPosition, _transform.position.x);

        SetWheels(motorOn, _speed);
    }

    private void SetWheels(bool active, float speed) {
        rearWheel.SetMotor(active, speed);
        frontWheel.SetMotor(active, speed);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!_active) return;
        if (_transform.up.y < -0.65f) {
            _active = false;
            _signalBus.Fire(new BikeDeadSignal());
            SetWheels(true, 0);
        }
    }

    public void Respawn() {
        SetWheels(true, 0);
        _speed = 0f;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        
        DOTween.Sequence()
            .Append(_transform.DOMove(new Vector3(_transform.position.x, 6f, 0f), 2.0f))
            .Insert(1.0f, _transform.DORotate(Vector3.zero, 1f, RotateMode.FastBeyond360))
            .OnComplete(() => {
                _offGroudTime = 0f;
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                _active = true;
                Distance = 0f;
                _lastPosition = _transform.position.x;
            });
    }
}
