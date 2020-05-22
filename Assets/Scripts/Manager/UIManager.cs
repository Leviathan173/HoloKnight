using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameManager {
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
    //[SerializeField] private ItemListController item;

    public void Startup() {
        BGMVolume = 1;
        SESoundVolume = 1;
        if(PlayerStatus != null) {
            equip = PlayerStatus.GetComponentInChildren<EquipController>();
            //item = PlayerStatus.GetComponentInChildren<ItemListController>();
            data = PlayerStatus.GetComponentInChildren<PlayerStatsDataController>();
        } else {
            Debug.LogWarning("PlayerStatus view is null");
        }
        
        status = ManagerStatus.Started;
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void OnExitGame() {
        Application.Quit();
    }
    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void OnNewGame() {
        // TODO 异步加载
        //SceneManager.LoadScene("Tutorial");
        SceneManager.LoadScene("Stage1");
    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    public void OnLoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// 加载游戏界面弹出
    /// </summary>
    public void OnLoadGame() {
        // TODO 加载游戏
        // 需要一个弹出界面
    }
    /// <summary>
    /// 加载游戏
    /// </summary>
    /// <param name="name">存档名字</param>
    public void LoadGame(string name) {
        // TODO 加载特定的一个存档
        // 传的参数没有决定
    }
    /// <summary>
    /// 设置界面弹出
    /// </summary>
    public void OnSettingOpen() {
        Setting.Open();
        MainCanvas.gameObject.SetActive(false);
    }
    /// <summary>
    /// 设置界面关闭
    /// </summary>
    public void OnSettingExit() {
        Setting.Close();
        MainCanvas.gameObject.SetActive(true);
    }
    /// <summary>
    /// 菜单界面弹出
    /// </summary>
    public void OnMenuOpen() {
        if (Menu.isActiveAndEnabled) {
            Menu.OnClose();
            return;
        }
        Menu.OnOpen();
    }
    /// <summary>
    /// 菜单界面关闭
    /// </summary>
    public void OnMenuClose() {
        Menu.OnClose();
    }
    /// <summary>
    /// 玩家状态界面弹出
    /// </summary>
    public void OnPlayStatusOpen() {
        PlayerStatus.OnOpen();
        equip.OnClose();
        data.OnOpen();
    }
    /// <summary>
    /// 玩家状态界面关闭
    /// </summary>
    public void OnPlayStatusClose() {
        data.OnClose();
        PlayerStatus.OnClose();
    }
    /// <summary>
    /// 玩家状态中的装备菜单显示
    /// </summary>
    public void OnEquipOpen() {
        equip.OnOpen();
    }
    /// <summary>
    /// 玩家状态中的装备菜单关闭
    /// </summary>
    public void OnEquipClose() {
        equip.OnClose();
    }
    /// <summary>
    /// 状态数值显示菜单弹出
    /// </summary>
    public void OnStatusOpen() {
        data.OnOpen();
    }
    /// <summary>
    /// 状态数值显示菜单关闭
    /// </summary>
    public void OnstatusClose() {
        data.OnClose();
    }
    ///// <summary>
    ///// 更换装备的装备栏显示
    ///// </summary>
    //public void OnItemOpen() {
    //    item.OnOpen();
    //}
    ///// <summary>
    ///// 更换装备的装备栏关闭
    ///// </summary>
    //public void OnItemClose() {
    //    item.OnClose();
    //}
    /// <summary>
    /// 修改音乐音量大小
    /// </summary>
    /// <param name="slider">控制音乐的滑动条</param>
    public void OnSoundSliderValueChanged(Slider slider) {
        BGMVolume = slider.value;
        //BGM.volume = BGMVolume;
        Managers.Audio.bgm.OnSliderValueChanged(slider);
    }
    /// <summary>
    /// 修改音效音量大小
    /// </summary>
    /// <param name="slider">控制音效的滑动条</param>
    public void OnSESoundSliderValueChanged(Slider slider) {
        Managers.Audio.OnSESliderValueChanged(slider);
    }

}
