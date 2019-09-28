using UnityEngine;
using System.Collections;

public class UnitychanController : MonoBehaviour {
    private Animator anim;
    //private CharacterController controller;
    //重力加速度
    const float Gravity = 9.81f;

    //重力の適応具合
    public float gravityScale = 1.0f;

    //Vector3 moveDirection = Vector3.zero;

    public float SpeedZ;
    public float RotSpeedR;
    public float RotSpeedL;
    private Vector3 m_pos;
    private float m_rot;
	// Use this for initialization
	void Start () {
        //controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        m_pos = this.transform.localPosition;
        m_pos.y = 0;
        m_rot = this.transform.localRotation.y;
	}

    // Update is called once per frame
    void Update() {
        Vector3 vector = new Vector3();

        //Unityエディターと実機で処理を分ける
        if (Application.isEditor)
        {
            //キーの入力を検知しベクトルを設定
            vector.x = Input.GetAxis("Horizontal");
            vector.z = Input.GetAxis("Vertical");
            //高さ方向の判定はキーのzとする
            if (Input.GetKey("z"))
            {
                vector.y = 1.0f;
            }
            else
            {
                vector.y = -1.0f;
            }
        }
        else
        {
            //加速度センサの入力をUnity空間の軸にマッピングする
            vector.x = Input.acceleration.x;
            vector.z = Input.acceleration.y;
            vector.y = Input.acceleration.z;
        }

        
        
            /*if(vector.z > 0.0f)
            {
                moveDirection.z = vector.z * SpeedZ;
            }
            else
            {
                moveDirection.z = 0;
            }
        

        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        if (vector.z > 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);

        }*/
        
        if(vector.z > 0)
        {
            anim.SetBool("Run", true);
            m_pos.z += SpeedZ;
            this.transform.localPosition += m_pos;
        }else if(vector.z < 0)
        {
            m_pos.z -= SpeedZ;
            this.transform.localPosition += m_pos;
        }
        else
        {
            m_pos.z = 0;
            anim.SetBool("Run", false);

        }
        if (vector.x > 0)
        {
            anim.SetBool("Run", true);
            m_pos.x += SpeedZ;
            this.transform.localPosition += m_pos;
            transform.LookAt(transform.localPosition);
            this.transform.Rotate(0, vector.x * RotSpeedR, 0);
        }else if(vector.x < 0){
            m_pos.x -= SpeedZ;
            this.transform.localPosition += m_pos;
            this.transform.Rotate(0, vector.x * RotSpeedL, 0);
        }
        else
        {
            m_pos.x = 0;
            anim.SetBool("Run", false);
        }


        }

    }

