﻿using UnityEngine;
using System.Collections;

public class ButtonNextLevel : MonoBehaviour
{
    public void NextLevelButton(int index)
    {
        Application.LoadLevel(index);
    }

    public void NextLevelButton(string levelName)
    {
        Application.LoadLevel(levelName);
        Time.timeScale = 1;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
}