using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandLights : SequencerCommand
    { // Rename to SequencerCommand<YourCommand>

        public void Awake()
        {
            Debug.Log("sequencer baby");
            GameObject lightParent = GameObject.Find("SceneLights");
            foreach (Light light in lightParent.GetComponentsInChildren<Light>())
            {
                light.color = Color.red;
            }
            Stop();
        }

    }

}
