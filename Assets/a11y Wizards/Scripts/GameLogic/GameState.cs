using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameState {
    private int total = 0;
    private int done = 0;

    public event System.Action GameEnded;
    
    public static GameState Instance { get; } = new GameState();

    GameState() {
        GameEnded += OnEnd;
    }

    public void AddTotalCounter() {
        total++;
    }

    public void AddCompletedCounter() {
        Debug.Log($"Done {done}/{total}");
        done++;
        if (done == total) {
            GameEnded?.Invoke();
        }
    }

    private void OnEnd() {
        Debug.Log($"Game completed {done}/{total}");
    }
}

public enum Category {
    Blue,
    Indigo,
    Violet,
}