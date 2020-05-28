using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] public BGMController bgm;
    [SerializeField] public AudioSource audio;
    [SerializeField] public AudioSource[] audios;
    [SerializeField] public AudioClip death_skeleton;
    [SerializeField] public AudioClip death_slime;
    [SerializeField] public AudioClip death_human;
    [SerializeField] public AudioClip equip;
    [SerializeField] public AudioClip coin;
    public void Startup() {
        status = ManagerStatus.Initializing;
        print("starting audio manager");
        if(audio == null) {
            Debug.LogError("audio source not found");
        }
        
        status = ManagerStatus.Started;
    }
    public void PlayClipOneShot(AudioClip clip) {
        if(clip != null) {
            audio.PlayOneShot(clip);
        } else {
            Debug.LogError("clip is null");
        }
    }
    public void OnSESliderValueChanged(Slider slider) {
        float value = slider.value;
        foreach(var audio in audios) {
            if(audio != null) {
                audio.volume = value;
            }
        }
    }
}
