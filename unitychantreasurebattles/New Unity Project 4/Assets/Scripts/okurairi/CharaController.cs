using UnityEngine;
using System.Collections;

public class CharaController : MonoBehaviour
{

    private Animator anim;
    private CharacterController charaCon;
    private Vector3 move = Vector3.zero;
    public float speed;
    public float jumpPower;
    private const float GRAVITY = 9.8f;
    public float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        charaCon = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        /*NormalControl();
        CameraAxisControl();
        jumpControl();
        attachMove();
        attachRotation();*/

        //移動量の取得
        float y = move.y;
        move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 playerDir = move;  //移動方向を取得*

        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
        move = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
        move *= speed;
        //move *= speed;
        //重力/ジャンプ処理
        
        if (charaCon.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                anim.SetBool("Jump", true);
                move.y = jumpPower;
            }else
            {
                anim.SetBool("Jump", false);
            }
            
        }
 
        move.y -= GRAVITY * Time.deltaTime;

        //プレイヤーの向きを変更
        if(playerDir.magnitude > 0.1f)
        {
            Quaternion q = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
        }

        if (playerDir.magnitude > 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
        //移動処理
        charaCon.Move(move * Time.deltaTime);



    }

    /*void NormalControl()
    {
        if (charaCon.isGrounded)
        {
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = transform.TransformDirection(move);
            move *= speed;
        }
    }

    void CameraAxisControl()
    {
        if (charaCon.isGrounded)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
            move = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            move *= speed;
        }
    }

    void jumpControl()
    {
        if (Input.GetKeyDown("KeyCode.Space"))
            {
            move.y = jumpPower;
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }
    }
    void attachMove()
    {
        move.y -= GRAVITY * Time.deltaTime;
        charaCon.Move(move * Time.deltaTime);
    }
    void attachRotation()
    {
        var moveYzero = -move;
        moveYzero.y = 0;

        if (moveYzero.sqrMagnitude > 0.001)
        {
            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, moveYzero, step, 0f);

            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }*/
}
