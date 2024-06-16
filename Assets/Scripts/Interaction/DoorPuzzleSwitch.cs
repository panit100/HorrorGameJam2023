using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzleSwitch : Door
{
    [Header("Puzzle")]
    [SerializeField] WirePuzzleController wirePuzzleController;

    [SerializeField] bool isPuzzleComplete;
    public bool IsPuzzleComplete => isPuzzleComplete;

    private void Start()
    {
        wirePuzzleController.gameObject.SetActive(false);
    }

    public override void TriggerDoor()
    {
        if (wirePuzzleController != null && isPuzzleComplete == false)
        {
            wirePuzzleController.gameObject.SetActive(true);
            PlayerManager.Instance.OnChangePlayerState(PlayerState.puzzle);
        }

        if (isPuzzleComplete == true)
        {
            OnActiveDoor();
            base.TriggerDoor();
        }
    }

    public void SetIsPuzzleComplete(bool value)
    {
        isPuzzleComplete = value;
    }
}
