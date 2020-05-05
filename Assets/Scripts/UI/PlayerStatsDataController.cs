using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsDataController : MonoBehaviour
{
    void Start() {
        gameObject.SetActive(false);
    }

    public void OnOpen() {
        gameObject.SetActive(true);
    }
    public void OnClose() {
        gameObject.SetActive(false);
    }
}
