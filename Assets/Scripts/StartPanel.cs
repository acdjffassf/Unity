using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ʵ�ֿ�ʼ����Ĺ���
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
        //�ж��豸�Ƿ�׼����
        bool isInit = KinectManager.Instance.IsInitialized();
        if (isInit)
        {
            if (KinectImg!=null&&KinectImg.texture==null)
            {
                //���豸��ȡ������ݣ���������ݸ��ؼ���ʾ
                Texture2D kinectUseMap = KinectManager.Instance.GetUsersLblTex();
                KinectImg.texture = kinectUseMap;
            }
            //��ȡ�ؽ���Ϣ��1.�Ƿ��⵽�û� 2. ��ȡ��һ���û���������û� 3.�Ƿ�׷�ٵ��ؽڵ�
            if (KinectManager.Instance.IsUserDetected())
            {
                //��ȡ��һ���û���id
               long UserId= KinectManager.Instance.GetPrimaryUserID();
                //��ȡ�ؽڵ�����
               int jointType = (int)KinectInterop.JointType.HandRight;
                if (KinectManager.Instance.IsJointTracked(UserId, jointType))
                {
                    //��ȡ���ؽڵ�λ����Ϣ
                    Vector3 joinPos = KinectManager.Instance.GetJointKinectPosition(UserId,jointType);

                    //���ؽڵ���������ת������Ļ����
                    Vector3 screenVec3 = Camera.main.WorldToScreenPoint(joinPos);
                    //����ά����ת���ɶ�ά����
                    Vector2 screenVec2 = new Vector2(screenVec3.x,screenVec3.y);
                    //UGUI����
                    Vector2 UGUIPos;
                    //�ж��������canvas����ʾ�ľ��η�Χ��
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenVec2, Camera.main, out UGUIPos))
                    {
                        RigHandImg.rectTransform.anchoredPosition = UGUIPos;
                    }
                    //����ͼ���ʼ��չ��װ̬
                    RigHandImg.sprite = mHandStateSprites[0];
                    //��ȡ����װ̬
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
                                //�ж����Ƿ��ڿ�ʼ��Ϸͼ�����
                                Fruit1.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,3000));
                                Fruit1.GetComponent<Rigidbody2D>().gravityScale = 10;
                                Fruit2.GetComponent<Rigidbody2D>().gravityScale = 10;
                                Circle1.gameObject.SetActive(false);
                                Circle2.gameObject.SetActive(false);
                                
                            }
                            else if (RectTransformUtility.RectangleContainsScreenPoint(Circle2.rectTransform, screenVec2, Camera.main))
                            {
                                //�ж��Ƿ����˳���Ϸ����
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
