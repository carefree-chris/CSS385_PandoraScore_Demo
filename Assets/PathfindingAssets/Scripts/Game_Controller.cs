using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour {
    

	void Update () {
        if (Input.GetKeyDown("escape"))
        {
             SceneManager.LoadSceneAsync("Menu");
        }


    }
}
