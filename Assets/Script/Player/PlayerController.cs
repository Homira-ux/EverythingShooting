using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playermovement;
    public float gravity = 20.0f;
    public float jumpSpeed = 0.5f;

    public bool isGrounded;

    public bool isFreezed = false;

    public bool isCool = true;
    public bool isDash = false;

    public float DashCool = 20.0f;
    public float DashKeep = 0.1f;

    public float FirstMoveSpeed;
    public float currentMoveSpeed;

    public GameManager gamemanager;

    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playermovement = GetComponent<PlayerMovement>();
        rb.transform.position = new Vector3(0, 1f, -15f);

        currentMoveSpeed = playermovement.MoveSpeed;
        isCool = true;
        isDash = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gamemanager.isPlayerlives){
            currentMoveSpeed = playermovement.MoveSpeed;
            if(isFreezed || gamemanager.GamePaused){
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            }
            else{
                rb.constraints &= ~RigidbodyConstraints.FreezePosition;
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection = moveDirection * playermovement.MoveSpeed;
                if(isGrounded){
                    if(Input.GetButton("Jump")){
                        moveDirection.y = jumpSpeed;
                    }

                    rb.velocity = moveDirection;
                }
                
                if(isCool){
                    if(Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift)){
                        FirstMoveSpeed = playermovement.MoveSpeed;
                        playermovement.MoveSpeed = currentMoveSpeed * 4.0f;
                        Dashing();
                        Cooling();
                    }
                }                
            }
        }
    }

    //クールタイム処理
    private async void Cooling(){
        isCool = false;
        await Task.Delay((int)(DashCool * 1000));
        isCool = true;
    }

    private async void Dashing(){
        isDash = true;
        await Task.Delay((int)(DashKeep * 1000));
        isDash = false;
        playermovement.MoveSpeed = FirstMoveSpeed;
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.CompareTag("Ground")){
            isGrounded = false;
        }
    }
}
