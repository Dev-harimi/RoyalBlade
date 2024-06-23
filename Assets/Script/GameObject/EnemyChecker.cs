using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    GroundChecker groundChecker;
    Player player;
    float distance = 2.2f;
    RaycastHit2D hit;

    private void Awake()
    {
        player = GetComponent<Player>();
        groundChecker = GetComponent<GroundChecker>();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up * distance, Color.red);
        hit = Physics2D.Raycast(transform.position, Vector2.up, distance, 1 << 7);

        if (hit.collider != null && groundChecker.isGround)
            player.LoseHeart(hit.collider.GetComponent<Enemy>());
    }
}
