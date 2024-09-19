using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public MSound[] musicsounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic("Background");
    }

    public void PlayMusic(String name) 
    {
        MSound s = Array.Find(musicsounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else 
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void StopMusic() 
    {
        if (musicSource != null) 
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(string name) 
    {
        MSound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sfx Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic() 
    {
        if (musicSource != null) 
        {
            musicSource.mute = !musicSource.mute;
        }
    }

    public void MusicVolume(float volume) 
    {
        musicSource.volume = volume;
    }
    public float GetMusicVolume()
    {
        return musicSource.volume; // Return the current volume of the AudioSource
    }

    public void SFXVolume(float volume) 
    {
        sfxSource.volume = volume;
    }
    public float GetSFXVolume()
    {
        return sfxSource.volume; // Return the current volume of the AudioSource
    }

}
