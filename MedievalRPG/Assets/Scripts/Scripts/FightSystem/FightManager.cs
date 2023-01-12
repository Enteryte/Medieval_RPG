using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    public GameObject targetEnemyCanvasObj;

    public GameObject currTargetEnemy;

    [Header("Tutorials")]
    public TutorialBaseProfile doARollTutorial;
    public TutorialBaseProfile shildBlockTutorial;

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
        if (currTargetEnemy != null)
        {
            if (Vector3.Distance(currTargetEnemy.transform.position, GameManager.instance.playerGO.transform.position) 
                > GameManager.instance.playerGO.GetComponent<Interacting>().viewRadius)
                             {
                currTargetEnemy = null;

                targetEnemyCanvasObj.SetActive(false);
            }
        }
    }

    public void TargetEnemy(GameObject currInteractTarget)
    {
        if (currTargetEnemy == currInteractTarget)
        {
            currTargetEnemy = null;

            targetEnemyCanvasObj.SetActive(false);
        }
        else
        {
            currTargetEnemy = currInteractTarget;

            targetEnemyCanvasObj.SetActive(true);
        }
    }
}
