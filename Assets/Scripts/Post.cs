using System.Collections;
using System.Collections.Generic;
using Twity.DataModels.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Post : MonoBehaviour {
    
    public InputField inputField;
    // 環境変数を入れたScriptableObject作成
    ApplicationConfigs Env;

    // Use this for initialization
    void Start () {
        //message = text.GetComponent<Text>();
        Env = new ApplicationConfigs();

        // Twitter設定
        Twity.Oauth.consumerKey = Env.CONSUMER_KEY;
        Twity.Oauth.consumerSecret = Env.CONSUMER_SECRET;
        Twity.Oauth.accessToken = Env.ACCESS_TOKEN;
        Twity.Oauth.accessTokenSecret = Env.ACCESS_TOKEN_SECRET;
    }


    void Callback(bool success, string response)
    {
        if (success)
        {
            Tweet tweet = JsonUtility.FromJson<Tweet>(response);
            Debug.Log(tweet);
        }
        else
        {
            Debug.Log(response);
        }
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log(inputField.text);
                
                //StartCoroutine(SlackPost(message.text));

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["status"] = "#OC5 " + inputField.text;  // ツイートするテキスト
                //StartCoroutine(Twity.Client.Post("statuses/update", parameters, this.Callback));
            }
                
        }
	}

    private IEnumerator SlackPost(string postMessage)
    {
        var token = Env.TOKEN;
        var channel_id = Env.CHNNELID;

        string url = string.Format(
            "https://slack.com/api/chat.postMessage?token={0}&channel={1}&text={2}&as_user=true",
            token,
            channel_id,
            WWW.EscapeURL(postMessage)
            );

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
    }

    private IEnumerator TwitterPost(string postMessage) {
        Debug.Log("TwitterでのPost: " + postMessage);
        yield return null;

    }
}
