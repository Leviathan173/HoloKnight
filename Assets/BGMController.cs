using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioClip dft;
    [SerializeField] AudioClip safeArea;
    [SerializeField] AudioClip BossFight;
    public void IntoDefault() {
        //BGM.clip = dft;
        //BGM.Play();
        StartCoroutine(Switch(dft));
    }
    public void IntoSafeArea() {
        //BGM.clip = safeArea;
        //BGM.Play();
        StartCoroutine(Switch(safeArea));
    }
    public void IntoBossFight() {
        //BGM.clip = BossFight;
        //BGM.Play();
        StartCoroutine(Switch(BossFight));
    }
    public void OnSliderValueChanged(Slider slider) {
        BGM.volume = slider.value;
    }
    IEnumerator Switch(AudioClip clip) {
        float originVolume = BGM.volume;
        while (BGM.volume > 0) {
            BGM.volume -= 0.01f;
            yield return null;
        }
        BGM.clip = clip;
        BGM.Play();
        while(BGM.volume < originVolume) {
            BGM.volume += 0.01f;
            yield return null;
        }
        BGM.volume = originVolume;
    }
}
