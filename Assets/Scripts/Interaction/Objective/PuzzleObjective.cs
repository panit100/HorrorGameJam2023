using UnityEngine;

public enum PuzzleType
{
    Code,
    Symbol,
    Battery,
}

public class PuzzleObjective : Objective, InteractObject
{
    [SerializeField] private PuzzleType puzzleType;

    [Header("Demo Setting")]
    [Tooltip("This check is for a test scene. PuzzleObjective is must have a Objective controller in scene to use it.")]
    [SerializeField] bool testObject;
    [SerializeField] string testCode;


    public void TestEvent()
    {
        print($"{this.name} Puzzle Complete");
        gameObject.SetActive(false);
    }

    public void OnInteract()
    {
        if (testObject)
        {
            PuzzleManager.Instance.SolvePuzzle(puzzleType, testCode, unityEvent, true);
            return;
        }

        if (CheckObjective())
        {
            PuzzleManager.Instance.SolvePuzzle(puzzleType, objectiveCode, unityEvent);
        }
    }
}
