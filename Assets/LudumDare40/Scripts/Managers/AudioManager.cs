using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum Category
    { 
        SoundFX,
        Background
    }

    public string name;

    [Range(0, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1;
    public AudioClip clip;
    public Category category;
    public bool loop;

    private AudioSource _source;

    public void SetSource(AudioSource source)
    {
        this._source = source;
        this._source.clip = this.clip;
    }
    
    public void Play()
    {
        this._source.volume = this.volume;
        this._source.pitch = this.pitch;
        this._source.loop = this.loop;
        this._source.Play();
    }

    public void Stop()
    {
        this._source.Stop();
    }
}

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Sound[] _sounds;

     void Awake()
    {
        if (AudioManager.instance != null)
        {
            Debug.LogError("Multiple instances of Sound Manager.");
            return;
        }

        AudioManager.instance = this;
    }

    // Use this for initialization
    void Start () {

        

		for (int i = 0; i < this._sounds.Length; i++)
        {
            GameObject go = new GameObject("Sound_" + i + "_" + this._sounds[i].name);
            go.transform.parent = this.transform;
            this._sounds[i].SetSource(go.AddComponent<AudioSource>());


        }
	}

    public void StopSound(string soundName)
    {
        for (int i = 0; i < this._sounds.Length; i++)
        {
            if (this._sounds[i].name == soundName)
            {
                this._sounds[i].Stop();
                
            }
        }
        
    }
	
    public void PlaySound (string soundName)
    {
        for (int i = 0; i < this._sounds.Length; i++)
        {
            if (this._sounds[i].name == soundName)
            {
                this._sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("Sound effect:" + soundName + " not found");
    }

    public void StopSoundtrack()
    {
        for (int i = 0; i < this._sounds.Length; i++)
        {
            if (this._sounds[i].category == Sound.Category.Background)
            {
                this._sounds[i].Stop();
            }
        }
    }
}
