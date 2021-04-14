using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAt2d : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float angleOffset = 0f;
    private Transform _transform = null;

    private void Start() {
        _transform = transform;
    }

    private void LateUpdate() {
#if UNITY_EDITOR
        if (target == null)
            return;
#endif
        var dir = transform.position - target.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _transform.eulerAngles = Vector3.forward * (angle - angleOffset);
    }
}
