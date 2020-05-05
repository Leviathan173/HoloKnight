using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnOpen() {
        gameObject.SetActive(true);
    }
    public void OnClose() {
        gameObject.SetActive(false);
    }

    
}
