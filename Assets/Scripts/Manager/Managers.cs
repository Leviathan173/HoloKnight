using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(SkeletonManager))]
//[RequireComponent(typeof(InventoryManager))]

public class Managers : MonoBehaviour {
    public static Managers managers { get; private set; }
	public static PlayerManager Player {get; private set;}
    public static SkeletonManager Skeleton { get; private set; }
    //public static InventoryManager Inventory {get; private set;}

    private Dictionary<string, IGameManager> _startSequence;
	
	void Awake() {
        managers = this;
		Player = GetComponent<PlayerManager>();
        Skeleton = GetComponent<SkeletonManager>();
        //Inventory = GetComponent<InventoryManager>();

        //_startSequence = new List<IGameManager>();
        _startSequence = new Dictionary<string, IGameManager>();

		_startSequence.Add("Player",Player);
        //_startSequence.Add("Skeleton",Skeleton);
		//_startSequence.Add(Inventory);

		StartCoroutine(StartupManagers());
	}

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

    public void AddManager(string name,IGameManager manager) {
        print("Add new manager "+ name);
        _startSequence.Add(name, manager);
        print("setup manager " + name);
        _startSequence[name].Startup();
        print("setup finish " + name);
    }
        

    public IGameManager GetManager(string name) {
        if (_startSequence.ContainsKey(name)) {
            print("has manager "+ name);
            return _startSequence[name];
        } else {
            print("has not manager "+ name);
            return null;
        }
    }
}
