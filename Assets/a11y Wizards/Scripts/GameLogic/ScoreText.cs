using System;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour {
    public TextMeshPro score;

    private void Update() {
        score.text = $"Score: {GameState.Instance.GetScore()}";
    }
}