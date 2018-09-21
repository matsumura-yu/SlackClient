using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class Model : MonoBehaviour{

    private ReactiveProperty<string> _value;
    public IReadOnlyReactiveProperty<string> ValueProperty
    {
        get
        {
            return _value;
        }
    }

    private IObservable<string> coroutine;

    
    // 環境変数を入れたScriptableObject作成
    ApplicationConfigs Env;


    //コンストラクタ
    public Model()
    {
        _value = new ReactiveProperty<string>();
        Env = new ApplicationConfigs();
        
    }
    
    private void SetValue(string value)
    {
        _value.Value = value;
    }
    // Send
    public void Send(string text)
    {
        // Sendボタン押した後の処理
        Debug.Log(text);
        //Monoを継承していないため使えない
        //StartCoroutine(SlackPost(text));

        //TODO：UniRx理解できてない
        //Observable.FromCoroutine<string>(observer => SlackPost(observer))
        //    .Subscribe

        //if(coroutine == null)
        //{
            Debug.Log("ストリームを定義します");
            coroutine = Observable.FromCoroutine<string>(observer => SlackPost(observer, text));
            //.Subscribe(_ => Debug.Log(_)); これはだめ
            coroutine.Subscribe(_ => Debug.Log(_));
       
        //}


       
        
    }
	
    public void Typing(string value)
    {
        Debug.Log("Typing");
        SetValue(value);
        
    }


    private IEnumerator SlackPost(IObserver<string> observer, string text)
    {
        Debug.Log("Slack送信します");
        

        // SlackにPostする処理を書く
        // UnityWebRequest

        var token = Env.TOKEN;
        var channel_id = Env.CHNNELID;

        string url = string.Format(
            "https://slack.com/api/chat.postMessage?token={0}&channel={1}&text={2}&as_user=true",
            token,
            channel_id,
            WWW.EscapeURL(text)
            );

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();


        Debug.Log("終了");

        Debug.Log("observer" + observer);
        Debug.Log(text);
    }

    

    
}
