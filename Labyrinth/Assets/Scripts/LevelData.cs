using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelData
{
    //Gameplay:
    public int levelIndex;
    public int countOfKeys;
    public int countOfKeysDuringGame;
    public float totalTime;
    public float keyTime;
    //Achievements:
    public float timeFor2star;
    public float timeFor3star;
    //Player results:
    public float timeLeft;
    public int star;
}
