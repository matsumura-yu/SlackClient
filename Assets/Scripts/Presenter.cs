using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Presenter : MonoBehaviour {

    [SerializeField]
    private View _view;

    private Model _model;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _model = new Model();

        _view.Initialize();

        Bind();
        SetEvents();
    }

    private void Bind()
    {
        _model.ValueProperty
            .Subscribe(_view.OnTextChanged)
            .AddTo(gameObject);
    }

    private void SetEvents()
    {
        _view.OnSendButtonClickedCallback = OnSendButtonClicked;
        _view.OnInputFieldTypedCallback = OnInputFieldTyped;
    }

    private void OnInputFieldTyped(string value)
    {
        _model.Typing(value);
        
    }

    private void OnSendButtonClicked(string text)
    {
        // viewからの操作
        _model.Send(text);
    }
}
