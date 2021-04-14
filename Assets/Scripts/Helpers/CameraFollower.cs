using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollower : MonoBehaviour {
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float toleration = 1f;
    private Transform _transform;

    private void Start() {
        _transform = transform;
    }

    private void Update() {
#if UNITY_EDITOR
        if (target == null)
            return;
#endif
        var targetPosition = target.position + offset;
        var diff = Vector3.ClampMagnitude(_transform.position - targetPosition, toleration);
        _transform.position = diff + targetPosition;
    }
}
