using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 实现开始界面的功能
/// </summary>
public class StartPanel : MonoBehaviour
{
    private RawImage KinectImg;
    private RectTransform Canvas;
    private Image RigHandImg;
    private Image Circle1;
    private Image Fruit1;
    private Image Circle2;
    private Image Fruit2;
    private Image curClickFruit;

    public Sprite[] mHandStateSprites;
    void Start()
    {
        KinectImg = transform.Find("KinectImg").GetComponent<RawImage>();
        Canvas = GetComponentInParent<RectTransform>();
        RigHandImg = transform.Find("KinectImg").GetComponent<Image>();
        Circle1 = transform.Find("Item1/Circle1").GetComponent<Image>();
        Fruit1 = transform.Find("Item1/Fruit1").GetComponent<Image>();
        Circle2 = transform.Find("Item2/Circle2").GetComponent<Image>();
        Fruit2 = transform.Find("Item2/Fruit2").GetComponent<Image>();
    }
    void Update()
    {
        //判断设备是否准备好
        bool isInit = KinectManager.Instance.IsInitialized();
        if (isInit)
        {
            if (KinectImg!=null&&KinectImg.texture==null)
            {
                //从设备获取深度数据，将深度数据给控件显示
                Texture2D kinectUseMap = KinectManager.Instance.GetUsersLblTex();
                KinectImg.texture = kinectUseMap;
            }
            //获取关节信息：1.是否检测到用户 2. 获取第一个用户或最近的用户 3.是否追踪到关节点
            if (KinectManager.Instance.IsUserDetected())
            {
                //获取第一个用户的id
               long UserId= KinectManager.Instance.GetPrimaryUserID();
                //获取关节的类型
               int jointType = (int)KinectInterop.JointType.HandRight;
                if (KinectManager.Instance.IsJointTracked(UserId, jointType))
                {
                    //获取到关节的位置信息
                    Vector3 joinPos = KinectManager.Instance.GetJointKinectPosition(UserId,jointType);

                    //将关节的世界坐标转换成屏幕坐标
                    Vector3 screenVec3 = Camera.main.WorldToScreenPoint(joinPos);
                    //将三维坐标转换成二维坐标
                    Vector2 screenVec2 = new Vector2(screenVec3.x,screenVec3.y);
                    //UGUI坐标
                    Vector2 UGUIPos;
                    //判断这个手在canvas所表示的矩形范围内
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenVec2, Camera.main, out UGUIPos))
                    {
                        RigHandImg.rectTransform.anchoredPosition = UGUIPos;
                    }
                    //右手图标初始是展开装态
                    RigHandImg.sprite = mHandStateSprites[0];
                    //获取手势装态
                    KinectInterop .HandState rightHandState = KinectManager.Instance.GetRightHandState(UserId);
                    switch (rightHandState)
                    {
                        case KinectInterop.HandState.Unknown:
                            break;
                        case KinectInterop.HandState.NotTracked:
                            break;
                        case KinectInterop.HandState.Open:
                            RigHandImg.sprite = mHandStateSprites[0];
                           
                            break;
                        case KinectInterop.HandState.Closed:
                            RigHandImg.sprite = mHandStateSprites[1];
                            if (RectTransformUtility.RectangleContainsScreenPoint(Circle1.rectTransform, screenVec2, Camera.main)&&Circle1.gameObject.activeSelf==true)
                            {
                                curClickFruit = Fruit1;
                                //判断手是否在开始游戏图标框内
                                Fruit1.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,3000));
                                Fruit1.GetComponent<Rigidbody2D>().gravityScale = 10;
                                Fruit2.GetComponent<Rigidbody2D>().gravityScale = 10;
                                Circle1.gameObject.SetActive(false);
                                Circle2.gameObject.SetActive(false);
                                
                            }
                            else if (RectTransformUtility.RectangleContainsScreenPoint(Circle2.rectTransform, screenVec2, Camera.main))
                            {
                                //判断是否在退出游戏框内
                                Application.Quit();   
                            }
                            break;
                        case KinectInterop.HandState.Lasso:
                            break;
                    }
                } 
            }
            
        }
        if (curClickFruit!=null&&curClickFruit.rectTransform.anchoredPosition.y<-300)
        {
            
        }
    }
}
