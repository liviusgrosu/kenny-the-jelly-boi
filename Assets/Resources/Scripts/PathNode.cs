using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathNode : MonoBehaviour
{
    public Transform nextNode;
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, nextNode.position - transform.position, Color.green);
    }
}
