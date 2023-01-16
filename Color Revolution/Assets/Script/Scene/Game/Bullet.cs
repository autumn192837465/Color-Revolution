using System.Collections;
using System.Collections.Generic;
using CR.Model;
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

    public void Initialize(RGB attackDamage)
    {
        AttackDamage = attackDamage;
        meshRenderer.material.color = attackDamage.GetColor();
    }

    public void SetDestination(GameObject des)
    {
        destination = des;
    }

}