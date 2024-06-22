using System;
/*
{
  "id": "chatcmpl-0z9if3z3yxbz6g6u0mtlh",
  "object": "chat.completion",
  "created": 1718962466,
  "model": "crusoeai/dolphin-2.9.1-llama-3-8b-GGUF/dolphin-2.9.1-llama-3-8b.Q2_K.gguf",
  "choices": [
    {
      "index": 0,
      "message": { "role": "assistant", "content": "Hello there!\n\n" },
      "finish_reason": "stop"
    }
  ],
  "usage": {
    "prompt_tokens": 14,
    "completion_tokens": 3,
    "total_tokens": 17
  }
}
*/

[Serializable]
public class LmMessage
{
    public string role;
    public string content;
}

[Serializable]
public class LmChoice
{
    public int index;
    public string finish_reason;
    public LmMessage message;
}

[Serializable]
public class LmUsage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}

[Serializable]
public class AiLmJson
{
    public string id;
    public string created;
    public string model;
    public LmChoice[] choices;
    public LmUsage[] usage;
}

/*
[
    {
    "base64":null,
    "url":"http://127.0.0.1:8888/files/2024-06-21/ba63c11e-25b9-4287-b62e-c5c014337832-0.png",
    "seed":"9107773874759660789",
    "finish_reason":"SUCCESS"
    }
]
*/

[Serializable]
public class AiFoocusJson
{
    public string base64;
    public string url;
    public string seed;
    public string finish_reason;
}