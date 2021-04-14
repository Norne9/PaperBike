using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelJoint2D))]
public class Wheel : MonoBehaviour {
    public bool OnGround => _groundCounter > 0;

    private int _groundCounter = 0;
    private WheelJoint2D _wheel;
    private bool _wheelSet = false;

    public void SetMotor(bool on, float speed) {
        if (!_wheelSet) {
            _wheel = GetComponent<WheelJoint2D>();
            _wheelSet = true;
        }
        _wheel.useMotor = on;
        var motor = _wheel.motor;
        motor.motorSpeed = speed;
        _wheel.motor = motor;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            _groundCounter += 1;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            _groundCounter -= 1;
        }
    }
}
