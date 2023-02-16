using Kinopi.Constants;
using Kinopi.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Kinopi.Extensions;
using Kinopi.Utils;
using MoreMountains.Feedbacks;
using UnityEngine.Rendering;


public class FloatingText : MonoBehaviour
{
    private float time;
   
    
    //[SerializeField] private GameObject iconRoot;
    //[SerializeField] private SpriteRenderer icon;
    [SerializeField] private TextMeshPro contentText;
    [SerializeField] private Transform textContent;

    [SerializeField] private Color green = Color.green;
    [SerializeField] private Color white = Color.white;
    [SerializeField] private Color red = Color.red;
    [SerializeField] private Color yellow = Color.yellow;

    [Header("Critical")]
    [SerializeField] private Color CriticalColor = Color.white;
    [SerializeField] private VertexGradient CriticalColorGradiant;

    //[SerializeField] private Material maskMatertial;
    //[SerializeField] private Material normalMatertial;

    [SerializeField] private AnimationCurve movingCurve;
    [SerializeField] private AnimationCurve alphaCurve;
    //[SerializeField] private SortingGroup sortingGroup;
    public enum Type
    {
        Red,
        Blue,
        Green,
        Miss,
        Critical
    }

    
    
    public static FloatingText CreateText(Vector3 worldPosition, Type type, string content)
    {
        FloatingText textObject = Instantiate(GameAssets.Instance.floatingText, worldPosition, Quaternion.identity);
        
        textObject.Initialize(type, content);

        return textObject;
    }
    public static FloatingText CreateText(Vector3 worldPosition, Type type, int value)
    {
        FloatingText textObject = Instantiate(GameAssets.Instance.floatingText, worldPosition, Quaternion.identity);
        
        textObject.Initialize(type, value.ToString());
        
        
        return textObject;
    }

    private void Awake()
    {
        time = Constants.FloatingTextLifeTime;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 0) return;

        float pos = movingCurve.Evaluate(1 - (time / Constants.FloatingTextLifeTime));
        textContent.localPosition = new Vector3(pos, pos, pos) * Constants.FloatingTextMoveFactor;

        float a = alphaCurve.Evaluate(time / Constants.FloatingTextLifeTime);

        Color tc = contentText.color;
        tc.a = a;
        contentText.color = tc;

        /*
        Color ic = icon.color;
        ic.a = a;
        icon.color = ic;*/

        time -= Time.deltaTime;
        if (time <= 0) Destroy(gameObject);

    }



    public void Initialize(Type type, string content)
    {
        transform.position += Utils.GetRandomVector(0.2f);
        contentText.text = content;

        contentText.color = type switch
        {
            Type.Red => red,
            Type.Green => Color.green,
            Type.Blue => Color.blue,
            _ => white
        };



        //sortingGroup.sortingLayerName = Utils.GetSortingLayerName(sortingLayer);
    }
    
    

}
