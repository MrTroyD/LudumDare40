using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    [Range(0, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1;
    public AudioClip clip;

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
        this._source.Play();
    }
}

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Sound[] _sounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
