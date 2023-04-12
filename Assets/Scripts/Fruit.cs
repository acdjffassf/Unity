using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fruit : MonoBehaviour
{
    private Sprite[] fruitSprites=new Sprite[16];
    private Image image;
    [HideInInspector]
    private int mType;
    public int type
    {
        get
        {
            return mType;
        }
    }
    private void OnEnable()
    {
        fruitSprites[0] = Resources.Load<Sprite>("Textures/Fruits/boom");
        fruitSprites[1] = Resources.Load<Sprite>("Textures/Fruits/apple");
        fruitSprites[2] = Resources.Load<Sprite>("Textures/Fruits/apple-1");
        fruitSprites[3] = Resources.Load<Sprite>("Textures/Fruits/apple-2");
        fruitSprites[4] = Resources.Load<Sprite>("Textures/Fruits/banana");
        fruitSprites[5] = Resources.Load<Sprite>("Textures/Fruits/banana-1");
        fruitSprites[6] = Resources.Load<Sprite>("Textures/Fruits/banana-2");
        fruitSprites[7] = Resources.Load<Sprite>("Textures/Fruits/basaha");
        fruitSprites[8] = Resources.Load<Sprite>("Textures/Fruits/basaha-1");
        fruitSprites[9] = Resources.Load<Sprite>("Textures/Fruits/basaha-2");
        fruitSprites[10] = Resources.Load<Sprite>("Textures/Fruits/peach");
        fruitSprites[11] = Resources.Load<Sprite>("Textures/Fruits/peach-1");
        fruitSprites[12] = Resources.Load<Sprite>("Textures/Fruits/peach-2");
        fruitSprites[13] = Resources.Load<Sprite>("Textures/Fruits/sandia");
        fruitSprites[14] = Resources.Load<Sprite>("Textures/Fruits/sandia-1");
        fruitSprites[15] = Resources.Load<Sprite>("Textures/Fruits/sandia-2");
    }
    public void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        
        
    }
    //设置水果类型
    public void SetType(int type)
    {
        mType = type;
        image.sprite = fruitSprites[mType];
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("111");
        if (collision.gameObject.tag=="Bound")
        {
            
            Destroy(gameObject);
        }
    }
}
