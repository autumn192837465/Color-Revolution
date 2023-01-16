using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public RGB AttackDamage;
    
    private float speed = 5;
    private GameObject destination;
    void Start()
    {
            
    }

    private bool destroying = false;
    void Update()
    {
        if (destination == null )
        {
            if (!destroying)
            {
                Destroy(gameObject);
                destroying = true;
            }
            
            return;
        }

        Vector3 dir = destination.transform.position - transform.position;
        dir = dir.normalized * speed;
        transform.position += dir * Time.deltaTime;
    }

    public void SetDestination(GameObject des)
    {
        destination = des;
    }

    public void SetColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void SetDamage(RGB attackDamage)
    {
        AttackDamage = attackDamage;
    }
}