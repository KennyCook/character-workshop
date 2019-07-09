using UnityEngine;

namespace CharacterWorkshop
{
    public static class GameController
    {
        private static UIController m_UIController;

        static GameController()
        {
            m_UIController = GameObject.Find("Main Canvas").GetComponent<UIController>();
        }

        public static void ToggleShieldHUD()
        {
            m_UIController.ToggleShieldHUD();
        }

        public static void UpdateShieldBar(float shieldPercentageRemaining)
        {
            m_UIController.UpdateShieldBar(shieldPercentageRemaining);
        }

        public static void UpdateWinstonCooldowns(float jpCool, float shCool)
        {
            m_UIController.UpdateWinstonCooldowns(jpCool, shCool);
        }
    }
}