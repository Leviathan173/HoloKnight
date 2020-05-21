using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {
    public AudioSource audio;
    public Vector3 finishPos = Vector3.zero;
    public float speed = 0.1f;
    public int _direction = 1; //移动方向，1向下，-1向上
    public bool atTop = true;

    private Vector3 _startPos;
    private float _trackPercent = 0;//起点到终点的移动百分比
    private bool hasCoroutine = false;


    void Start() {
        _startPos = transform.position;
        audio = GetComponent<AudioSource>();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);
    }
    /// <summary>
    /// 移动电梯
    /// </summary>
    public void Move() {
        if (!hasCoroutine) {
            StartCoroutine(MoveElevator());
            hasCoroutine = true;
        }

    }
    /// <summary>
    /// 电梯移动协程
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveElevator() {
        audio.Play();
        while (true) {
            _trackPercent += _direction * speed * Time.deltaTime;
            float x = (finishPos.x - _startPos.x) * _trackPercent + _startPos.x;// 沿X轴移动的最终点
            float y = (finishPos.y - _startPos.y) * _trackPercent + _startPos.y;// Y轴最终点
            transform.position = new Vector3(x, y, _startPos.z);
            yield return new WaitForSeconds(0.001f);
            if ((_direction == 1 && _trackPercent > 0.99f) || (_direction == -1 && _trackPercent < 0.01f)) {
                _direction *= -1;
                hasCoroutine = false;
                atTop = !atTop;
                audio.Pause();
                yield break;
            }
        }
    }
}
