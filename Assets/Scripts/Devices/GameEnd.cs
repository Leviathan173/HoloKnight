using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] GameEndView view;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            view.OnOpen();
            Managers.Audio.bgm.PlayEndMusic();
        }


    }
}
