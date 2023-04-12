using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    private Fruit fruitPrefab;
    private Fruit curFruit;
    private int fruitMaxPosX = 280;
    private int fruitMinPosX = -280;
    private int fruitPosY = -270;

    private float time = 0;
    private float timer = 2;
    void Start()
    {
        
        fruitPrefab = Resources.Load<Fruit>("Prefabs/Fruit");
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time>=timer)
        {
            CreateFruit();
            time = 0;
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
}
