using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float smoothTime = 0.2f;

    private Vector3 _velocity = Vector3.zero;

    void LateUpdate() {
        // 只改变xy
        Vector3 playerPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref _velocity, smoothTime);
    }
}
