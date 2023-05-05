using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * AudioManager.cs
 *
 * used for playing music and sound effects
 *
 * the music and sfx are stored in Resources/Audio
 *
 * audio setting is set here instead of GameManager
 */

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;
	
	private float musicVolume=0.2f;
	private float sfxVolume=0.2f;

	public void PlayMusic(string clipName, bool loop=true) {
		musicSource.clip = Resources.Load<AudioClip>("Audio/"+clipName);
		musicSource.volume = musicVolume;
        musicSource.loop = loop;
		musicSource.Play();
    }
    public void PlayMusic(string clipName) {
	    musicSource.clip = Resources.Load<AudioClip>("Audio/"+clipName);
	    musicSource.volume = musicVolume;
	    // Debug.Log("musicVolume = " + musicVolume);
	    musicSource.loop = true;
	    musicSource.Play();
    }
    
	public void PlaySfx(string clipName, float pitch=1.0f) {
		sfxSource.clip =  Resources.Load<AudioClip>("Audio/"+clipName);
        sfxSource.volume = sfxVolume;
        sfxSource.pitch = pitch;
		sfxSource.Play();
	}
	public void PlaySfx(string clipName) {
		sfxSource.clip =  Resources.Load<AudioClip>("Audio/"+clipName);
		sfxSource.volume = sfxVolume;
		sfxSource.pitch = 1.0f;
		sfxSource.Play();
	}

	public void StopMusic() {
		musicSource.Stop();
	}

	public void SetMusicVolume(float value) {
		musicSource.Pause();
		musicSource.volume = value*1.0f;
		musicVolume = value*1.0f;
		musicSource.Play();
	}
    public void SetSfxVolume(float value) {
        sfxVolume = value*1.0f;
    }
}