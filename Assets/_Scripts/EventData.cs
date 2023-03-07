using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData : MonoBehaviour
{

}

public struct LevelEndStats
{
    public int enemiesKilled;
    public int distanceTravelled;
    public LevelEndStats(int enemiesKilled, int distanceTravelled)
    {
        this.enemiesKilled = enemiesKilled;
        this.distanceTravelled = distanceTravelled;
    }
}
