using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindControl : MonoBehaviour
{
    public InputActionReference action;
    public bool movement;
    public int variant = 0;
    int bindingIndex;

    OverlappingControls overlap;
    TMP_Text bindingtext;
    Button button;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    
    // Start is called before the first frame update
    void Start()
    {
        overlap = FindObjectOfType<OverlappingControls>().GetComponent<OverlappingControls>();
        button = this.GetComponent<Button>();
        button.onClick.AddListener(StartRebinding);
        bindingtext = this.transform.GetChild(0).GetComponent<TMP_Text>();

        if (movement)
            bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[variant]);
        else
            bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);

        bindingtext.text = InputControlPath.ToHumanReadableString(
            action.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void StartRebinding()
    {
        bindingtext.text = "Enter Input";

        rebindingOperation = action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        bindingtext.text = InputControlPath.ToHumanReadableString(
            action.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();
        overlap.Ping();
    }
}
