using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractor : MonoBehaviour
{
    public Camera playerCam;
    private PuzzleManager puzzleStarter;
    public LayerMask layerItemSelect;
    private Interactable exitDoor;

    public static bool canInteract = false;

    public void AimAtThings()
    {
        Ray ray = playerCam.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, 10f, layerItemSelect))
        {
            puzzleStarter = hitinfo.collider.GetComponent<PuzzleManager>();
            exitDoor = hitinfo.collider.GetComponent<Interactable>();

            if (puzzleStarter != null && canInteract && !puzzleStarter.isSolved)
            {
                puzzleStarter.charPopup.enabled = true;
            }
            else if(exitDoor != null && canInteract && exitDoor.canInteract)
            {
                exitDoor.popupText.enabled = true;
            }
        }
        else
        {
            if (puzzleStarter != null)
            {
                puzzleStarter.charPopup.enabled = false;
                puzzleStarter = null;
            }
            if(exitDoor != null)
            {
                exitDoor.popupText.enabled = false;
                exitDoor = null;
            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AimAtThings();

        if(Input.GetKeyDown(KeyCode.E) && puzzleStarter != null && !puzzleStarter.isSolved && canInteract)
        {
            PlayerLook.SetMouseMover(false);
            puzzleStarter.StartPuzzle();
        }
        if(Input.GetKeyDown(KeyCode.E) && exitDoor != null && exitDoor.canInteract && canInteract)
        {
            PlayerLook.SetMouseMover(false);
            exitDoor.EndGameLeave();
        }
    }
}
