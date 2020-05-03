using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListController : MonoBehaviour
{
    void Start() {
        gameObject.SetActive(false);
    }

    public void OnOpen() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
    }
    public void OnClose() {
        gameObject.SetActive(false);
    }
}
