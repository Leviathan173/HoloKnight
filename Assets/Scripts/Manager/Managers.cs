using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(SkeletonManager))]
//[RequireComponent(typeof(InventoryManager))]

public class Managers : MonoBehaviour {
	public static PlayerManager Player {get; private set;}
    public static SkeletonManager Skeleton { get; private set; }
    //public static InventoryManager Inventory {get; private set;}

    private List<IGameManager> _startSequence;
	
	void Awake() {
		Player = GetComponent<PlayerManager>();
        Skeleton = GetComponent<SkeletonManager>();
		//Inventory = GetComponent<InventoryManager>();

		_startSequence = new List<IGameManager>();

		_startSequence.Add(Player);
        _startSequence.Add(Skeleton);
		//_startSequence.Add(Inventory);

		StartCoroutine(StartupManagers());
	}

	private IEnumerator StartupManagers() {
		foreach (IGameManager manager in _startSequence) {
			manager.Startup();
		}

		yield return null;

		int numModules = _startSequence.Count;
		int numReady = 0;
		
		while (numReady < numModules) {
			int lastReady = numReady;
			numReady = 0;
			
			foreach (IGameManager manager in _startSequence) {
				if (manager.status == ManagerStatus.Started) {
					numReady++;
				}
			}
			
			if (numReady > lastReady)
				Debug.Log("Progress: " + numReady + "/" + numModules);
			
			yield return null;
		}
		
		Debug.Log("All managers started up");
	}

    public void AddManager(IGameManager manager) {
        _startSequence.Add(manager);
    }

    public void GetSkletonManager(string name) {
        // TODO 获取相应manager
    }
}
