using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    private int level;
    private int points;

    public Experience()
    {
        level = 1;
        points = 0;
    }

    public void AddPoints(int amount)
    {
        points += amount;
        int newLevel = points / 100 + 1;
        if (newLevel != level)
        {
            level = newLevel;
            // Trigger level up event
        }
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetPoints()
    {
        return points;
    }
}

