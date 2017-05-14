using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    int key;
    int cookie;
    int potion;
    public GameObject notificationBar;
    public GameObject paused;
    public GameObject Button;
    public Button myButton;
    public Text keyNum;
    public Text potionNum;
    public Text cookieNum;
    public Text noti;
    public GameObject Hero;

    // Use this for initialization
    void Start () {
        notificationBar = GameObject.Find("notificationBar");
        notificationBar.SetActive(false);
        paused = GameObject.Find("PauseCanvas");
        paused.SetActive(false);
        cookie = 0;
        potion = 0;
        key = 0;
    }

	
	// Update is called once per frame
	void Update () {
        cookie = Hero.gameObject.GetComponent<Player>().cookiesHeld;
        key = Hero.gameObject.GetComponent<Player>().keysHeld;
        potion = Hero.gameObject.GetComponent<Player>().potionsHeld;
        keyNum.text = "x " + key;
        potionNum.text = "x " + potion;
        cookieNum.text = "x " + cookie;
    }

    public void addKey() {
        key ++;
        SendNotification("You get a key.");
    }

    public void addCookie()
    {
        cookie++;
        SendNotification("You get a cookie.");
    }

    public void addpotion()
    {
        potion++;
        SendNotification("You get a potion.");
    }

    public void resetKey()
    {
        cookie = 0;
        potion = 0;
        key = 0;
    }

    private void SendNotification(string str)
    {
        noti.text = str;
        notificationBar.SetActive(true);
        StartCoroutine(Wait(3));
        
    }

    public void hidePause()
    {
        paused.SetActive(false);
    }

    public void showPause() {
        paused.SetActive(true);
    }
 

    public void backToMenu() {
        SceneManager.LoadSceneAsync("TitleScreen");
    }
    IEnumerator Wait(float duration)
    {
        Debug.Log("Start Wait() function. The time is: " + Time.time);
        Debug.Log("Float duration = " + duration);
        yield return new WaitForSeconds(duration);   //Wait
        notificationBar.SetActive(false);
        Debug.Log("End Wait() function and the time is: " + Time.time);
    }
}
