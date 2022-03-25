using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private AnnotationSystem annotationSystem;
    [SerializeField] private OVRInput.Button annotationButton;

    private GameObject _playerHead;
    private void Start()
    {
        _playerHead = GetComponent<Player>().PlayerHead;
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
                RaycastUtility.RaycastPosition(_playerHead.transform.position, _playerHead.transform.forward));
            
            // TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default); // DOESNT WORK :/
        }
    }
}
