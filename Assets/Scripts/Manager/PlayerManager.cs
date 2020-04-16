using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {
    [SerializeField] private Player player;

    public ManagerStatus status { get; private set; }

    public void Startup() {
        print("starting PlayerManager...");

        

        status = ManagerStatus.Started;
    }


}
