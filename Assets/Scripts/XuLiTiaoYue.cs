using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XuLiTiaoYue : MonoBehaviour {

    bool OnGround;   //是否在地面上
    float jumpPressure = 0f;  //蓄力值
    float MinjumpPressure = 3f;  //蓄力最小值
    public float MaxjumpPressure = 5f;  // 蓄力最大值
    Rigidbody rbody;
   // Animator animator;
    // Use this for initialization
    void Start()
    {
        OnGround = true;  //初始设置在地面上
        rbody = GetComponent<Rigidbody>();  //获取组件
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnGround)  //判断是否在地面上
        {
            if (Input.GetButton("Jump"))  //hold  按下(住)不放空格键
            {
                if (jumpPressure < MaxjumpPressure)
                {  //如果当前蓄力值小于最大值
                    jumpPressure += Time.deltaTime * 5f; //则每帧递增当前蓄力值
                }
                else
                {  //达到最大值时，当前蓄力值就等于最大蓄力值
                    jumpPressure = MaxjumpPressure;
                }
                print("hold: " + jumpPressure);  //测试用，请忽略
                //这时设置动画为蓄力状态动画
               // animator.SetFloat("IsPressure", jumpPressure);
            }
            else  //not hold   鼠标松开时
            {
                if (jumpPressure > 0f)
                {   //如果是轻轻按下就松开则把最小蓄力值赋值给当前蓄力值
                    //如果是按住不松则把上面递增的值传下来
                    jumpPressure += MinjumpPressure;
                    //给一个向上速度
                    rbody.velocity = new Vector3(0f, jumpPressure, 0f);
                    jumpPressure = 0f; //升空以后把蓄力值重设为0
                    OnGround = false;  //在地面上设为否
                }
              //  animator.SetFloat("IsPressure", 0f); //设置动画的Float值为0
               // animator.SetBool("OnGround", OnGround); //根据是否在地面上播放动画
            }
        }

    }
    //void OnCollisionEnter(Collision other)
    //{
    //    //检测是否碰撞到地面
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        OnGround = true;
    //    }

    //}
}