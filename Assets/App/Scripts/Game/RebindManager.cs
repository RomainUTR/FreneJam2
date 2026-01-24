using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    public InputActionReference actionToRebind;
    public TMP_Text buttonText;
    public string bindingName = "Thrust";

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionToRebind.action.LoadBindingOverridesFromJson(rebinds);
        }

        UpdateUI();
    }

    public void StartRebinding()
    {
        buttonText.text = "Press a key...";
        actionToRebind.action.Disable();
        rebindingOperation = actionToRebind.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishRebinding())
            .Start();
    }

    void FinishRebinding()
    {
        rebindingOperation.Dispose();
        actionToRebind.action.Enable();

        UpdateUI();

        string rebinds = actionToRebind.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();

        Debug.Log("New input saved");
    }

    void UpdateUI()
    {
        string keyName = actionToRebind.action.GetBindingDisplayString(0);
        Debug.Log(keyName);
        buttonText.text = $"{bindingName}: [{keyName}]";
    }
}
