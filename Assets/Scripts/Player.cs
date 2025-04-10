using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{
    public float Speed;
    public GameObject[] Weapon;
    public bool[] hasWeapon;
    public GameObject[] grenads;
    public int hasGrenade;

    public int ammo;
    public int coin;
    public int health;
    
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenade;

    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;


    bool isJump;
    bool isDodge;
    bool isSwap;

    Rigidbody Rigid;
    Animator Anim;

    Vector3 moveVec;
    Vector3 dodgeVec;

    GameObject nearObject;
    GameObject equipWeapon;
    int equipWeapoIndex = -1;

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
        Swap();
        Interation();



    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    void Move()
    {
 
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (isSwap)
            moveVec = Vector3.zero;

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
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            Rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            Anim.SetBool("isJump", true);
            Anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)
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
    void Swap()
    {
        if (sDown1 && (!hasWeapon[0] || equipWeapoIndex == 0))
            return;
        if (sDown2 && (!hasWeapon[1] || equipWeapoIndex == 1))
            return;
        if (sDown3 && (!hasWeapon[2] || equipWeapoIndex == 2))
            return;


        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if(equipWeapon != null)
            equipWeapon.SetActive(false);

            equipWeapoIndex = weaponIndex;
            equipWeapon = Weapon[weaponIndex];
            equipWeapon.SetActive(true);

            Anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
      
        isSwap = false;
    }
    void Interation()
    {
        if(iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.Value;
                hasWeapon[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item") {
            Item item = other.GetComponent<Item>();
            switch(item.type) {

                case Item.Type.Ammo:
                ammo += item.Value;
                if(ammo > maxAmmo)
                ammo = maxAmmo;
                    break;

                case Item.Type.Coin:
                coin += item.Value;
                if(coin > maxCoin)
                coin = maxCoin;
                    break;

                case Item.Type.Heart:
                health += item.Value;
                if(health > maxHealth)
                health = maxHealth;
                    break;

                case Item.Type.Grenade:
                    grenads[hasGrenade].SetActive(true);
                hasGrenade += item.Value;
                if(hasGrenade > maxHasGrenade)
                hasGrenade = maxHasGrenade;

                    break;
            
            }
            Destroy(other.gameObject);
        }   
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }

}
