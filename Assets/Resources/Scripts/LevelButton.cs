using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public string levelName;
    Transform[] stars;

    void Awake()
    {
        stars = new Transform[transform.childCount - 4];
        for(int i = 0; i < transform.childCount - 4; i++)
        {
            stars[i] = transform.GetChild(i + 3).transform;
        }
    }

    public void ToggleStarImages(int starScore)
    {
        if(stars == null) print("is null");
        for(int i = 0; i < starScore; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }
}
