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
    void Start()
    {
        fruitPrefab = Resources.Load<Fruit>("Prefabs/Fruit");
        CreateFruit();
    }
    void Update()
    {
        
    }
    private void CreateFruit()
    {
        curFruit= Instantiate(fruitPrefab);
        //将水果的父物体设置为GamePanel
        curFruit.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        curFruit.transform.SetParent(transform);

        curFruit.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(fruitMinPosX, fruitMaxPosX), fruitPosY);
        curFruit.transform.localScale = new Vector3(1,1,1);
        Quaternion Pos = Quaternion.Euler(new Vector3(-45,0,0));
        Debug.Log(Pos); 
    }
}
