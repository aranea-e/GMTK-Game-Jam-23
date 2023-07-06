using UnityEngine;
using System;
using UnityEngine.Audio;

    /*
    *   Code block taken from: 
    *   https://www.youtube.com/watch?v=6OT43pvUyfY
    */

public class audioManager : MonoBehaviour {
    public sound[] sounds;

    void Awake() {
        foreach (sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start() {
        play("music");
    }

    public void play (string name) {
        sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
