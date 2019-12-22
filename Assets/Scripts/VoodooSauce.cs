/* Introduction
 *
 * VoodooSauce is a Unity SDK that we implement into all of our games here at Voodoo.  This SDK is responsible for
 * providing Ads, Analytics, IAP, GDPR, etc. functionality in an easy to use package for internal and external studios
 * to integrate into their games. The SDK is used around the world by more than 200+ games, thus reliability and ease of use is
 * incredibly important for us. 
 *
 * For this exercise, we would like you to create a basic VoodooSauce that integrates the fake "TopAds" and "TopAnalytics"
 * SDKs.
 *
 * At the end we ask that you answer some quick questions at the bottom of this file. 
 * 
 */

/* Instructions 
 *
 * Please fill out the method implementations below 
 * Feel free to create additional classes to help with your implementation 
 * Please do not spend more than 2.5 hours on the code implementation portion of this exercise
 * Please do not modify the code in the 3rdParty folder
 * Make sure to read this entire file before starting to code.  We include important instructions on how to use the TopAds and TopAnalytics SDKs
 * 
 */

// Bonus Question : Show an android Toast when you launch the app.
using _3rdParty;
using UnityEngine;

public static class VoodooSauce{

    const string GAME_ENDED_EVENT="GAME_ENDED";
    const string GAME_STARTED_EVENT = "GAME_STARTED";
    const string AD_DISPLAYED_EVENT = "AD_DISPLAYED_EVENT";

    static float MinTimeBetweenAds=10f;
    static int MinGamesBetweenAds=1;

    static float TimeLastAdShown=float.MinValue;
    static int GamesPlayedSinceLastAd=0;

    // Before calling methods in TopAds and TopAnalytics you must call their init methods 
    // TopAds requires the TopAds prefab to be created in the scene
    // You also need to collect user GDPR consent and pass that boolean value to TopAds and TopAnalytics 
    // You can collect this consent by displaying a popup to the user at the start of the game and then storing that value for future use 
	public static void StartGame()
	{
        UnityEngine.Assertions.Assert.IsTrue(RGPDManager.RgpdConsentalue.HasValue);
        TopAds.InitializeSDK();
        TopAnalytics.InitWithConsent(RGPDManager.RgpdConsentalue.Value);
        if(RGPDManager.RgpdConsentalue.Value)
        {
            TopAds.GrantConsent();
        }else
        {
            TopAds.RevokeConsent();
        }
        // Track in TopAnalytics that a game has started 
        TopAnalytics.TrackEvent(GAME_STARTED_EVENT);
        TopAds.OnAdShownEvent += TopAds_OnAdShownEvent;
	}

    private static void TopAds_OnAdShownEvent()
    {
        TopAnalytics.TrackEvent(AD_DISPLAYED_EVENT);
        TimeLastAdShown = Time.time;
        GamesPlayedSinceLastAd = 0;
    }

    public static void EndGame()
	{
        // Track in TopAnalytics that a game has ended 
        TopAnalytics.TrackEvent(GAME_ENDED_EVENT);
        GamesPlayedSinceLastAd++;
	}
	
	public static void ShowAd()
	{

        // TopAds methods must be called with a unique "string" ad unit id 
        // For your test app that id is "f4280fh0318rf0h2" 
        // However, when releasing the SDK to other studios, their ad unit id will be different 
        // Please find a flexible way to allow studios to provide their ad unit id to your VoodooSauce SDK 


        // Before an ad is available to display, you must call TopAds.RequestAd 
        // You must call RequestAd each time before an ad is ready to display

        // RequestAd will make a "fake" request for an ad that will take 0 to 10 seconds to complete
        // Afterwards, either the OnAdLoadedEvent or OnAdFailedEvent will be invoked 
        // Please implement an autorequest system that ensures an ad is always ready to be displayed
        // Keep in mind that RequestAd can fail multiple times in a row 

        // If an ad is loaded correctly, clicking on the "Show Ad" button within Unity-VoodooSauceTestApp 
        // should display a fake ad popup that you can close. 


        // Track in TopAnalytics when an ad is displayed.  Hint: TopAds.OnAdShownEvent 
        if (GamesPlayedSinceLastAd < MinGamesBetweenAds || Time.time - TimeLastAdShown < MinTimeBetweenAds)
        {
            return;
        }
        AdBuffer.Instance.DisplayAd();
	}

	public static void SetAdDisplayConditions(int secondsBetweenAds, int gamesBetweenAds)
	{
        // Sometimes studios call "ShowAd" too often and bombard players with ads 
        // Add a system that prevents the "ShowAd" method from showing an available ad 
        // Unless EITHER condition provided is true: 
        // 1) secondsBetweenAds: only show an ad if the previous ad was shown more than "secondBetweenAds" ago 
        // 2) gamesBetweenAds: only show an ad if "gamesBetweenAds" amount of games was played since the previous ad 
        MinGamesBetweenAds = gamesBetweenAds;
        MinTimeBetweenAds = secondsBetweenAds;
	}
	
	
	// === Please answer these quick questions within the code file ===  
	
	// In the VoodooSauce we integrate many 3rd party SDKs of varying reliability that display Ads, Analytics, etc.
	// What processes would you suggest to ensure that the VoodooSauce SDK is minimally affected by crashes 
	// in another SDK?
	
	// What are some pitfalls/shortcomings in your above implementation?
    // only registered ad displayed once the game has started
    // No option to revoke the RGPD consent

	
	// How would you improve your implementation if you had more than 2 hours? 
	// Check the time before the ad is closed and feed it to the Analytics module.
    // Buffer multiple ads, and implement parameters to configure how many.

	// What do you enjoy the most about being a developer? 
    //Discovering a new technology through solving a real problem
	
	// What do you enjoy the least about being a developer? 
    //Reading through undocumented, uncommented, badly written legacy code to maintain it.
	
	// Why do you want to work on the VoodooSauce SDK vs. creating games in Unity?
    // It seems to have some interesting possibilities regarding data collection and advertising, two things that would be interesting to practise
	
	
	
}
