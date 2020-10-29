using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController cc;
    public float turnSmoothTime = 0.1f;
    public Transform cam;
    public bool ff = true;
    float turnSmoothVelocity;
    private Vector3 velocity = Vector3.zero;
    public Vector3 _vectorPos;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _vectorPos = transform.position;
    }

    void Update()
    {
        // float horizontal = Input.GetAxis("Horizontal");
        float vertical = 1;
        Vector3 direction = new Vector3(0,0,vertical).normalized;

        if (direction.magnitude >= 0.1 && ff)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,0, ref turnSmoothVelocity,turnSmoothTime);
            transform.rotation = Quaternion.Euler(0,angle,0);
            // Vector3 moveDir = Quaternion.Euler(0,targetAngle,0)*Vector3.forward;
            cc.Move(direction.normalized*speed*Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position,new Vector3(0,1,transform.position.z), ref velocity, 0.3f);
        }
    }
}
