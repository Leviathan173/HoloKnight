using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] SettingPopup Setting;
    [SerializeField] Canvas MainCanvas;
    [SerializeField] AudioSource BGM;

    public void OnExitGame() {
        Application.Quit();
    }

    public void OnNewGame() {
        // TODO 异步加载
        SceneManager.LoadScene("Tutorial");
    }

    public void OnLoadGame() {
        // TODO 加载游戏
        // 需要一个弹出界面
    }

    public void LoadGame(string name) {
        // TODO 加载特定的一个存档
        // 传的参数没有决定
    }

    public void OnSettingOpen() {
        Setting.Open();
        MainCanvas.gameObject.SetActive(false);
    }

    public void OnSettingExit() {
        Setting.Close();
        MainCanvas.gameObject.SetActive(true);
    }

    public void OnSliderValueChanged(Slider slider) {
        if (slider.name.Contains("SE")) {

        } else {
            BGM.volume = slider.value;
        }
    }
}
