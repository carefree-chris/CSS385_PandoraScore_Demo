using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public int maxLives = 3;
    public int currentLives = 3;
    public Text resetText;
    public Image resetImage;
    private bool resetInProgress = false;
    public HashSet<int> visitedRoom;
    //SoundManager soundManager;
    RoomManager r;

    bool showMap;
    int key;
    int cookie;
    int potion;
    int gold;

    public GameObject map;
    string mapContent;
    public GameObject notificationBar;
    public GameObject paused;
    public GameObject Button;
    public Button myButton;
    public Text keyNum;
    public Text potionNum;
    public Text cookieNum;
    public Text noti;
    public Text onMap;
    public GameObject Hero;
    public Text goldAmount;
    public Image key0;
    public Image key1;
    public Image key2;
    public Image key3;
    public Image key4;
    public Image key5;

    void Awake()
    {
        //soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        resetText.text = "";
        resetImage.enabled = false;
        
    }

    // Use this for initialization
    void Start () {
        //resetScreen.gameObject.SetActive(false);
        visitedRoom = new HashSet<int>();
        showMap = false;
        map = GameObject.Find("Map");
        map.SetActive(false);
        notificationBar = GameObject.Find("notificationBar");
        notificationBar.SetActive(false);
        paused = GameObject.Find("PauseCanvas");
        paused.SetActive(false);
        cookie = 0;
        potion = 0;
        key = 0;
        updateGold(0681);
        key0.enabled = false;

        r = GameObject.Find("RoomManager").GetComponent<RoomManager>();

        updateMap();

    }

	
	// Update is called once per frame
	void Update () {
       // cookie = Hero.gameObject.GetComponent<Player>().cookiesHeld;
       // key = Hero.gameObject.GetComponent<Player>().keysHeld;
        //potion = Hero.gameObject.GetComponent<Player>().potionsHeld;
       
        potionNum.text = "x" + Hero.gameObject.GetComponent<Player>().potionsHeld;
        cookieNum.text = "x" + Hero.gameObject.GetComponent<Player>().cookiesHeld;
        gold = Hero.gameObject.GetComponent<Player>().goldHeld;

        

        goldAmount.text = gold.ToString();
        key = Hero.gameObject.GetComponent<Player>().keysHeld;

        updateKeyDisplay();
        
        onMap.text = mapContent;
        updateMap();
        if (Input.GetKeyDown("m")) {
            showMap = !showMap;
            pressM();
        }
    }

    public void addKey() {
        key = Hero.gameObject.GetComponent<Player>().keysHeld;
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

    // ues a int to update the amount of gold in UI
    public void updateGold(int amount)
    {
        gold = amount;
    }

    public void setKeyStatus(int n) {
        key = n;
    }

    void updateKeyDisplay(){
        if (key == 0) {
            key0.enabled = true;
            key1.enabled = false;
            key2.enabled = false;
            key3.enabled = false;
            key4.enabled = false;
            key5.enabled = false;
        }else if (key == 1) {
            key0.enabled = false;
            key1.enabled = true;
            key2.enabled = false;
            key3.enabled = false;
            key4.enabled = false;
            key5.enabled = false;
        } else if (key == 2) {
            key0.enabled = false;
            key1.enabled = false;
            key2.enabled = true;
            key3.enabled = false;
            key4.enabled = false;
            key5.enabled = false;
        } else if (key == 3) {
            key0.enabled = false;
            key1.enabled = false;
            key2.enabled = false;
            key3.enabled = true;
            key4.enabled = false;
            key5.enabled = false;
        } else if (key == 4) {
            key0.enabled = false;
            key1.enabled = false;
            key2.enabled = false;
            key3.enabled = false;
            key4.enabled = true;
            key5.enabled = false;
        } else {
            key0.enabled = false;
            key1.enabled = false;
            key2.enabled = false;
            key3.enabled = false;
            key4.enabled = false;
            key5.enabled = true;
        }
    }

    private int HashAB(int a, int b) {
        return (a + b) * (a + b + 1) / 2 + a;
    }

    public void pressM() {
        if (showMap) {
            updateMap();
            map.SetActive(true);

        } else {
            map.SetActive(false);
        }
    }

    public void updateMap() {
        int tmp = 0;
        int tmpV = 0;
        int row = r.getRows();
        int col = r.getCollumns();
        int actRow = r.getActiveRow();
        int actCol = r.getActiveCol();
        mapContent = "";
        for (int i = 0; i < row; i++) {
            for (int j = 0; j < col; j++) {
                tmp = HashAB(i, j);
                if (i == actRow && j == actCol) {
                    mapContent += "A";
                    visitedRoom.Add(HashAB(i,j));
                } else if (visitedRoom.Contains(tmp)) {
                    mapContent += "o";
                }
                else {
                    mapContent += "k";
                }
            }
            mapContent += "\n";
        }
    }



    IEnumerator BacktoMenu()
    {
        float fadetime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        SceneManager.LoadScene("TitleScreen");
    }

    public void backToMenu() {
        StartCoroutine(routine: BacktoMenu());
    }

    IEnumerator Wait(float duration)
    {
        Debug.Log("Start Wait() function. The time is: " + Time.time);
        Debug.Log("Float duration = " + duration);
        yield return new WaitForSeconds(duration);   //Wait
        notificationBar.SetActive(false);
        Debug.Log("End Wait() function and the time is: " + Time.time);
    }

    public void ResetGame()
    {
        
        if (resetInProgress)
        {
            return;
        }
        else
        {
            resetInProgress = true;
        }

        if (currentLives <= 0)
        {
            Debug.Log("Dead!");
            currentLives--;
            SceneManager.LoadSceneAsync("Over");
        } else
        {
            
            StartCoroutine(ResetScreenFade());
        }
        
    }

    public void WinGame()
    {
        StartCoroutine(WinGameFade());
    }

    private IEnumerator WinGameFade()
    {
        float fadetime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        SceneManager.LoadSceneAsync("Win");
    }

    private IEnumerator ResetScreenFade()
    {
        //string losingText = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES BEFORE THE DOOR IS SEALED.";
       
        float fadetime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        resetImage.enabled = true;
        fadetime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(-1);

        resetText.text = "THE MONSTER CAUGHT YOU.\n\n";
        yield return new WaitForSeconds(1.5f);

        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().ResetActors();

        yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU";
        yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE";
        yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives;
        yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES";
        yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES LEFT";
            yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES LEFT BEFORE";
        yield return new WaitForSeconds(0.2f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES BEFORE THE DOOR";
        yield return new WaitForSeconds(0.4f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES BEFORE THE DOOR IS";
        yield return new WaitForSeconds(0.4f);
        resetText.text = "THE MONSTER CAUGHT YOU.\n\nYOU HAVE " + currentLives + " CHANCES BEFORE THE DOOR IS SEALED.";
        yield return new WaitForSeconds(1);
        fadetime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadetime);
        resetImage.enabled = false;
        resetText.text = "";
        fadetime = GameObject.Find("MainCamera").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadetime);
        currentLives--;
        resetInProgress = false;

    }
}
