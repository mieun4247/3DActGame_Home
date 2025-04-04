using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public Vector3 offSet;
    void Update()
    {
        transform.position = Target.position + offSet;
    }
}
