using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(AudioManager))]

public class Managers : MonoBehaviour {
    public static Managers managers { get; private set; }
	public static PlayerManager Player {get; private set;}
    public static UIManager UI { get; private set; }
    public static AudioManager Audio { get; private set; }

    private Dictionary<string, IGameManager> _startSequence;
	
	void Awake() {
        managers = this;
		Player = GetComponent<PlayerManager>();
        UI = GetComponent<UIManager>();
        Audio = GetComponent<AudioManager>();


        _startSequence = new Dictionary<string, IGameManager>();

		_startSequence.Add("Player",Player);
        _startSequence.Add("UI", UI);
        _startSequence.Add("Audio", Audio);

		StartCoroutine(StartupManagers());
	}
    /// <summary>
    /// 启动所有管理器的协程
    /// </summary>
    /// <returns></returns>
	private IEnumerator StartupManagers() {
		foreach (KeyValuePair<string,IGameManager> manager in _startSequence) {
            manager.Value.Startup();
		}

		yield return null;

		int numModules = _startSequence.Count;
		int numReady = 0;
		
		while (numReady < numModules) {
			int lastReady = numReady;
			numReady = 0;
			
			foreach (KeyValuePair<string, IGameManager> manager in _startSequence) {
				if (manager.Value.status == ManagerStatus.Started) {
					numReady++;
				}
			}
			
			if (numReady > lastReady)
				Debug.Log("Progress: " + numReady + "/" + numModules);
			
			yield return null;
		}
		
		Debug.Log("All managers started up");
	}
    /// <summary>
    /// 添加新的管理器
    /// </summary>
    /// <param name="name">管理器关联的物体的名字</param>
    /// <param name="manager">管理器</param>
    public void AddManager(string name,IGameManager manager) {
        if (!_startSequence.ContainsKey(name)) {
            //print("Add new manager " + name);
            _startSequence.Add(name, manager);
            //print("setup manager " + name);
            _startSequence[name].Startup();
            //print("setup finish " + name);
        } else {
            print("already has such manager");
        }
        
    }
        
    /// <summary>
    /// 通过名字获取管理器
    /// </summary>
    /// <param name="name">管理器关联的物体的名字</param>
    /// <returns></returns>
    public IGameManager GetManager(string name) {
        if (_startSequence.ContainsKey(name)) {
            //print("return manager name "+ name + " obj :"+_startSequence[name]);
            return _startSequence[name];
        } else {
            print("has not manager "+ name);
            return null;
        }
    }
}
