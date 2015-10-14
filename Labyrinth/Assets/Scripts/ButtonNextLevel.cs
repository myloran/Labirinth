using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public void LoadLastGame(string levelName)
    {
        Application.LoadLevel(levelName);
        Time.timeScale = 1;
        Data.IsContinue = true;        
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void CampaignLevel(Button button)
    {
        Application.LoadLevel("Game");
        Time.timeScale = 1;
        Data.campaignLevel = int.Parse(button.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text);
    }

    public void CampaignNextLevel()
    {
        Application.LoadLevel("Game");
        Time.timeScale = 1;
        if (Data.campaignLevel <= 9)
            Data.campaignLevel = Data.campaignLevel + 1;
    }
}