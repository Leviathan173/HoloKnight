using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameManager
{
    [SerializeField] SettingPopup Setting;
    [SerializeField] MenuController Menu;
    [SerializeField] Canvas MainCanvas;
    [SerializeField] AudioSource BGM;
    [SerializeField] PlayerStatsViewController PlayerStatus;

    public ManagerStatus status { get; private set; }

    public float BGMVolume;
    public float SESoundVolume;

    [SerializeField] private EquipController equip;
    [SerializeField] private PlayerStatsDataController data;
    [SerializeField] private ItemListController item;

    public void Startup() {
        BGMVolume = 1;
        SESoundVolume = 1;
        equip = PlayerStatus.GetComponentInChildren<EquipController>();
        item = PlayerStatus.GetComponentInChildren<ItemListController>();
        data = PlayerStatus.GetComponentInChildren<PlayerStatsDataController>();
        print("equip:" + equip);
        print("item:" + item);
        print("data:" + data);
        status = ManagerStatus.Started;
    }

    //void Awake() {
    //    BGMVolume = 1;
    //    SESoundVolume = 1;
    //    equip = PlayerStatus.GetComponentInChildren<EquipController>();
    //    item = PlayerStatus.GetComponentInChildren<ItemListController>();
    //}

    public void OnExitGame() {
        Application.Quit();
    }

    public void OnNewGame() {
        // TODO 异步加载
        //SceneManager.LoadScene("Tutorial");
        SceneManager.LoadScene("SampleScene");
    }

    public void OnLoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
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

    public void OnMenuOpen() {
        Menu.OnOpen();
    }
    public void OnMenuClose() {
        Menu.OnClose();
    }
    // 玩家状态界面
    public void OnPlayStatusOpen() {
        PlayerStatus.OnOpen();
        data.OnOpen();
    }
    public void OnPlayStatusClose() {
        data.OnClose();
        PlayerStatus.OnClose();
    }

    public void OnEquipOpen() {
        equip.OnOpen();
    }
    public void OnEquipClose() {
        equip.OnClose();
    }

    public void OnStatusOpen() {
        data.OnOpen();
    }
    public void OnstatusClose() {
        data.OnClose();
    }

    public void OnItemOpen() {
        item.OnOpen();
    }
    public void OnItemClose() {
        item.OnClose();
    }

    public void OnSliderValueChanged(Slider slider) {
        if (slider.name.Contains("SE")) {

        } else {
            BGMVolume = slider.value;
            BGM.volume = BGMVolume;
        }
    }
}
