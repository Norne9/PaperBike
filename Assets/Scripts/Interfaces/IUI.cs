using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IUI {
    public float Score { set; }
    public void ShowWheelie();
    public void ShowGameOver(UnityAction onDone);
}
