using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class AiConfig
{
    public const string LmAddress = "http://localhost:1234/v1/chat/completions";
    public const string FoocusAddress = "http://127.0.0.1:8888";
    public const string FoocusTxt2Img = "/v1/generation/text-to-image";
    public const string TtsAddress = "http://127.0.0.1:5000";
}

public class AiAssistant : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputPrompt;
    [SerializeField] 
    TMP_InputField foocuPrompt;
    [SerializeField]
    TMP_Text answerText;
    [SerializeField]
    AudioSource sound;
    [SerializeField]
    Image assistantImage;

    IEnumerator SendRequest(string url, string postData, Action<string> callback)
    {
        using UnityWebRequest request = UnityWebRequest.Post(url, postData, "Content-Type: application/json");

        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result.HasFlag(UnityWebRequest.Result.ConnectionError))
        {
            string error = "ERROR: " + request.error + " url: " + url + " data: " + postData;
            Debug.LogError(error);
            callback.Invoke(error);
            yield break;
        }

        callback.Invoke(request.downloadHandler.text);

    }

    public void LM()
    {

        string postData = @"
        {
            ""messages"":[
                  { ""role"": ""system"", ""content"": ""You are blacksmith. Your name is Martins. You are 46 years old. You can make a sword and give it to me. You known Emily, she living in Lizardtown near by church.""},
                  { ""role"": ""user"", ""content"": ""%prompt%""}
            ],
            ""temperature"": 0.7,
            ""stream"": false,
            ""max_tokens"": -1
        }
        ".Replace("%prompt%", inputPrompt.text);

        InsertAnswer("<color=blue>" + inputPrompt.text + "</color>");

        Debug.Log(postData);

        StartCoroutine(SendRequest(AiConfig.LmAddress, postData, (content) => { 
        
            if (content.IndexOf("ERROR:") > -1)
            {
                InsertAnswer("<color=red>" + content + "</color>");
                return;
            }

            AiLmJson json = JsonUtility.FromJson<AiLmJson>(content);
            string answer = json.choices[0].message.content;

            InsertAnswer(answer);

            answer = answer.Replace("\n", " ").Replace("\r", "").Trim();

            postData = "{ \"message\": \"%data%\"}".Replace("%data%", answer);

            Debug.Log(postData);
            //StartCoroutine(SendRequest(AiConfig.TtsAddress + "/tts", postData, (conent) =>
            //{
            //    if (content.IndexOf("ERROR:") > -1)
            //        return;

            //    StartCoroutine(GetSound());

            //}));

        }));
    }

    IEnumerator GetSound()
    {
        using UnityWebRequest clip = UnityWebRequestMultimedia.GetAudioClip(AiConfig.TtsAddress + "/audio", AudioType.WAV);
        
        yield return clip.SendWebRequest();
        
        if (clip.result == UnityWebRequest.Result.ConnectionError)
            yield break;

        sound.clip = DownloadHandlerAudioClip.GetContent(clip);
        sound.Play();
    }


    public void Foocus()
    {
        //t2i_params = {
        //    "prompt": "a dog",
        //    "performance_selection": "Lightning",
        //    "aspect_ratios_selection": "896*1152",
        //    "async_process": False
        //}
       string postData = @"
        {
            ""prompt"": ""%prompt%"",
            ""performance_selection"": ""Lightning"",
            ""aspect_ratios_selection"": ""1024*1024"",
            ""async_process"": ""false""
        }
        ".Replace("%prompt%", foocuPrompt.text);

        StartCoroutine(SendRequest(AiConfig.FoocusAddress + AiConfig.FoocusTxt2Img, postData, (content) =>
        {


            if (content.IndexOf("ERROR:") > -1)
            {
                InsertAnswer("<color=red>" + content + "</color>");
                return;
            }

            string answer = content.Replace("[", "").Replace("]", "");

            InsertAnswer(answer);

            AiFoocusJson json = JsonUtility.FromJson<AiFoocusJson>(answer);

            StartCoroutine(GetImage(json.url));
        }));
    }

    IEnumerator GetImage(string path)
    {
        UnityWebRequest image = UnityWebRequestTexture.GetTexture(path);
        yield return image.SendWebRequest();


        Texture2D texture = ((DownloadHandlerTexture)image.downloadHandler).texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));

        assistantImage.sprite = sprite;
    }


    void InsertAnswer(string message)
    {
        answerText.text = answerText.text.Insert(0, message + "\n");
    }
}
