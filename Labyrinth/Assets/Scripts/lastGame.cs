using UnityEngine;
using System.Collections;

[System.Serializable]
public class LastGame 
{
    public Loc[,] Maze;
    public Vector3 playerPosition;
    public Vector3[] keysPosition;
    public float[] keyTime;
    public float timeLeft;
    public float keyLeft;
}
