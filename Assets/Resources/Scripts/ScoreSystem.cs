using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public MasterAudioSystem audioSource;

    public Text scoreText;
    int score = 0;

    public void AddToScore(int amount) {
        score += amount;
        ChangeScoreText();
        audioSource.PlayOtherSounds("Gem Pickup");
    }

    void ChangeScoreText() {
        scoreText.text = "Score: " + score;
    }

    public int GetCurrentScore() {
        return score;
    }

    public void ToggleScoreText(bool state) {
        scoreText.enabled = state;
    }
}
