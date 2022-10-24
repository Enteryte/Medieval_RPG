using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public PlayableDirector playableDirector;

    public CutsceneProfile currCP;

    public float timeToPressToSkipCS = 3;
    public float pressedTime = 0;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playableDirector.playableAsset != null)
        {
            if (Input.GetKey(KeyCode.Escape) && currCP.isNotADialogue)
            {
                if (!GameManager.instance.playedTheGameThrough)
                {
                    pressedTime += Time.deltaTime;

                    if (pressedTime >= timeToPressToSkipCS)
                    {
                        SkipCutscene(currCP.timeTillWhereToSkip);

                        pressedTime = 0;
                    }
                }
                else
                {
                    SkipCutscene(currCP.timeTillWhereToSkip);
                }   
            }

            if (Input.GetKeyDown(KeyCode.Return) && !currCP.isNotADialogue)
            {
                SkipSentenceInDialogue();
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                pressedTime = 0;
            }
        }
    }

    public void StartPlayingCutscene()
    {
        playableDirector.Play(currCP.cutscene);
    }

    public void SkipSentenceInDialogue()
    {
        for (int i = 0; i < currCP.timesWhenNewSentenceStarts.Count; i++)
        {
            if (playableDirector.time < currCP.timesWhenNewSentenceStarts[i])
            {
                playableDirector.time = currCP.timesWhenNewSentenceStarts[i];

                return;
            }
        }
    }

    public void SkipCutscene(float timeTillWhereToSkip)
    {
        playableDirector.time = timeTillWhereToSkip;
    }
}
