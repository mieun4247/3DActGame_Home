using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float Speed;
    float hAxis;
    float vAxis;
    bool wDown;

    Animator Anim;

    Vector3 moveVec;
    // Start is called before the first frame update
    void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
            transform.position += moveVec * Speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        Anim.SetBool("isRun", moveVec != Vector3.zero);
        Anim.SetBool("isWalk", wDown);


        transform.LookAt(transform.position + moveVec);
    }
}
