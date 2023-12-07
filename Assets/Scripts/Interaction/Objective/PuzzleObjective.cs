using UnityEngine;

public enum PuzzleType
{
    Code,
    Symbol,
    Battery,
}

public class PuzzleObjective : Objective
{
    [SerializeField] private PuzzleType puzzleType;
    
    void OnTriggerEnter(Collider other) 
    {
        if(!other.CompareTag("Player"))
            return;

        if(CheckObjective())
        {
            PuzzleManager.Instance.SolvePuzzle(puzzleType,objectiveCode,unityEvent);
            gameObject.SetActive(false);
        }
    }
}
