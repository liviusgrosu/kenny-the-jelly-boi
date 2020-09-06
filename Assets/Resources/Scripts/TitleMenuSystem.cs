using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuSystem : MonoBehaviour
{
    public Transform startingMenu, levelSelectMenu, optionSelectButton, optionSelectMenu, levelButtonsParent;
    public LevelButton[] levelButtons;
    
    // Start is called before the first frame update
    void Start()
    {
        levelButtons = new LevelButton[levelButtonsParent.childCount - 2];

        for (int i = 0; i < levelButtons.Length; i++) {
            levelButtons[i] = levelButtonsParent.GetChild(i).GetComponent<LevelButton>();
        }
        
        //LevelData data = SaveSystem.LoadLevel("World 1 - Level 2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevelMenuSelectClick()
    {
        startingMenu.gameObject.SetActive(false);
        levelButtonsParent.gameObject.SetActive(true);

        UpdateLevelStarScore();
    }

    public void OnLevelMenuBackClick()
    {
        startingMenu.gameObject.SetActive(true);
        levelButtonsParent.gameObject.SetActive(false);
    }

    public void OnOptionClick()
    {
        startingMenu.gameObject.SetActive(false);
        optionSelectMenu.gameObject.SetActive(true);
    }

        public void OnOptionBackClick()
    {
        startingMenu.gameObject.SetActive(true);
        optionSelectMenu.gameObject.SetActive(false);
    }

    public void OnLevelSelectClick(int levelIndex)
    {
        SceneManager.LoadScene(levelButtons[levelIndex - 1].levelName);
    }

    void UpdateLevelStarScore()
    {
        foreach(LevelButton button in levelButtons)
        {
            LevelData data = SaveSystem.LoadLevel(button.levelName);
            if(data != null) button.ToggleStarImages(data.starScore);
        }
    }
}
