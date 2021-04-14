using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Line : MonoBehaviour {
    public float Lenght => 0.55f;
    
    [SerializeField] private Sprite[] variants;
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private SpriteRenderer _renderer;

    [Inject]
    public void Construct() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }
    
    public Vector2 Place(Vector2 position, float rotation) {
        _renderer.sprite = variants[Random.Range(0, variants.Length)];
        _transform.position = position;
        _transform.localEulerAngles = Vector3.forward * rotation;
        
        _transform.localScale = new Vector3(1f, 0f, 1f);
        _transform.DOScale(Vector3.one, 0.2f);
        
        return position + (Vector2)_transform.up * Lenght;
    }
    
    public class Pool: MonoMemoryPool<Line> { }
}
