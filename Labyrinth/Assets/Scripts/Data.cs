using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Data 
{
    public static List<LevelData> levels;
    public static LastGame lastGame;
    public static bool IsContinue;
    public static int campaignLevel;
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/LastLevel.dat");
        bf.Serialize(file, lastGame);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/LastLevel.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/LastLevel.dat", FileMode.Open);
            lastGame = (LastGame)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void SaveLevels()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Levels.dat");
        bf.Serialize(file, levels);
        file.Close();
    }

    public static void LoadLevels()
    {
        if (File.Exists(Application.persistentDataPath + "/Levels.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Levels.dat", FileMode.Open);
            levels = (List<LevelData>)bf.Deserialize(file);
            file.Close();
        }
    }
}
