using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractor : MonoBehaviour
{
    public Camera playerCam;
    private PuzzleManager puzzleStarter;
    public LayerMask layerItemSelect;

    public void AimAtThings()
    {
        Ray ray = playerCam.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, 10f, layerItemSelect))
        {
            puzzleStarter = hitinfo.collider.GetComponent<PuzzleManager>();

            if (puzzleStarter != null)
            {
                puzzleStarter.charPopup.enabled = true;
            }
        }
        else
        {
            if (puzzleStarter != null)
            {
                puzzleStarter.charPopup.enabled = false;
                puzzleStarter = null;
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

        if(Input.GetKeyDown(KeyCode.E) && puzzleStarter != null && !puzzleStarter.isSolved)
        {
            PlayerLook.SetMouseMover(false);
            puzzleStarter.StartPuzzle();
        }
    }
}
