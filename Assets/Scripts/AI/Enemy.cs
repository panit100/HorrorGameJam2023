using System;
using UnityEngine;
using UnityEngine.AI;

namespace HorrorJam.AI
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] NavMeshAgent agent;

        void Update()
        {
            if (target)
                agent.destination = target.position;
        }
    }
}