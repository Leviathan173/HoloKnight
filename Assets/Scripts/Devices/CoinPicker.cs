using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPicker : MonoBehaviour
{
    // UNDONE 拾取硬币并增加玩家金币
    [SerializeField]public int gold = 100;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            Managers.Player.gold += gold;
            Destroy(gameObject);
        }
    }
    
    public void SetGold(int value) {
        gold = value;
    }
}
