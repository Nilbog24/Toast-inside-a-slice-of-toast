using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{

    public static NovelController instance;
    List<string> data = new List<string>();
    int progress = 0;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadChapterFile("chapter0_start");
    }

    // Update is called once per frame
    void Update()
    {
        //testing
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }
    void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        progress = 0;
        cachedLastSpeaker = "";

        if(handlingChapterFile != null)
            StopCoroutine(handlingChapterFile);
        handlingChapterFile = StartCoroutine(HandlingChapterFile());
    }

    bool _next = false;
    public void Next()
    {
        _next = true;
    }

    Coroutine handlingChapterFile = null;
    IEnumerator HandlingChapterFile()
    {
        int progress = 0;

        while(progress < data.Count)
        {
            if(_next)
            {
                HandleLine(data[progress]);
                progress++;
                while(isHandlingLine)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void HandleLine(string rawLine)
    {
        CLM.LINE line = CLM.Interpret(rawLine);
        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine()
    {
        if(isHandlingLine)
        {
            StopCoroutine(handlingLine);
        }
        handlingLine = null;
    } 

    public bool isHandlingLine{get{return handlingLine != null;}}
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(CLM.LINE line)
    {
        _next = false;
        int lineProgress = 0;

        while(lineProgress < line.segments.Count)
        {
            _next = false;
            CLM.LINE.SEGMENT segment = line.segments[lineProgress];

            if(lineProgress > 0)
            {
                if(segment.trigger == CLM.LINE.SEGMENT.TRIGGER.autoDelay)
                {
                    for(float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    while(!_next)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            _next = false;

            segment.Run();

            while(segment.isRunning)
            {
                yield return new WaitForEndOfFrame();
                if(_next)
                {
                    if(!segment.architect.skip)
                    {
                        segment.architect.skip = true;
                    }
                    else
                    {
                        segment.ForceFinish();
                    }
                }
            }

            lineProgress++;

            yield return new WaitForEndOfFrame();
        }

        handlingLine = null;
    }


    
    [HideInInspector]
    public string cachedLastSpeaker = "";
   
    void HandleAction(string action)
    {
        string[] data = action.Split('(',')');

        switch(data[0])
        {
            case("setBackground"):
                Command_SetLayerImage(data[1], BCFC.instance.background);
                break;
            case("setCinematic"):
                Command_SetLayerImage(data[1], BCFC.instance.cinematic);
                break;
            case("setForground"):
                Command_SetLayerImage(data[1], BCFC.instance.foreground);
                break;
            case("playSound"):
                Command_PlaySound(data[1]);
                break;
            case("playMusic"):
                Command_PlayMusic(data[1]);
                break;
            case("move"):
                Command_MoveCharacter(data[1]);
                break;
            case("setPosition"):
                Command_SetPosition(data[1]);
                break;
            case("setFace"):
                Command_SetFace(data[1]);
                break;
            case("setBody"):
                Command_SetBody(data[1]);
                break;
            case("flip"):
                Command_Flip(data[1]);
                break;
            case("faceLeft"):
                Command_FaceLeft(data[1]);
                break;
            case("faceRight"):
                Command_FaceRight(data[1]);
                break;
            case("transBackground"):
                Command_TransLayer(BCFC.instance.background, data[1]);
                break;
            case("transCinematic"):
                Command_TransLayer(BCFC.instance.cinematic, data[1]);
                break;
            case("transForeground"):
                Command_TransLayer(BCFC.instance.foreground, data[1]);
                break;
            case("showScene"):
                Command_ShowScene(data[1]);
                break;
        }

        
    }

    void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = texName == "null" ? null : Resources.Load("Backgrounds/" + texName) as Texture2D;
        float spd = 2f;
        bool smooth = false;

        if(data.Contains(","))
        {
            string[] paramaters = data.Split(',');
            foreach(string p in paramaters)
            {
                float fVal = 0;
                bool bVal = false;
                if(float.TryParse(p, out fVal))
                {
                    spd = fVal; continue;
                }
                    
                if(bool.TryParse(p, out bVal))
                {
                    smooth = bVal; continue;
                }
            }
        }

        layer.TransitionToTexture(tex, spd, smooth); 
    }
    void Command_PlaySound(string data)
    {   
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;
        if(clip != null)
            AudioManager.instance.PlaySFX(clip);
        else
            Debug.LogError("clip does not exist - " +data);

    }

    void Command_PlayMusic(string data)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;
        if(clip != null)
            AudioManager.instance.PlaySong(clip);
        else
            Debug.LogError("clip does not exist - " +data);
    }

    void Command_MoveCharacter(string data)
    {
        string[] paramaters = data.Split(',');
        string character = paramaters[0];
        float locationX = float.Parse(paramaters[1]);
        float locationY = paramaters.Length >= 3 ? float.Parse(paramaters[2]): 0;
        float speed = paramaters.Length >= 4 ? float.Parse(paramaters[3]) : 1f;
        bool smooth = paramaters.Length == 5 ? bool.Parse(paramaters[4]) : true;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed, smooth);
        
    }

    void Command_SetPosition(string data)
    {
        string[] paramaters = data.Split(',');
        string character = paramaters[0];
        float locationX = float.Parse(paramaters[1]);
        float locationY = float.Parse(paramaters[2]);

        Character c = CharacterManager.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));   

    }
    void Command_ChangeExpression(string data)
    {
        string[] paramaters = data.Split(',');
        string character = paramaters[0];
        string region = paramaters[1];
        string expression = paramaters[2];
        float speed = paramaters.Length == 4 ? float.Parse(paramaters[3]) : 1f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);
        if (region.ToLower() == "body")
            c.TransitionBody(sprite, speed, false);
        if (region.ToLower() == "face")
            c.TransitionExpression(sprite, speed, false);

    }

    void Command_SetFace(string data)
    {
        string[] paramaters = data.Split(',');
        string character = paramaters[0];
        string expression = paramaters[1];
        float speed = paramaters.Length == 3 ? float.Parse(paramaters[2]) : 1f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        c.TransitionExpression(sprite, speed, false);

    }

    void Command_SetBody(string data)
    {
        string[] paramaters = data.Split(',');
        string character = paramaters[0];
        string expression = paramaters[1];
        float speed = paramaters.Length == 3 ? float.Parse(paramaters[2]) : 1f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        c.TransitionBody(sprite, speed, false);

    }


    void Command_Flip(string data)
    {
        string[] characters = data.Split(',');
        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.Flip();
        }
        
    }

    void Command_FaceLeft(string data)
    {
        string[] characters = data.Split(',');
        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FaceLeft();
        }
        
    }

    void Command_FaceRight(string data)
    {
        string[] characters = data.Split(',');
        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FaceRight();
        }
        
    }

    void Command_Exit(string data)
    {
        string[] paramaters = data.Split(',');
        string[] characters = paramaters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for(int i = 1; i < paramaters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if(float.TryParse(paramaters[i], out fVal))
            {
                speed = fVal; continue;
            }
            if(bool.TryParse(paramaters[i], out bVal))
            {
                smooth = bVal; continue;
            }
        }

        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FadeOut(speed, smooth);
        }
    }

    void Command_Enter(string data)
    {
        string[] paramaters = data.Split(',');
        string[] characters = paramaters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for(int i = 1; i < paramaters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if(float.TryParse(paramaters[i], out fVal))
            {
                speed = fVal; continue;
            }
            if(bool.TryParse(paramaters[i], out bVal))
            {
                smooth = bVal; continue;
            }
        }

        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s, true, false);
            if(!c.enabled)
            {
                c.renderers.bodyRenderer.color = new Color(1,1,1,0);
                c.renderers.expressionRenderer.color = new Color(1,1,1,0);
                c.enabled = true;

                c.TransitionBody(c.renderers.bodyRenderer.sprite,speed,smooth);
                c.TransitionExpression(c.renderers.expressionRenderer.sprite,speed,smooth);
            }
            else
                c.FadeIn(speed, smooth);
        }
    }

    void Command_TransLayer(BCFC.LAYER layer, string data)
    {
        string[] paramaters = data.Split(',');

        string texName = paramaters[0];
        string transTexName = paramaters[1];
        Texture2D tex = texName == "null" ? null : Resources.Load("Backgrounds" + texName) as Texture2D;
        Texture2D transTex = Resources.Load("Backgrounds" + transTexName) as Texture2D;

        float spd = 2f;
        bool smooth = false;

        for(int i = 2; i < paramaters.Length; i++)
        {
            string p = paramaters[i];
            float fVal = 0;
            bool bVal = false;
            if(float.TryParse(p, out fVal))
            {
                spd = fVal; continue;
            }
            if(bool.TryParse(p, out bVal))
            {
                smooth = bVal; continue;
            }
        }

        TransitionMaster.TransitionLayer(layer, tex, transTex, spd, smooth);
    }

    void Command_ShowScene(string data)
    {
        string[] paramaters = data.Split(',');
        bool show = bool.Parse(paramaters[0]);
        string texName = paramaters[1];
        Texture2D transTex = Resources.Load("Backgrounds" + texName) as Texture2D;

        float spd = 2f;
        bool smooth = false;

        for(int i = 2; i < paramaters.Length; i++)
        {
            string p = paramaters[i];
            float fVal = 0;
            bool bVal = false;
            if(float.TryParse(p, out fVal))
            {
                spd = fVal; continue;
            }
            if(bool.TryParse(p, out bVal))
            {
                smooth = bVal; continue;
            }
        }

        TransitionMaster.ShowScene(show, spd, smooth, transTex);
    }
}
