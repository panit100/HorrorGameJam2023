using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTracker : Singleton<PlayerTracker>
{
    [SerializeField] List<Collider> patrolZones = new List<Collider>();

    Transform player;

    protected override void InitAfterAwake()
    {
        player = PlayerManager.Instance.transform;
    }

    public Vector3 GetRandomPositionInPatrolZone()
    {
        Vector3 randomPosition = Vector3.zero;
        Bounds playerBounds = player.GetComponent<Collider>().bounds;

        foreach(var n in patrolZones)
        {
            if(n.bounds.Intersects(playerBounds))
            {
                //Player inside patrol zone
                float x = Random.Range(n.bounds.min.x,n.bounds.max.x);
                float z = Random.Range(n.bounds.min.z,n.bounds.max.z);

                if(NavMesh.SamplePosition(new Vector3(x,0,z),out NavMeshHit hit, n.bounds.extents.magnitude, NavMesh.AllAreas))
                {
                    randomPosition = hit.position;
                }
            }
        }
        
        return randomPosition;
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.magenta;
        foreach(var n in patrolZones)
        {
            Gizmos.DrawWireCube(n.bounds.center,n.bounds.size);
        }
    }
}
