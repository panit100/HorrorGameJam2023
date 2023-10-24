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
        Enemy enemy;
        protected override void InitAfterAwake()
        {
            
        }

        void Start()
        {
            playerTransform = PlayerManager.Instance.transform;
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
                enemy.ShowAI();
        }

        [Button]
        public void HideAllEnemy()
        {
            //TODO: Run on game start
                enemy.HideAI();
        }

        [Button]
        public void SpawnEnemy()
        {
            Enemy _enemy = Instantiate(spawnEnemyConfig.enemyPrefab,spawnEnemyConfig.spawnPosition,Quaternion.identity);
            _enemy.SetCurrentWaypointContainer(spawnEnemyConfig.waypointContainer);
            enemy = _enemy;
        }
        
        [Button]
        public void RemoveEnemy()
        {
            Destroy(enemy.gameObject);
        }

        [Button]
        public void EnterStopEnemy()
        {
            if(enemy != null)
                enemy.EnterStop();
        }

        [Button]
        public void ExitStopEnemy()
        {
            if(enemy != null)
                enemy.ExitStop();
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