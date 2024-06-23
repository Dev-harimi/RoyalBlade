using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    class WaveData
    {
        public Sprite sprite;
        public RuntimeAnimatorController animController;
        public int enemyCount;
        public int goldReward;
        public float enemyHP;
        public float scoreReward;
        public float mass;
        public float maxVelocity;
    }

    [SerializeField]
    WaveData[] waveData;
    int waveIndex = 0;
    float enemyDistance = 3f;

    private void Awake()
    {
        transform.position = GameManager.instance.EnemySpawnPos;
    }

    public void SpawnEnemyWave()
    {
        if (waveIndex >= waveData.Length)
        {
            GameManager.instance.GameWin();
            return;
        }

        SpawnEnemyGroup(waveData[waveIndex]);
        UIManager.instance.OnUpdateWaveProgress((float)waveIndex, (float)waveData.Length);
        waveIndex++;
    }

    void SpawnEnemyGroup(WaveData waveData)
    {
        bool isBoss = false;
        if (waveIndex == 4)
            isBoss = true;

        EnemyGroup enemyGroup = ObjectPooler.SpawnFromPool("EnemyGroup", transform.position).GetComponent<EnemyGroup>();
        enemyGroup.EnemyCount = waveData.enemyCount;
        enemyGroup.Mass = waveData.mass;
        enemyGroup.MaxVelocity = waveData.maxVelocity;

        for (int i = 0; i < enemyGroup.EnemyCount; i++)
        {
            Enemy enemy = ObjectPooler.SpawnFromPool("Enemy", transform.position).GetComponent<Enemy>();
            enemy.group = enemyGroup;
            enemy.SetHP(waveData.enemyHP);
            enemy.GoldReward = waveData.goldReward;
            enemy.ScoreReward = waveData.scoreReward;
            float posY = i * enemyDistance;
            enemy.transform.SetParent(enemyGroup.transform);
            enemy.transform.localPosition = new Vector2(0, posY);
            enemy.sr.sprite = waveData.sprite;
            enemy.anim.runtimeAnimatorController = waveData.animController;

            if (isBoss)
                enemy.transform.localScale *= 3f;
        }
    }
}
