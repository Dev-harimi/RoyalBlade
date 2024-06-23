using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 preset;

    [SerializeField]
    float followSpeed = 10f;

    private void Start()
    {
        preset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + preset, Time.deltaTime * followSpeed);
    }
}
