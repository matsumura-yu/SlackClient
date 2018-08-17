using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class SlackApiTest : MonoBehaviour
{

    ApplicationConfigs Env;

    // Use this for initialization
    void Start()
    {
        Env = new ApplicationConfigs();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("ChannelListの取得");
            StartCoroutine(GetSlackChannel());

            Debug.Log("Coroutineの↓のコード");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Slack投稿");
            StartCoroutine(PostSlackChannel());

            Debug.Log("Coroutineの↓のコード");
        }

    }

    private IEnumerator GetSlackChannel()
    {
        var token = Env.TOKEN;
        string url = string.Format(
            "https://slack.com/api/channels.list?token={0}",
            token

            );

        UnityWebRequest req = UnityWebRequest.Get(url);
        // yield returnした後ろのコードはレスポンスが返ってきたら処理が実行
        // 呼び出し先に戻る
        yield return req.SendWebRequest();

        var data = req.downloadHandler.text;
        //Stringデータで返ってくる
        //Debug.Log(data);
        //Debug.Log((req.downloadHandler.text).GetType());


        // Jsonクラス変換
        JsonClass jsonClass = JsonUtility.FromJson<JsonClass>(data);
        Debug.Log(jsonClass.Ok);
        Debug.Log(jsonClass.Value);
    }

    private IEnumerator PostSlackChannel()
    {
        var text = "test";
        var token = Env.TOKEN;
        var channel = Env.CHNNELID;

        /*
        string url = string.Format(
            "https://slack.com/api/chat.postMessage?token={0}&channel={1}&text={2}",
            token,
            channel,
            text
            );
    */
        string url = "https://slack.com/api/chat.postMessage";

        UnityWebRequest req = new UnityWebRequest(url, "POST");

        var json = "{\"channel\":\"C7Y6ZG917\",\"text\": \"I am a test message http://slack.com\",\"attachments\": [{\"text\": \"And here’s an attachment!\"}]}";

        byte[] body = Encoding.UTF8.GetBytes(json);

        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // ヘッダーの設定
        req.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        string auth = "Bearer " + token;
        req.SetRequestHeader("Authorization", auth);


        yield return req.SendWebRequest();

        var data = req.downloadHandler.text;

        Debug.Log(data);
    }


    // 返ってくるJsonデータの欲しい部分だけを記述すればよい
    [Serializable]
    public class JsonClass
    {

        [SerializeField]
        private int value;
        public int Value { get { return value; } }

        [SerializeField]
        private string ok;
        public string Ok { get { return ok; } }

    }

    //Getしたチャンネルリストのクラス
    //編集>形式を選択肢て貼り付け> Jsonを行うと自動生成できる

    /*
    public class Rootobject
    {
        public bool ok { get; set; }
        public Channel[] channels { get; set; }
    }

    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool is_channel { get; set; }
        public int created { get; set; }
        public bool is_archived { get; set; }
        public bool is_general { get; set; }
        public int unlinked { get; set; }
        public string creator { get; set; }
        public string name_normalized { get; set; }
        public bool is_shared { get; set; }
        public bool is_org_shared { get; set; }
        public bool is_member { get; set; }
        public bool is_private { get; set; }
        public bool is_mpim { get; set; }
        public string[] members { get; set; }
        public Topic topic { get; set; }
        public Purpose purpose { get; set; }
        public string[] previous_names { get; set; }
        public int num_members { get; set; }
    }

    public class Topic
    {
        public string value { get; set; }
        public string creator { get; set; }
        public int last_set { get; set; }
    }

    public class Purpose
    {
        public string value { get; set; }
        public string creator { get; set; }
        public int last_set { get; set; }
    }

    */
}
