using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public Vector3 XZPosition => new Vector3(transform.position.x, 0, transform.position.z);
}