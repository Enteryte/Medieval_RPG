using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BookHandler : MonoBehaviour
{
    [TextArea(0, 50)] public string[] allBookPageEntries;

    public TMP_Text leftPageTxt;
    public TMP_Text rightPageTxt;

    public TMP_Text leftPageNumberTxt;
    public TMP_Text rightPageNumberTxt;

    public Image leftArrowImg;
    public Image rightArrowImg;

    public int currPageNumber = 1;

    public bool wasNextLastClick = false;

    public void Start()
    {
        StartDisplayingPages();
    }

    public void Update()
    {
        Debug.Log(currPageNumber);
    }

    public void StartDisplayingPages()
    {
        currPageNumber = 1;

        leftPageTxt.text = allBookPageEntries[0];
        leftPageNumberTxt.text = (currPageNumber).ToString();

        //currPageNumber += 1;

        rightPageTxt.text = allBookPageEntries[1];
        rightPageNumberTxt.text = (currPageNumber + 1).ToString();

        //currPageNumber += 1;

        leftArrowImg.enabled = false;

        rightArrowImg.enabled = true;

        wasNextLastClick = true;
    }

    public void DisplayPagesNext()
    {
        currPageNumber += 2;

        if (currPageNumber > 1)
        {
            leftArrowImg.enabled = true;
        }
        else
        {
            leftArrowImg.enabled = false;
        }

        if (currPageNumber < 19)
        {
            rightArrowImg.enabled = true;
        }
        else
        {
            rightArrowImg.enabled = false;

            currPageNumber = 19;
        }

        ////////////if (currPageNumber > 0)
        ////////////{
        ////////////    currPageNumber += 1;
        ////////////}
        /////

        //if (!wasNextLastClick)
        //{
        //    currPageNumber += 3;

        //    wasNextLastClick = true;
        //}

        //if (currPageNumber < allBookPageEntries.Length)
        //{

        //if (currPageNumber > 3)
        //{
        //    leftPageTxt.text = allBookPageEntries[currPageNumber - 2];
        //    leftPageNumberTxt.text = (currPageNumber).ToString();
        //}
        //else
        //{
            leftPageTxt.text = allBookPageEntries[currPageNumber - 1];
            leftPageNumberTxt.text = (currPageNumber).ToString();
        //}

        //}

        //if (currPageNumber < allBookPageEntries.Length)
        //{
            rightPageTxt.text = allBookPageEntries[currPageNumber];
            rightPageNumberTxt.text = (currPageNumber + 1).ToString();
        //}

        //if (currPageNumber > 0)
        //{
        //    leftArrow.SetActive(true);
        //}
        //else
        //{
        //    leftArrow.SetActive(false);
        //}

        //if (currPageNumber <= allBookPageEntries.Length)
        //{
        //    rightArrow.SetActive(true);
        //}
        //else
        //{
        //    rightArrow.SetActive(false);

        //    currPageNumber = allBookPageEntries.Length - 1;
        //}
    }

    public void DisplayPagesBack()
    {
        currPageNumber -= 2;

        //Debug.Log("gzhjkm,");

        //if (wasNextLastClick)
        //{
        //    currPageNumber -= 3;

        //    wasNextLastClick = false;
        //}
        ////else
        ////{

        if (currPageNumber > 1)
        {
            leftArrowImg.enabled = true;
        }
        else
        {
            leftArrowImg.enabled = false;

            currPageNumber = 1;
        }

        if (currPageNumber < allBookPageEntries.Length)
        {
            rightArrowImg.enabled = true;
        }
        else
        {
            rightArrowImg.enabled = false;
        }

        //if (currPageNumber >= 0)
        //{

            rightPageTxt.text = allBookPageEntries[currPageNumber];
            rightPageNumberTxt.text = (currPageNumber + 1).ToString();
        //}

        //if (currPageNumber >= 0)
        //{
            leftPageTxt.text = allBookPageEntries[currPageNumber - 1];
            leftPageNumberTxt.text = (currPageNumber).ToString();
        //}

        ////}


        ////else
        ////{
        ////    currPageNumber = 19;
        ////}       
    }
}
