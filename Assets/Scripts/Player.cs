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
    bool jDown;

    bool isJump;
    bool isDodge;

    Rigidbody Rigid;
    Animator Anim;

    Vector3 moveVec;
    Vector3 dodgeVec;
    // Start is called before the first frame update
    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();



    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }
    void Move()
    {
 
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        transform.position += moveVec * Speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        Anim.SetBool("isRun", moveVec != Vector3.zero);
        Anim.SetBool("isWalk", wDown);

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }
    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            Rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            Anim.SetBool("isJump", true);
            Anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            Speed *= 2;
            Anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }
    void DodgeOut() 
    {
        Speed *= 0.5f;
        isDodge = false;
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
