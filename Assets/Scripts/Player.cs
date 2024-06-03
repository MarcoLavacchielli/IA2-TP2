using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody myrb;
    public int speed;
    void Start()
    {
        
        myrb = GetComponent<Rigidbody>();
    }
    public void FixedUpdate()
    {
        PlayerMovement();
    }
    public void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
       
        Vector3 direction = new Vector3(h, v,0f);
        
       
        myrb.MovePosition(myrb.position + (direction.normalized * speed * Time.fixedDeltaTime));
    }
}
