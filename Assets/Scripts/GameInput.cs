using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : IInput {
    public float Throttle {
        get {
            if (Input.GetMouseButton(0)) 
                return Input.mousePosition.x > Screen.width / 2f ? 1f : -1f;
            return Input.GetAxis("Horizontal");
        }
    }
}
