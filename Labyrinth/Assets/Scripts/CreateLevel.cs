using UnityEngine;
using System.Collections;

public class CreateLevel:MonoBehaviour 
{
    void Start()
    {
        //CreateFirstLevel();
        //Data.LoadLevels();
        //Debug.Log(Data.levels.Count);
        ModifyLevel();
    }
    public void CreateFirstLevel()
    {
        Data.levels = new System.Collections.Generic.List<LevelData>();
        LevelData l = new LevelData();
        l.levelIndex = 1;
        l.countOfKeys = 1;
        l.countOfKeysDuringGame = 1;
        l.totalTime = 60;
        l.keyTime = 10;

        Data.levels.Add(l);
        Data.SaveLevels();
    }

    public void AddNewLevel()
    {
        Data.LoadLevels();
        LevelData l = new LevelData();
        l.levelIndex = 3;
        l.countOfKeys = 4;
        l.countOfKeysDuringGame = 3;
        l.totalTime = 60;
        l.keyTime = 10;
        l.timeFor2star = 50;
        l.timeFor3star = 60;

        Data.levels.Add(l);
        Data.SaveLevels();
    }
    public void ModifyLevel()
    {
        Data.LoadLevels();
        LevelData l = Data.levels[1];
        //l.levelIndex = 2;
        l.countOfKeys = 2;
        l.countOfKeysDuringGame = 1;
        //l.totalTime = 60;
        //l.keyTime = 10;
        //l.timeFor2star = 40;
        //l.timeFor3star = 50;
        Data.SaveLevels();
    }
}
