using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour {
    [SerializeField] private Transform target;
    private void Update()
    {
        transform.position = new Vector3(Mathf.Floor(target.position.x / 10f) * 10f,
            Mathf.Floor(target.position.y / 10f) * 10f, transform.position.z);
    }
}
