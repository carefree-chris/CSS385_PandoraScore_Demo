using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Followed the official unity tutorial for making a sound manager, then modified it. -Chris
public class SoundManager : MonoBehaviour {

    
    public Player player;
    //private MonsterAI monster;


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

    [SerializeField] private AudioSource playerMovement;
    [SerializeField] private AudioClip fastWalk;
    [SerializeField] private AudioClip normalWalk;

    private AudioClip monsterPatrolClip;

    private float lowPitchRange = .95f;
    private float highPitchRange = 1.05f;

    private bool playerIsMoving = false;

    void Awake()
    {
      //DontDestroyOnLoad(gameObject);
       //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       //monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>();

       
        
    }

   private void Update()
   {

      
      if ((player.getMoveState().ToString() == "run" || player.getMoveState().ToString() == "walk") && !playerIsMoving)
      {
         playerIsMoving = true;
         playFootsteps();
      }

      if (!(player.getMoveState().ToString() == "run" || player.getMoveState().ToString() == "walk") && playerIsMoving)
      {
         playerIsMoving = false;
         stopFootsteps();
      }
   }

   public void playFootsteps()
   {
      playerMovement.loop = true;

      if (player.getMoveState().ToString() == "walk")
      {
         playerMovement.clip = normalWalk;
         playerMovement.Play();
      } else if (player.getMoveState().ToString() == "run")
      {
         playerMovement.clip = fastWalk;
         playerMovement.Play();

         //TODO: fix bug where only slow footsteps make sound.
      }
   }

   public void stopFootsteps()
   {
      playerMovement.loop = false;
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

    /// PropogateSound takes in a location of a sound source, a sound distance (how far the
    /// sound will travel), and a sound strength. The location is usually (if not always)
    /// the player, the range determines whether it reaches the monster, and the strength
    /// is how large of an area the monster will search for the player.
    /// As an integer, soundStrength indicates the dimensions of the area that will be searched,
    /// in terms of number of rooms, 1 being the strongest, as that means the monster will hone
    /// in directly on the room that the player is in. 7 being the weakest, as that means the monster
    /// chooses a random point within the maze (we likely won't use that value, as then sound becomes
    /// useless).
    /// <param name="soundLocation"></param>
    /// <param name="soundRange"></param>
    /// <param name="soundStrength"></param>
    public void PropogateSound(Vector2 soundLocation, float soundRange, int soundStrength)
    {

    }

    
}
