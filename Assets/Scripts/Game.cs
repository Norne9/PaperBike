using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Game : ITickable, IGame {
    private readonly IMotorbike _motorbike;
    private readonly IUI _ui;

    public Game(IMotorbike motorbike, IUI ui) {
        _motorbike = motorbike;
        _ui = ui;
    }

    public void Tick() {
        _ui.Score = _motorbike.Distance;
    }

    public void OnBikeDead() {
        _ui.ShowGameOver(() => {
            _motorbike.Respawn();
        });
    }

    public void OnWheelie() {
        _ui.ShowWheelie();
    }
}
