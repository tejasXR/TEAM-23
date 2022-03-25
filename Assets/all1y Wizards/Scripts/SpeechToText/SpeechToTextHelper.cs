using System;
using GoogleCloudStreamingSpeechToText;
using UnityEngine;
using UnityEngine.Events;

public class SpeechToTextHelper : MonoBehaviour {
    [SerializeField] private StreamingRecognizer recognizer;
    [SerializeField] private string wakeWord = "Annotate";

    [SerializeField] private TranscriptionEvent onAnnotationUpdated;
    [SerializeField] private UnityEvent onAnnotationComplete;
    
    private void OnEnable() {
        recognizer.onInterimResult.AddListener(_OnInterimSentence);
        recognizer.onFinalResult.AddListener(_OnFinalSentence);
    }

    private void OnDisable() {
        recognizer.onInterimResult.RemoveListener(_OnInterimSentence);
        recognizer.onFinalResult.RemoveListener(_OnFinalSentence);

    }

    private void _OnInterimSentence(string sentence) {
        if (!sentence.StartsWith(wakeWord)) return;
        var sentenceWithoutWakeWord = sentence.Substring(wakeWord.Length);
        onAnnotationUpdated?.Invoke(sentenceWithoutWakeWord);
    }

    private void _OnFinalSentence(string sentence) {
        if (!sentence.StartsWith(wakeWord)) return;
        _OnInterimSentence(sentence);
        onAnnotationComplete?.Invoke();
    }
}