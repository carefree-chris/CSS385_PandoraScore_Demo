using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public GameObject Hero;
    private int amount;
    private Text Number;

    private void Start()
    {
        Number = GetComponent<Text>();
        amount = Hero.gameObject.GetComponent<Player>().keysHeld;
    }

    private void Update()
    {
        amount = Hero.gameObject.GetComponent<Player>().keysHeld;
        Number.text = "penis";
    }
}
