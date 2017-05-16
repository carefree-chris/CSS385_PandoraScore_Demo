using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PalyGame()
    {
        float fadetime = GameObject.Find("TitleScream").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        SceneManager.LoadScene("Main");
    }

    public void startGame()
    {
        StartCoroutine(routine:PalyGame());
    }

    IEnumerator EndGame()
    {
        float fadetime = GameObject.Find("TitleScream").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        Application.Quit();
    }

    public void QuitGame()
    {
        StartCoroutine(routine: EndGame());
    }
}
