using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterWorkshop
{
    public class UIController : MonoBehaviour
    {
        public Text m_JumpPackCooldownText;
        public Text m_ShieldCooldownText;

        public GameObject m_ShieldHUD;
        public Image m_ShieldBar;

        public void ToggleShieldHUD()
        {
            m_ShieldHUD.SetActive(!m_ShieldHUD.activeSelf);
        }

        public void UpdateShieldBar(float shieldPercentageRemaining)
        {
            if (shieldPercentageRemaining <= 0f)
            {
                ToggleShieldHUD();
                m_ShieldBar.rectTransform.localScale.Set(1, 1, 1);
            }
            else
                m_ShieldBar.rectTransform.localScale = new Vector3(shieldPercentageRemaining, 1, 1);
        }

        public void UpdateWinstonCooldowns(float jumpPackCooldown, float shieldCooldown)
        {
            m_JumpPackCooldownText.text = jumpPackCooldown.ToString("0.00");
            m_ShieldCooldownText.text = shieldCooldown.ToString("0.00");
        }
    }
}