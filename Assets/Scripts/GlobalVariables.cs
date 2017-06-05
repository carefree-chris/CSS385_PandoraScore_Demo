using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

   private int score;
    public bool debugModeOn = false;

   void Awake()
   {
      DontDestroyOnLoad(transform.gameObject);
   }

   public bool SetScore(int scoreUpdate)
   {
        if (scoreUpdate >= 0)
        {
            score = scoreUpdate;
            return true;
        }

        return false;
   }

   public bool IncreaseScore(int scoreUpdate)
    {
        if (scoreUpdate >= 0)
        {
            score += scoreUpdate;
            return true;
        }

        return false;
    }

    public int GetScore()
    {
        return score;
    }

    void Update()
    {
        if (debugModeOn && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Current score: " + score);
        }
    }
}
