using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private AnnotationSystem annotationSystem;
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
            annotationSystem.CreateAnnotation("", 
                RaycastUtility.RaycastPosition(_player.PlayerHead.transform.position, 
                    _player.PlayerHead.transform.forward));
            
            // TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default); // DOESNT WORK :/
        }
    }
}
