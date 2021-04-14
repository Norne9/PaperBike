using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotorbike {
    public float Distance { get; }
    public float PositionX { get; }
    public void Respawn();
}
