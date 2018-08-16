using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour {

    [SerializeField]
    private Button _sendButton;

    [SerializeField]
    private InputField _inputField;

    [SerializeField]
    private Text _status;

    //Sendボタンが押された時のコールバック
    public Action<string> OnSendButtonClickedCallback;

    public Action<string> OnInputFieldTypedCallback;

    public void OnTextChanged(string value)
    {
        _status.text = "Matsumura is typing...";
    }

    public void Initialize()
    {
        _sendButton.onClick.AddListener(OnSendButtonClicked);
        _inputField.onValueChanged.AddListener(OnInputFieldTyped);
    }

    public void OnInputFieldTyped(string value)
    {
        if(OnInputFieldTypedCallback != null)
        {
            OnInputFieldTypedCallback(value);
        }
    }

    public void OnSendButtonClicked()
    {
        if(OnSendButtonClickedCallback != null)
        {
            
            OnSendButtonClickedCallback(_inputField.text);
        }
    }
    
}
