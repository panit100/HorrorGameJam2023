using System;
using System.Collections.Generic;
using DG.Tweening;
using HorrorJam.AI;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

namespace HorrorJam.AI
{
    public class AIManager : Singleton<AIManager>
    {
        public Vector3 PlayerPlanePosition => new Vector2(PlayerPosition.x, PlayerPosition.z);
        public Vector3 PlayerPosition { get; private set; }
        Transform playerTransform;

        [SerializeField] NavMeshSurface surface;
        [SerializeField] SpawnEnemyConfig spawnEnemyConfig; //TODO: Change it to array when a game have another enemy
        Enemy[] enemies;
        protected override void InitAfterAwake()
        {
            
        }

        void Start()
        {
            playerTransform = PlayerManager.Instance.transform;
            enemies = GameObject.FindObjectsOfType<Enemy>();
        }

        void Update()
        {
            PlayerPosition = playerTransform.position;
        }

        [Button]
        public void BakeNavMeshAfterDelay(float delay)
        {
            DOTween.Sequence()
                .AppendInterval(delay)
                .AppendCallback(surface.BuildNavMesh);
        }
        
        [Button]
        public void ShowAllEnemy()
        {
            //TODO: Run when cutscene end
            foreach(var n in enemies)
                n.ShowAI();
        }

        [Button]
        public void HideAllEnemy()
        {
            //TODO: Run on game start
            foreach(var n in enemies)
                n.HideAI();
        }

        [Button]
        public void SpawnEnemy()
        {
            Enemy enemy = Instantiate(spawnEnemyConfig.enemyPrefab,spawnEnemyConfig.spawnPosition,Quaternion.identity);
            enemy.SetCurrentWaypointContainer(spawnEnemyConfig.waypointContainer);
        }
    }
}

[Serializable]
public class SpawnEnemyConfig
{
    public Enemy enemyPrefab;
    public Vector3 spawnPosition;
    public WaypointContainer waypointContainer;
}