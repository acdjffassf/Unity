using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Fruit fruitPrefab;
    private Fruit curFruit;
    private int fruitMaxPosX = 280;
    private int fruitMinPosX = -280;
    private int fruitPosY = -270;

    private RawImage KinectImage;
    private Transform LeftTrail;
    private Transform RightTrail;

    private float time = 0;
    private float timer = 2;
    void Start()
    {
        LeftTrail = transform.Find("LeftTrail");
        RightTrail = transform.Find("RightTrail");
        fruitPrefab = Resources.Load<Fruit>("Prefabs/Fruit");
        KinectImage = transform.Find("KinectImg").GetComponent<RawImage>();
       
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time >= timer)
        {
            CreateFruit();
            time = 0;
        }
        if (KinectImage!=null&&KinectImage.texture==null)
        {
            KinectImage.texture= KinectManager.Instance.GetUsersLblTex();
        }
        if (curFruit!=null)
        {
            if (KinectManager.Instance.IsUserDetected())
            {
                long UserId = KinectManager.Instance.GetPrimaryUserID();
                int joinType = (int)KinectInterop.JointType.HandRight;
                if (KinectManager.Instance.IsJointTracked(UserId, joinType))
                {
                    Vector3 jointPos = KinectManager.Instance.GetJointKinectPosition(UserId, joinType);
                    RightTrail.position = jointPos;
                    Vector3 screenVector3 = Camera.main.WorldToScreenPoint(jointPos);
                    Vector2 screenVector2 = new Vector2(screenVector3.x, screenVector3.y);
                    KinectInterop.HandState rightHandState = KinectManager.Instance.GetRightHandState(UserId);
                    switch (rightHandState)
                    {
                        case KinectInterop.HandState.Unknown:
                            break;
                        case KinectInterop.HandState.NotTracked:
                            break;
                        case KinectInterop.HandState.Open:
                            if (RectTransformUtility.RectangleContainsScreenPoint(curFruit.transform as RectTransform, screenVector2, Camera.main))
                            {
                                CutFruit();
                            }
                            break;
                        case KinectInterop.HandState.Closed:
                            break;
                        case KinectInterop.HandState.Lasso:
                            break;
                    }
                }
                joinType = (int)KinectInterop.JointType.HandLeft;
                if (KinectManager.Instance.IsJointTracked(UserId, joinType))
                {
                    Vector3 jointPos = KinectManager.Instance.GetJointKinectPosition(UserId, joinType);
                    LeftTrail.position = jointPos;
                    Vector3 screenVector3 = Camera.main.WorldToScreenPoint(jointPos);
                    Vector2 screenVector2 = new Vector2(screenVector3.x, screenVector3.y);
                    KinectInterop.HandState leftHandState = KinectManager.Instance.GetLeftHandState(UserId);
                    switch (leftHandState)
                    {
                        case KinectInterop.HandState.Unknown:
                            break;
                        case KinectInterop.HandState.NotTracked:
                            break;
                        case KinectInterop.HandState.Open:
                            if (RectTransformUtility.RectangleContainsScreenPoint(curFruit.transform as RectTransform, screenVector2, Camera.main))
                            {
                                CutFruit();
                            }
                            break;
                        case KinectInterop.HandState.Closed:
                            break;
                        case KinectInterop.HandState.Lasso:
                            break;
                    }
                }
            }
        }
        
    }
    private void CreateFruit()
    {
        curFruit = Instantiate(fruitPrefab);
        curFruit.transform.SetParent(transform);
        //将水果的父物体设置为GamePanel
        curFruit.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;


        int fruitPosX = Random.Range(fruitMinPosX, fruitMaxPosX);
        curFruit.GetComponent<RectTransform>().anchoredPosition = new Vector2(fruitPosX, fruitPosY);
        
        curFruit.transform.localScale = new Vector3(1, 1, 1);

        int[] types = new int[] { Constant.Boom, Constant.Apple, Constant.Banana, Constant.Basaha, Constant.Peach, Constant.Sandia };
        int fruitType = Random.Range(0, types.Length);
        curFruit.SetType(types[fruitType]);

        Rigidbody2D rig = curFruit.GetComponent<Rigidbody2D>();
        if (fruitPosX > 0)
        {
            rig.AddForce(new Vector2(-1000, 7000));
        }
        else
        {
            rig.AddForce(new Vector2(1000, 7000));
        }

    }
    private void CutFruit()
    {
        if (curFruit.type == Constant.Boom)
        {
            Destroy(curFruit.gameObject);
            
        }
        else
        {
            Fruit Leftfruit= Instantiate(fruitPrefab);
            Leftfruit.SetType(curFruit.type+1);
            InitLeftRightFruit(Leftfruit,true);
            Fruit rightfruit= Instantiate(fruitPrefab);
            rightfruit.SetType(curFruit.type + 2);
            InitLeftRightFruit(rightfruit,false);
        }
    }

    private void InitLeftRightFruit(Fruit fruit,bool isLeft)
    {
        fruit.transform.SetParent(transform);
        RectTransform rt= fruit.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        rt.anchoredPosition = curFruit.GetComponent<RectTransform>().anchoredPosition;
        rt.localScale = Vector3.one;
        if (isLeft)
        {
            fruit.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000, 2000));
        }
        else
        {
            fruit.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000, 2000));
        }
        
    }
}
