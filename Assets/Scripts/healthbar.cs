using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class healthbar : MonoBehaviour {

    public Image currentHealthbar;

    private float hitpoint = 150;
    private float maxHitpoint = 150;

    // Use this for initialization
    void Start () {
        UpdateHealthbar();
	}
	
	// Update is called once per frame
	void UpdateHealthbar () {
        float ratio = hitpoint / maxHitpoint;
        currentHealthbar.rectTransform.localScale = new Vector3(ratio, 1, 1);
	}

    public void FullHealth() {
        hitpoint = 150;
        UpdateHealthbar();
    }

    public void TakeDemage(float damage) {
        hitpoint -= damage;
        if (hitpoint < 0) {
            hitpoint = 0;
            Debug.Log("Dead!");
            SceneManager.LoadSceneAsync("Over");
      }
        UpdateHealthbar();
    }

    public void HealDamage(float heal)
    {
        hitpoint += heal;
        if (hitpoint > maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }
        UpdateHealthbar();
    }
}
