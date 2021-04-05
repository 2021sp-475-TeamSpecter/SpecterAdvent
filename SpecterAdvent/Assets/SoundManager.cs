using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioClip gunShot, swordSlash, jumpEffect;
    static AudioSource audioSrc;
    void Start()
    {
    	gunShot = Resources.Load<AudioClip> ("GunShot");
    	jumpEffect = Resources.Load<AudioClip> ("JumpSound");
    	swordSlash = Resources.Load<AudioClip> ("SwordSlash");
    	audioSrc = GetComponent<AudioSource> ();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
// Sound clips can't be found for some reason, d
    public static void PlaySound (string clip)
    {
    	switch (clip)
    	{
    		case "GunShot":
    			audioSrc.PlayOneShot(gunShot);
    			break;
    		case "JumpSound":
    			audioSrc.PlayOneShot(jumpEffect);
    			break;
    		case "SwordSlash":
    			audioSrc.PlayOneShot(swordSlash);
    			break;
    	}
    }
}
