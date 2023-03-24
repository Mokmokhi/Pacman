using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	private AudioSource musicSource;
	private AudioSource sfxSource;
	
	private float musicVolume = 1.0f;
	private float sfxVolume = 1.0f;

	public void PlayMusic(string clipName, bool loop=true) {
		musicSource.clip = Resources.Load<AudioClip>("Sound/"+clipName);
		musicSource.volume = musicVolume;
        musicSource.loop = loop;
		musicSource.Play();
    }
    public void PlayMusic(string clipName) {
	    musicSource.clip = Resources.Load<AudioClip>("Sound/"+clipName);
	    musicSource.volume = musicVolume;
	    musicSource.loop = true;
	    musicSource.Play();
    }
    
	public void PlaySfx(string clipName, float pitch=1.0f) {
		sfxSource.clip =  Resources.Load<AudioClip>("Sound/"+clipName);
        sfxSource.volume = sfxVolume;
        sfxSource.pitch = pitch;
		sfxSource.Play();
	}
	public void PlaySfx(string clipName) {
		sfxSource.clip =  Resources.Load<AudioClip>("Sound/"+clipName);
		sfxSource.volume = sfxVolume;
		sfxSource.pitch = 1.0f;
		sfxSource.Play();
	}

	public void StopMusic() {
		musicSource.Stop();
	}

	private void SetMusicVolume(float value) {
		musicVolume = value;
    }
    private void SetSfxVolume(float value) {
        sfxVolume = value;
    }
}