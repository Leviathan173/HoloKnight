using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] ElevatorController elevator;

    public bool MoveDown = true;
    Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (CheckDistance()) {
                if (MoveDown) {
                    if (elevator.atTop) {
                        StartCoroutine(Move());
                    }
                } else {
                    if (!elevator.atTop) {
                        StartCoroutine(Move());
                    }
                }
            }
        }
    }

    bool CheckDistance() {
        Vector3 direction = player.transform.position - transform.position;
        //print(gameObject.name+":"+Vector3.Dot(transform.forward, direction.normalized));
        if (Vector3.Dot(transform.forward, direction.normalized) < 0.5f) {
            if(transform.position.x-player.transform.position.x>-5 && transform.position.x - player.transform.position.x < 5) {
                if(transform.position.y - player.transform.position.y > -5 && transform.position.y - player.transform.position.y < 5) {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator Move() {
        elevator.Move();
        animator.SetBool("LeverUp", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("LeverUp", false);
    }
}
