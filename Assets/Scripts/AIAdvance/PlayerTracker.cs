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

    public Vector3 GetRandomPositionFarthestFromPlayer()
    {
        float farthestDistance = Mathf.NegativeInfinity;
        Vector3 farthestPosition = Vector3.zero;

        foreach(var n in patrolZones)
        {
            Vector3 tempPosition = RandomPositionInZone(n.bounds);

            float distance = Vector3.Distance(tempPosition,player.position);

            if(distance > farthestDistance)
            {
                farthestPosition = tempPosition;
                farthestDistance = distance;
            }
        }

        return farthestPosition;
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
                randomPosition = RandomPositionInZone(n.bounds);
            }
        }
        
        return randomPosition;
    }

    public Vector3 RandomPositionInZone(Bounds bound)
    {
        Vector3 position = Vector3.zero;

        float x = Random.Range(bound.min.x,bound.max.x);
        float z = Random.Range(bound.min.z,bound.max.z);

        if(NavMesh.SamplePosition(new Vector3(x,0,z),out NavMeshHit hit, bound.extents.magnitude, NavMesh.AllAreas))
        {
            position = hit.position;
        }

        return position;
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
