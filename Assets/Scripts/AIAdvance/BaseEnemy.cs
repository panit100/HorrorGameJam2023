using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseEnemy : MonoBehaviour
{
    [TitleGroup("Speed")]
    [SerializeField] float chaseSpeed = 10f;
    [TitleGroup("Detection")]
    [SerializeField] float detectionRange = 10f;
    [SerializeField] Vector3 raycastDetectSize = Vector3.one;
    Vector3 distination;

    NavMeshAgent agent;

    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();    
    }

    void Start()
    {
        SetAISpeed(chaseSpeed);
    }

    void Update()
    {
        SetDistination();
    }

    [Button]
    void SetAIDistination(Vector3 distination)
    {
        if(agent.enabled == true)
            agent.destination = distination;
    }

    [Button]
    void SetAISpeed(float speed)
    {
        agent.speed = chaseSpeed;
    }

    [Button]
    void EabledAI(bool enabled)
    {
        agent.enabled = enabled;
    }

    void SetDistination()
    {
        if(!IsPlayerInDectectionRange())
            return;

        if(IsSomethingInDirectionToPlayer(out var hit))
        {
            if(hit.collider.CompareTag("Player"))
            {
                distination = PlayerManager.Instance.transform.position;
                SetAIDistination(distination);
            }
        }
    }

    bool IsSomethingInDirectionToPlayer(out RaycastHit hit)
    {
        Quaternion rotation = Quaternion.identity;

        var playerPos = PlayerManager.Instance.transform.position;
        playerPos.y = transform.position.y;
        var direction = (playerPos - transform.position).normalized;
        return Physics.BoxCast(transform.position, raycastDetectSize, direction, out hit, rotation, detectionRange,LayerMask.GetMask("Player","Wall"));
    }

    bool IsPlayerInDectectionRange()
    {
        var playerPos = PlayerManager.Instance.transform.position;
        float distance = Vector3.Distance(playerPos,transform.position);

        if(distance > detectionRange)
            return false;

        return true;
    }

    void OnDrawGizmos() 
    {
        Vector3 origin = transform.position;
        origin.y = transform.position.y - transform.localScale.y;
        
        Gizmos.color = Handles.color = Color.red;
        Handles.DrawWireDisc(origin,Vector3.up,detectionRange,2f); 
        
        Vector3 playerPos = PlayerManager.Instance.transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;

        Gizmos.color = Handles.color = Color.red;

        if(!IsPlayerInDectectionRange())
            Gizmos.color = Handles.color = Color.green;

        if(IsSomethingInDirectionToPlayer(out var hit))
        {
            if(!hit.collider.CompareTag("Player"))
                Gizmos.color = Handles.color = Color.green;
        }

        Handles.DrawLine(transform.position,transform.position + direction * detectionRange,5f);
    }
}
