using System;
using System.Collections;
using System.Collections.Generic;
using a11y_Wizards.Scripts;
using GoogleCloudStreamingSpeechToText;
using UnityEngine;
using VRKeys;

public class SystemsManager : MonoBehaviour
{
    [SerializeField] private AnnotationSystem annotationSystem;
    [SerializeField] private StreamingRecognizer streamingRecognizer;
    [SerializeField] private SpeechManager speechManager;
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ScreenshotNetwork screenshotTaker;

    [SerializeField] private GameObject writingCanvas;
    [SerializeField] private PreviewCanvas previewCanvas;


    /*[SerializeField] private SpeechToTextHelper flowStartHelper;
    [SerializeField] private SpeechToTextHelper confirmPreviewHelper;*/

    // private bool _flowStarted;
    // private bool _feedbackWritten;
    // private bool _feedbackSubmitted;

    private string _timeStamp;
    
    public enum FlowState
    {
        AwaitingCall,
        WantsToInputFeedback,
        WantsToPreviewFeedback,
        WantsToSubmitFeedback,
        InputSentenceDone
    }

    public FlowState flowState { get; private set; }

    private void OnEnable()
    {
        playerInput.AnnotationButtonPressedCallback += StartAnnotationFlow;
        // flowStartHelper.

        // streamingRecognizer.onInterimResult.AddListener(UpdateKeyboardDisplayText);
    }

    private void OnDisable()
    {
        playerInput.AnnotationButtonPressedCallback -= StartAnnotationFlow;
        // streamingRecognizer.onInterimResult.RemoveListener(UpdateKeyboardDisplayText);
        // streamingRecognizer.onInterimResult.AddListener(System.OnSentenceUpdated);
    }

    public void StartAnnotationFlow()
    {
        if (flowState != FlowState.AwaitingCall) return;
        ChangeFlowState(FlowState.WantsToInputFeedback);
        StartCoroutine(AnnotationFlow());
    }

    private IEnumerator AnnotationFlow()
    {
        // Maybe not position keyboard 
        // PositionKeyboard();
        _timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
        StartCoroutine(EnableKeyboard());
        speechManager.SpeakWithSDKPlugin("Keyboard is enabled. Speak your annotation when ready.");
        Debug.Log("Keyboard is enabled, and waiting for user input");
        yield return new WaitUntil(() => flowState == FlowState.WantsToInputFeedback);
        WriteFeedback();
        Debug.Log("Keyboard updated with new text, waiting for confirm their feedback");
        yield return new WaitUntil(() => flowState == FlowState.InputSentenceDone);
        ChangeFlowState(FlowState.WantsToPreviewFeedback);
        PreviewFeedback();
        Debug.Log("User did confirm, waiting for user to preview and submit");
        yield return new WaitUntil(() => flowState == FlowState.WantsToSubmitFeedback);
        SubmitFeedback();
        Debug.Log("User submitted text");
    }

    
    
    public void ChangeFlowState(FlowState newState)
    {
        flowState = newState;
        /*switch (newState)
        {
            case FlowState.AwaitingCall:
                // Subscribe to player button press
                playerInput.AnnotationButtonPressedCallback += StartAnnotationFlow;
                break;
            case FlowState.UserInputtingFeedback:
                // Unsubscribe to button press
                playerInput.AnnotationButtonPressedCallback -= StartAnnotationFlow;

                /*System.RegisterAll(
                    ("confirm", () => ChangeFlowState(FlowState.AnnotationPreview)),
                    ("cancel", () => ))#1#
                break;
            case FlowState.FeedbackPreview:
                break;
        
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }*/
    }

    public void UpdateKeyboardDisplayText(string s)
    {
        if (!keyboard.initialized) return;
        var lower = s.ToLowerInvariant();
        if (lower == "confirm" || lower == "cancel" || lower == "create") return;
        if (flowState != FlowState.WantsToInputFeedback) return;
        keyboard.SetText(s);
    }

    private void PositionKeyboard()
    {
        var newPosition = playerInput.GetComponent<Player>().transform.forward + keyboard.positionRelativeToUser;
        keyboard.transform.position = newPosition;

        var playerDirection = playerInput.gameObject.transform.position;
        playerDirection.z = keyboard.transform.rotation.z;
        playerDirection.x = keyboard.transform.rotation.x;

        keyboard.transform.rotation = Quaternion.Euler(playerDirection.x, playerDirection.y, playerDirection.z);
    }
    
    public void Keyboard_EnterKeyPressed()
    {
        if (flowState == FlowState.WantsToInputFeedback)
            ChangeFlowState(FlowState.WantsToPreviewFeedback);
        else
        {
            ChangeFlowState(FlowState.WantsToSubmitFeedback);
        }
    }
    
    public void Keyboard_CancelKeyPressed()
    {
        CancelFeedback();
    }

    public void Voice_StartFeedback()
    {
        StartAnnotationFlow();
    }

    public void Voice_SentenceDone()
    {
        if (flowState == FlowState.WantsToInputFeedback && keyboard.text.ToLowerInvariant() != "create")
            ChangeFlowState(FlowState.InputSentenceDone);
    }

    public void Voice_PreviewFeedback()
    {
        if (flowState == FlowState.WantsToInputFeedback)
        {
            ChangeFlowState(FlowState.WantsToPreviewFeedback);

        } else if (flowState == FlowState.WantsToPreviewFeedback)
        {
            ChangeFlowState(FlowState.WantsToSubmitFeedback);
        }
    }

    public void Voice_CancelFeedback()
    {
        CancelFeedback();
    }

    private void WriteFeedback()
    {
        speechManager.SpeakWithSDKPlugin($"Waiting for text or voice input.");
        writingCanvas.SetActive(true);
        screenshotTaker.CaptureData("", _timeStamp);
        previewCanvas.gameObject.SetActive(false);
    }

    private void PreviewFeedback()
    {
        writingCanvas.SetActive(false);
        speechManager.SpeakWithSDKPlugin($"You wrote {keyboard.text}. Say confirm if this is correct, cancel if not.");
        previewCanvas.SetText(keyboard.text);
        previewCanvas.gameObject.SetActive(true);
    }

    private void SubmitFeedback()
    {
        var feedbackText = keyboard.text;
        screenshotTaker.UpdateAnnotation(feedbackText);
        screenshotTaker.SendData();
        
        // annotationSystem.CreateAnnotation(feedbackText);

        ResetSystem();
    }

    private void CancelFeedback()
    {
        // ChangeFlowState(FlowState.WantsToInputFeedback);
        ResetSystem();
    }

    private void ResetSystem()
    {
        // streamingRecognizer.onInterimResult.RemoveListener(UpdateKeyboardDisplayText);

        StopAllCoroutines();
        keyboard.Disable();
        keyboard.SetText("");
        
        ChangeFlowState(FlowState.AwaitingCall);
    }

    
    private IEnumerator EnableKeyboard()
    {
        yield return new WaitUntil(() => keyboard.initialized);
        keyboard.Enable();
    }
}
