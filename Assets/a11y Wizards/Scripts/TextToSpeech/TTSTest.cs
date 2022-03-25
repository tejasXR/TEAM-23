using System;
using UnityEngine;

namespace a11y_Wizards.Scripts {
    public class TTSTest : MonoBehaviour {
        [SerializeField] private string _input = "hello";
        [SerializeField] private SpeechManager _speech;

        private void OnEnable() {
            _speech.SpeakWithSDKPlugin(_input);
        }
    }
}