using Facebook.Unity;
using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Main MainC;
    
    private void Awake()
    {
        MainC.OpenMenu();

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback/*, OnHideUnity*/);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    
    public void Training()
    {
        MainC.OpenTraining();
    }
    public void Close()
    {
        if (gameObject.activeSelf)
        {
            //burasý hala gerekli mi
            Application.Quit();
        }
        else if(MainC.Game.activeSelf)
        {
            MainC.OpenMenu();
            if (!MainC.Won.activeSelf)
            {
                MainC.Score0();
            }
            if (MainC.TrainingLevels|| MainC.Won.activeSelf|| MainC.NextLevelBText.text == "Try Again")
            {
                MainC.CloseGameGuide();
            }
            
        }
        
    }
    public void Restart()
    {
        MainC.Restart();
    }
    public void ShowMathPoints()
    {
        MainC.ShowMathPoints();
        
    }

    private void ShareCallback(IShareResult result)
    {
        //optimize ediliebilir:
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
            MainC.ShowGameGuide(result.Error, true, false);
            MainC.isProcessing = true;
            StartCoroutine(MainC.CloseIsProccessing(0.2f));
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
            MainC.ShowGameGuide(result.Error, true, false);
            MainC.isProcessing = true;
            StartCoroutine(MainC.CloseIsProccessing(0.2f));
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
            MainC.GainPlayTicket(10);
            PlayerPrefs.SetInt("FacebookShared", 1);
            MainC.FacebookGift.enabled = false;
            MainC.FacebookButton.interactable = false;
            MainC.isProcessing = true;
            StartCoroutine(MainC.CloseIsProccessing(0.2f));
        }

    }

    public void FacebookShare()
    {

        MainC.CloseGameGuide();
        FB.ShareLink(new Uri("https://play.google.com/store/apps/details?id=com.KaanSelcukKaraboce.ArcherEquations"), callback: ShareCallback, contentTitle: "Do some brain exercise! & Play to see your math level!");
        //        FB.FeedShare(
        //  link: "https://example.com/myapp/?storyID=thelarch",
        //  linkName: "The Larch",
        //  callback: FeedCallback
        //);
        //Eski mesaj : "A fun, targeting and mathematical game : "
        
        MainC.isProcessing = true;
        StartCoroutine(MainC.CloseIsProccessing(2));
    }
    public void InstagramShare()
    {

    }


    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }









    //private void CloseIsProcessing(bool isGameShown)
    //{
    //    MainC.isProcessing = false;
    //}
    //private void OnHideUnity(bool isGameShown)
    //{
    //    if (!isGameShown)
    //    {
    //        // Pause the game - we will need to hide
    //        Time.timeScale = 0;
    //    }
    //    else
    //    {
    //        // Resume the game - we're getting focus again
    //        Time.timeScale = 1;
    //    }
    //}
}
