using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    int enemyCount;
    float mass;
    float maxVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public int EnemyCount
    {
        get => enemyCount;
        set
        {
            enemyCount = value;
            if (enemyCount <= 0)
            {
                GameManager.instance.spawner.SpawnEnemyWave();
                gameObject.SetActive(false);
            }
        }
    }

    public float Mass
    {
        get => mass; 
        set
        {
            mass = value;
            rb.mass = mass;
        }
    }

    public float MaxVelocity { get => maxVelocity; set => maxVelocity = value; }

    void Update()
    {
        if (rb.velocity.y <= -maxVelocity)
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
    }
}
