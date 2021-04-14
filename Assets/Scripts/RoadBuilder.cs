using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadBuilder : MonoBehaviour {
    [SerializeField] private float targetDistance = 5f,
        startHeight = 0f,
        heightAmplitude = 4f;
    [SerializeField] private int maxLines = 50;

    private Vector2 _lastPoint = Vector2.zero;
    private Line.Pool _linePool;
    private IMotorbike _motorbike;
    private Queue<Line> _lines;
    
    [Inject]
    public void Construct(Line.Pool linePool, IMotorbike motorbike) {
        _linePool = linePool;
        _motorbike = motorbike;
        _lines = new Queue<Line>();
    }
    
    private void Update()
    {
        while (_lastPoint.x < _motorbike.PositionX + targetDistance) {
            var line = _linePool.Spawn();
            _lines.Enqueue(line);
            
            var pos = new Vector2(_lastPoint.x + 0.5f, GetHeight(_lastPoint.x + 0.5f));
            var dir = pos - _lastPoint;
            _lastPoint = line.Place(_lastPoint, Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg);
        }

        while (_lines.Count > maxLines) {
            var line = _lines.Dequeue();
            _linePool.Despawn(line);
        }
    }

    private float GetHeight(float x) {
        return Mathf.PerlinNoise(x * 0.23f, 0.123456f) * heightAmplitude + startHeight;
    }
}
