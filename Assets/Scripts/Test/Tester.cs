using System;
using UnityEngine;
using UnityEngine.UI;


namespace Test
{
    public class Tester : MonoBehaviour
    {
        public Button showAdButton;

        public Button startGameButton;

        public Button endGameButton;

        [SerializeField]
        GameObject m_topAdsPrefab=null;

        [SerializeField]
        RGPDManager m_rgpdManager=null;

        private void Awake()
        {
            GameObject.Instantiate(m_topAdsPrefab);

            showAdButton.onClick.AddListener(ShowAdClick);
            startGameButton.onClick.AddListener(StartGameClick);
            endGameButton.onClick.AddListener(EndGameClick);
            
            VoodooSauce.SetAdDisplayConditions(60, 3); 
        }

        private void ShowAdClick()
        {
            VoodooSauce.ShowAd(); 
        }

        /// <summary>
        /// here the ui event callback atempts to retrieve the rgpd consent value before starting the game
        /// </summary>
        private void StartGameClick()
        {
            m_rgpdManager.RetrieveRGPDValue(VoodooSauce.StartGame);
        }

        private void EndGameClick()
        {
            VoodooSauce.EndGame(); 
        }
    }
}