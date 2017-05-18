using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Followed the official unity tutorial for making a sound manager, then modified it. -Chris
public class SoundManager : MonoBehaviour {

    
    //private GameObject player;
    //private GameObject monster;


    //Ambient effects and music
    [SerializeField] private AudioSource efxSource;
    [SerializeField] private AudioSource musicSource;


    [SerializeField] public AudioClip receiveGold;
    [SerializeField] public AudioClip receiveKey;
    [SerializeField] public AudioClip receivePotion;
    [SerializeField] public AudioClip receiveCookie;
    [SerializeField] public AudioClip[] openObject;

    [SerializeField] public AudioClip winGame;
    [SerializeField] public AudioClip loseGame;

    private AudioClip monsterPatrolClip;

    private float lowPitchRange = .95f;
    private float highPitchRange = 1.05f;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //player = GameObject.FindGameObjectWithTag("Player");
        //monster = GameObject.FindGameObjectWithTag("Monster");

       
        
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlaySingleOnLocation(AudioClip clip, Vector3 location, float volume)
    {
        

        AudioSource.PlayClipAtPoint(clip, location, volume);
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    
}
