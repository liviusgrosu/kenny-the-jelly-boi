using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    private PlayerSpawn spawn;
    private Player playerObj;

    private bool prepareDeathScreen, prepareNextLevel;

    public GameObject advanceLevelScreen, deathMenu, pauseMenu;
    public GameObject pauseButton;

    string nextSceneName;
    private string currentSceneName;

    public ScoreSystem scoreSys;
    public Text advanceLevelScoreText;
    public Transform goldenStarsParent;
    public Transform[] goldenStars; 
    bool startScoreDisplay;
    int starScore = 0;
    int maxScore = 0;
    float starRevealTime = 0.0f;

    public Transform playerMovementButtonParent;
    Transform[] playerMovementButtons;

    private void Start() {
        spawn = GameObject.Find("Player Spawn").GetComponent<PlayerSpawn>();
        playerObj = transform.parent.GetComponent<Player>();

        currentSceneName = SceneManager.GetActiveScene().name;

        CalculateMaxScore();

        goldenStars = new Transform[goldenStarsParent.childCount];

        for(int i = 0; i < goldenStars.Length; i++) {
            goldenStars[i] = goldenStarsParent.GetChild(i);
        }

        playerMovementButtons = new Transform[playerMovementButtonParent.childCount];

        for (int i = 0; i < playerMovementButtons.Length; i++) {
            playerMovementButtons[i] = playerMovementButtonParent.GetChild(i);
        }
    }

    private void Update() {
        if (playerObj.IsPlayerDead() && !prepareDeathScreen) {
            prepareDeathScreen = true;
            ShowDeathScreen();
        }

        if(playerObj.IsAdvancingLevel() && !prepareNextLevel) {
            nextSceneName = playerObj.GetNextLevelName();
            prepareNextLevel = true;
            ShowAdvanceLevelScreen();
        }
        if(startScoreDisplay) {
            starRevealTime += Time.deltaTime;

            if (starRevealTime >= 0.5f && starScore >= 1) goldenStars[0].gameObject.SetActive(true);
            if (starRevealTime >= 1.0f && starScore >= 2) goldenStars[1].gameObject.SetActive(true);
            if (starRevealTime >= 1.5f && starScore == 3) goldenStars[2].gameObject.SetActive(true);
        }
    }

    public void ShowDeathScreen() {
        scoreSys.ToggleScoreText(false);
        deathMenu.SetActive(true);
        ToggleMovementButtons(false);
    }

    public void ShowAdvanceLevelScreen() {
        advanceLevelScreen.gameObject.SetActive(true);
        advanceLevelScoreText.text = "Score: " + scoreSys.GetCurrentScore();
        CalculateStarScore();
        startScoreDisplay = true;
        scoreSys.ToggleScoreText(false);
        ToggleMovementButtons(false);
        pauseButton.SetActive(false);

        SaveSystem.SaveLevel(gameObject.GetComponent<MenuSystem>());

    }

    public void OnRestartLevelPress() {
        SceneManager.LoadScene(currentSceneName);
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    public void OnAdvanceLevelPress() {
        if (nextSceneName == "") {
            OnQuitPress();
            return;
        }

        SceneManager.LoadScene(nextSceneName);
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    public void OnQuitPress() {
        SceneManager.LoadScene("TitleScreen");
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    public void OnContinuePress() {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);

        ToggleMovementButtons(true);
        scoreSys.ToggleScoreText(true);

        Time.timeScale = 1;
    }

    public void OnPausePress() {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);

        ToggleMovementButtons(false);
        scoreSys.ToggleScoreText(false);
        Time.timeScale = 0;
    }

    public string GetLevelName()
    {
        return currentSceneName;
    }

    void ToggleMovementButtons(bool state) {
        foreach(Transform button in playerMovementButtons) {
            button.gameObject.SetActive(state);
        }
    }

    void CalculateStarScore() {
        float starPerc = (float)scoreSys.GetCurrentScore() / (float)GetMaxScore();

        if (starPerc >= 0.3333) starScore = 1;
        if (starPerc >= 0.6666) starScore = 2;
        if (starPerc >= 0.9999) starScore = 3;
    }

    public int GetStarValue() {
        return starScore;
    }

    int GetMaxScore() {
        return maxScore;
    }

    public void CalculateMaxScore() {

        if(GameObject.Find("Gems") == null) {
            maxScore = 0;
            return;
        }

        Transform gemParent = GameObject.Find("Gems").transform;

        for (int i = 0; i < gemParent.transform.childCount; i++) {
            maxScore += gemParent.GetChild(i).GetComponent<ScoreContainer>().scoreValue;
        }
    }
}
