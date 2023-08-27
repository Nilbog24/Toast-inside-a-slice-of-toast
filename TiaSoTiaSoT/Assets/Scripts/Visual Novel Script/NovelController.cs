using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{

    List<string> data = new List<string>();
    int progress = 0;
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
            HandleLine(data[progress]);
            progress++;
        }
    }
    void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        progress = 0;
        cachedLastSpeaker = "";
    }

    void HandleLine(string line)
    {
        string[] dialogueAndActions = line.Split('"');

        if(dialogueAndActions.Length == 3)
        {
            HandleDialogue(dialogueAndActions[0], dialogueAndActions[1]);
            HandleEventsFromLine(dialogueAndActions[2]);
        }
        else
        {
            HandleDialogue(dialogueAndActions[0]);
        }
    }
    string cachedLastSpeaker = "";
    void HandleDialogue(string dialogueDetails, string dialogue)
    {
        string speaker = cachedLastSpeaker;
        bool additive = dialogueDetails.Contains("+");

        if(additive)
            dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length-1);

        if(dialogueDetails.Length > 0)
        {
            if(dialogueDetails[dialogueDetails.Length-1] == ' ')
                dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length-1);

            speaker = dialogueDetails;
            cachedLastSpeaker = speaker;
        }

        if(speaker != "narrator")
        {
            Character character = CharacterManager.instance.GetCharacter(speaker);
            character.Say(dialogue, additive);
        }
        else
        {
            DialogueSystem.instance.Say(dialogue, speaker, additive);
        }
    }

    void HandleEventsFromLine(string events)
    {
        string[] actions = events.Split(' ');

        foreach(string action in actions)
        {
            HandleAction(action);
        }
    }

    void HandleAction(string action)
    {
        string[] data = action.Split('(',')');

        if(data[0] == "setBackground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.background);
            return;
        }
        if(data[0] == "setCinematic")
        {
            Command_SetLayerImage(data[1], BCFC.instance.cinematic);
            return;
        }
        if(data[0] == "setForground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.foreground);
            return;
        }
        if(data[0] == "playSound")
        {
            Command_PlaySound(data[1]);
            return;
        }
        if(data[0] == "playMusic")
        {
            Command_PlayMusic(data[1]);
            return;
        }
        if(data[0] == "move")
        {
            Command_MoveCharacter(data[1]);
            return;
        }
        if(data[0] == "setPosition")
        {
            Command_SetPosition(data[1]);
            return;
        }
        if(data[0] == "changeExpression")
        {
            Command_ChangeExpression(data[1]);
            return;
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
        float locationY = float.Parse(paramaters[2]);
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
}
