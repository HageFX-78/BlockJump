using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [NonReorderable]
    public Sound[] BGM;
    [NonReorderable]
    public Sound[] SoundEffects;
    public static AudioManager amInstance;
    void Awake()
    {
        if (amInstance==null)
        {
            amInstance = this;         
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        foreach (Sound s in BGM)
        {
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip = s.clip;
            s.src.volume = s.volume;
            s.src.pitch = s.pitch;
        }
        foreach (Sound s in SoundEffects)
        {
            s.src = gameObject.AddComponent<AudioSource>();
            s.src.clip = s.clip;
            s.src.volume = s.volume;
            s.src.pitch = s.pitch;
        }
    }
    public void plyBGM(string name)
    {
        Sound s = Array.Find(BGM, sound => sound.name == name);
        s.src.loop = true;
        s.src.Play();
    }
    public void plySF(string name)
    {
        Sound s = Array.Find(SoundEffects, sound => sound.name == name);
        s.src.Play();
    }
    public void stopBGM(string name)
    {
        Sound s = Array.Find(BGM, sound => sound.name == name);
        s.src.Stop();
    }
    public void stopSF(string name)
    {
        Sound s = Array.Find(SoundEffects, sound => sound.name == name);
        s.src.Stop();
    }
    public void stopAllSF()
    {
        foreach (Sound s in SoundEffects)
        {
            s.src.Stop();
        }
    }
    
}
