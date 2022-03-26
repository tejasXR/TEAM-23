using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private SystemsManager systemsManager;
    [SerializeField] private OVRInput.Button annotationButton;

    private Player _player;
    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        OVRInput.Update();
        CreateAnnotationButton();
    }

    private void CreateAnnotationButton()
    {
        if (OVRInput.GetDown(annotationButton))
        {
            // systemsManager.StartAnnotationFlow();
            // System.TriggerManual("Confirm");
            systemsManager.ChangeFlowState(SystemsManager.FlowState.UserInput);
            
            // TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default); // DOESNT WORK :/
        }
    }
}
