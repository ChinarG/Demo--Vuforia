using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChinarTouch : MonoBehaviour
{
    private float   touchTime;     //点击事件
    private bool    IsFirstTouch;  //是否第一次点击
    private float   xSpeed = 150f; //旋转速度
    private Vector2 aPos;
    private Vector2 bPos;


    /// <summary>
    /// 判断手势是 放大还是缩小
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="aa"></param>
    /// <param name="bb"></param>
    /// <returns></returns>
    private bool IsEnlarge(Vector2 a, Vector2 b, Vector2 aa, Vector2 bb) //是否放大
    {
        float length1 = Mathf.Sqrt((a.x  - b.x)  * (a.x  - b.x)  + (a.y  - b.y)  * (a.y  - b.y));  //根号 2个坐标点的  x的平方+y的平方
        float length2 = Mathf.Sqrt((aa.x - bb.x) * (aa.x - bb.x) + (aa.y - bb.y) * (aa.y - bb.y)); //根号 2个坐标点的  x的平方+y的平方
        return length1 < length2 ? true : false;                                                   //true 放大，false 缩小
    }


    void Start()
    {
    }


    void Update()
    {


        if (Input.GetMouseButton(0))
        {
            ChangeRotation();
            OnLongTouch();
            ChangeScale();
        }

        if (transform.localPosition.y >= 0)
        {
            return;
        }
        transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
    }


    /// <summary>
    /// 控制角色缩放比例
    /// </summary>
    private void ChangeScale()
    {
        if (Input.touchCount == 2) //两个手指
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) //只要有手指一个移动
            {
                Vector2 temPos1 = Input.GetTouch(0).position;
                Vector2 temPos2 = Input.GetTouch(1).position;
                if (IsEnlarge(aPos, bPos, temPos1, temPos2)) //如果是对，就放大
                {
                    float oldScale       = transform.localScale.x; //原比例
                    float n              = oldScale * 1.025f;      //当前比例
                    transform.localScale = new Vector3(n, n, n);
                }
                else
                {
                    float oldScale       = transform.localScale.x; //原比例
                    float n              = oldScale / 1.025f;      //当前比例
                    transform.localScale = new Vector3(n, n, n);
                }


                aPos = temPos1;
                bPos = temPos2;
            }
        }
    }


    /// <summary>
    /// 控制旋转
    /// </summary>
    private void ChangeRotation()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved) //如果点击的是零
            {
                //旋转物体
                transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 150f * Time.deltaTime, Space.World);
            }
        }
    }


    /// <summary>
    /// 控制长按删除人物
    /// </summary>
    private void OnLongTouch()
    {
        Ray        ray = Camera.main.ScreenPointToRay(Input.mousePosition); //透过屏幕点 从相机发射一条射线
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo))
        {
            //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) //如果一根手指，且刚开始点击屏幕
            //{
            //    if (Input.GetTouch(0).tapCount == 2) //双击的时候                 
            //        Destroy(hitinfo.collider.gameObject);
            //}

            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    IsFirstTouch = true;
                    touchTime    = Time.time;
                }
                else if (t.phase == TouchPhase.Stationary)
                {
                    if (IsFirstTouch && Time.time - touchTime > 1f)
                    {
                        IsFirstTouch = false;
                        Destroy(hitinfo.collider.gameObject);
                    }
                }
                else
                {
                    IsFirstTouch = false;
                }
            }
        }
    }
}