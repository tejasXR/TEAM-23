using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreviewCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI previewText;

    public void SetText(string text)
    {
        previewText.text = text;
    }
}
