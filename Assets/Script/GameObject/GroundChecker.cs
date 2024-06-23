using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [HideInInspector]
    public bool isGround;
    float distance = 2f;
    RaycastHit2D hit;

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * 2f, Color.red);
        hit = Physics2D.Raycast(transform.position, Vector2.down, distance, 1 << 3);

        if (hit.collider != null)
            isGround = true;
        else
            isGround = false;
    }
}
