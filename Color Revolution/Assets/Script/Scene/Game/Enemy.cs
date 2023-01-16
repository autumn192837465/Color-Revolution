using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    
    private float redHealth = 255;
    private float blueHealth = 255;
    private float greenHealth = 255;
    void Start()
    {
        
    }
    
    void Update()
    {
        Vector3 dir = new Vector3(1, 0, 0);
        transform.Translate(dir * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if(bullet == null) return;

        redHealth -= bullet.AttackDamage.RedValue;
        blueHealth -= bullet.AttackDamage.BlueValue;
        greenHealth -= bullet.AttackDamage.GreenValue;

        if (redHealth < 0) redHealth = 0;
        if (blueHealth < 0) blueHealth = 0;
        if (greenHealth < 0) greenHealth = 0;
        
        SetColor();
        Destroy(bullet.gameObject);
    }
    
    

    private void SetColor()
    {
        meshRenderer.material.color = new Color(redHealth / 255.0f, blueHealth / 255.0f, greenHealth / 255.0f);
        if(redHealth ==0 && blueHealth == 0 && greenHealth == 0)    Destroy(gameObject);
    }
}