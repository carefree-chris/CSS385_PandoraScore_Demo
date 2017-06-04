using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

    GlobalVariables globalData;


	// Use this for initialization
	void Start () {
        globalData = GameObject.FindGameObjectWithTag("GlobalStorage").GetComponent<GlobalVariables>();


        if (globalData == false)
        {
            Debug.Log("Error: Can't find GlobalStorage object in scene.");
        }
        else
        {
            GetComponent<Text>().text = globalData.gold.ToString();
        }
        

    }
}
