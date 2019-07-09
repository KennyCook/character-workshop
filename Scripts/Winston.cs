using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    public class Winston : MonoBehaviour
    {
        private Camera m_Camera;
        private CharacterController m_CharacterController;
        private UnityStandardAssets.Characters.FirstPerson.FirstPersonController m_ControllerScript;

        [SerializeField] private TeslaCannon m_TeslaCannon;

        [SerializeField] private float m_JumpPackSpeed;
        [SerializeField] private float m_JumpPackCooldown;
        [SerializeField] private JumpPackImpact m_JumpPackImpact;
        private bool m_JumpPack;
        private Vector3 m_JumpPackVector;

        [SerializeField] private float m_ShieldCooldown;
        public GameObject m_Shield;

        private const float JUMP_PACK_COOLDOWN = 6;
        private const float SHIELD_COOLDOWN = 13;


        private void Start()
        {
            m_Camera = Camera.main;
            m_CharacterController = GetComponent<CharacterController>();
            m_ControllerScript = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

            m_JumpPackCooldown = m_ShieldCooldown = 0;
        }

        private void Update()
        {
            GetInput();

            UpdateCooldowns();
        }

        private void FixedUpdate()
        {
            if (m_JumpPack)
            {
                if (m_CharacterController.isGrounded)
                {
                    m_JumpPack = false;
                    m_ControllerScript.m_CharacterMovementModifier = Vector3.zero;

                    m_JumpPackImpact.EnableJumpImpact();
                }
            }
        }

        private void GetInput()
        {
            if (Input.GetAxis("Fire1") > 0)
            {
                m_TeslaCannon.Fire(true);
            }
            else
            {
                m_TeslaCannon.Fire(false);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && m_JumpPackCooldown <= 0)
            {
                m_JumpPackVector = new Vector3(m_Camera.transform.forward.x, Mathf.Clamp(m_Camera.transform.forward.y, 0.3f, 0.8f), m_Camera.transform.forward.z);
                m_JumpPack = true;

                print("JV: " + m_JumpPackVector);
                //m_ControllerScript.m_CharacterMovementModifier = (m_JumpPackVector + -(Physics.gravity * m_ControllerScript.GetGravityMultiplier())) * m_JumpPackSpeed;
                m_ControllerScript.m_CharacterMovementModifier = m_JumpPackVector * m_JumpPackSpeed;

                m_JumpPackCooldown = JUMP_PACK_COOLDOWN;
            }

            if (Input.GetKeyDown(KeyCode.E) && m_ShieldCooldown <= 0)
            {
                m_Shield.transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z + 1);

                GameController.ToggleShieldHUD();
                m_Shield.SetActive(true);

                m_ShieldCooldown = SHIELD_COOLDOWN;
            }
        }

        private void UpdateCooldowns()
        {
            m_JumpPackCooldown = m_JumpPackCooldown > 0 ? m_JumpPackCooldown -= Time.deltaTime : 0;
            m_ShieldCooldown = m_ShieldCooldown > 0 ? m_ShieldCooldown -= Time.deltaTime : 0;

            GameController.UpdateWinstonCooldowns(m_JumpPackCooldown, m_ShieldCooldown);
        }

        //// Shift
        //private void JumpPack(Vector3 startPos)
        //{
        //    if (m_CharacterController.isGrounded && m_JumpPackCooldown == 0)
        //    {
        //        Vector3 v = new Vector3(m_Camera.transform.forward.x, Mathf.Clamp(m_Camera.transform.forward.y, 0.1f, 0.8f), m_Camera.transform.forward.z);
        //        //m_CharacterController.Move(v * m_JumpPackSpeed);
        //        //transform.Translate(v * m_JumpPackSpeed);
        //        transform.Translate(Vector3.Lerp(startPos, v, Time.deltaTime));
        //    }
        //}

        //private void Physics_JumpPack()
        //{
        //    if (m_FPController.Grounded && m_JumpPackCooldown == 0)
        //    {
        //        m_Rigidbody.drag = 0;
        //        //float yF = Mathf.Clamp(m_Camera.transform.forward.y, 0.1f, 0.8f);
        //        Vector3 v = new Vector3(m_Camera.transform.forward.x, Mathf.Clamp(m_Camera.transform.forward.y, 0.1f, 0.8f), m_Camera.transform.forward.z);
        //        //m_Rigidbody.AddForce(v, ForceMode.Impulse);
        //        m_Rigidbody.velocity = v * m_JumpPackSpeed;
        //        //m_Rigidbody.AddForce(transform.forward * m_JumpPackSpeed, ForceMode.Impulse);

        //        print(v * m_JumpPackSpeed);
        //        //m_JumpPackCooldown = 5f;
        //    }
        //}

        // Q 
        private void PrimalRage() { }
    }
}

// reg jump is slightly less than one second of hangtime
// horizontal walking movement speed is equivalent in as jumping