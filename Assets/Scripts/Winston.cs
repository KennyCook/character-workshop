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

        [SerializeField] private float _shieldCooldown;
        public GameObject m_Shield;

        [SerializeField] private RageSwing RageSwingGO;
        [SerializeField] private float _rageCooldown;
        private bool _rageActive = false;
        private float _rageTimer;
        // swing vars?

        private const float JUMP_PACK_COOLDOWN = 6;
        private const float RAGE_JUMP_PACK_COOLDOWN = 3;
        private const float RAGE_DURATION = 6;
        //private const float RAGE_COOLDOWN = 20;
        private const float RAGE_COOLDOWN = 1;
        private const float SHIELD_COOLDOWN = 13;


        private void Start()
        {
            m_Camera = Camera.main;
            m_CharacterController = GetComponent<CharacterController>();
            m_ControllerScript = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

            m_JumpPackCooldown = _shieldCooldown = _rageCooldown = 0;
        }

        private void Update()
        {
            GetInput();

            UpdateCooldowns();

            if (_rageActive)
            {
                _rageTimer -= Time.deltaTime;

                if (_rageTimer <= 0)
                {
                    _rageActive = false;
                    RageSwingGO.gameObject.SetActive(false);
                    m_TeslaCannon.gameObject.SetActive(true);
                    _rageCooldown = RAGE_COOLDOWN;
                }
            }
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
            if (!_rageActive)
            {
                if (Input.GetAxis("Fire1") > 0)
                {
                    m_TeslaCannon.Fire(true);
                }
                else
                {
                    m_TeslaCannon.Fire(false);
                }

                if (Input.GetKeyDown(KeyCode.E) && _shieldCooldown <= 0)
                {
                    m_Shield.transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z + 1);

                    GameController.ToggleShieldHUD();
                    m_Shield.SetActive(true);

                    _shieldCooldown = SHIELD_COOLDOWN;
                }

                if (Input.GetKeyDown(KeyCode.Q) && _rageCooldown <= 0)
                {
                    PrimalRage();
                }
            }

            if (_rageActive)
            {
                if (Input.GetAxis("Fire1") > 0)
                {
                    RageSwingGO.Swing(true);
                }
                else
                {
                    RageSwingGO.Swing(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && m_JumpPackCooldown <= 0)
            {
                m_JumpPackVector = new Vector3(m_Camera.transform.forward.x, Mathf.Clamp(m_Camera.transform.forward.y, 0.3f, 0.8f), m_Camera.transform.forward.z);
                m_JumpPack = true;

                print("JV: " + m_JumpPackVector);
                //m_ControllerScript.m_CharacterMovementModifier = (m_JumpPackVector + -(Physics.gravity * m_ControllerScript.GetGravityMultiplier())) * m_JumpPackSpeed;
                m_ControllerScript.m_CharacterMovementModifier = m_JumpPackVector * m_JumpPackSpeed;

                m_JumpPackCooldown = _rageActive ? RAGE_JUMP_PACK_COOLDOWN : JUMP_PACK_COOLDOWN;
            }
        }

        private void UpdateCooldowns()
        {
            m_JumpPackCooldown = m_JumpPackCooldown > 0 ? m_JumpPackCooldown -= Time.deltaTime : 0;
            _shieldCooldown = _shieldCooldown > 0 ? _shieldCooldown -= Time.deltaTime : 0;
            _rageCooldown = _rageCooldown > 0 ? _rageCooldown -= Time.deltaTime : 0;

            GameController.UpdateWinstonCooldowns(m_JumpPackCooldown, _shieldCooldown, _rageCooldown);
        }

        // Q 
        private void PrimalRage()
        {
            // toggle rage mode, activate rage timer
            // disable tesla cannon
            // ** rage screen effects
            _rageActive = true;
            RageSwingGO.gameObject.SetActive(true);
            _rageTimer = RAGE_DURATION;
            m_TeslaCannon.gameObject.SetActive(false);

            // UI rage timer 

            // >> in getinput: left click = swing
            // swing: do damage to enemy and generate impulse force
            //              show swing effect, ** arm swing animation

            // ignore shield input

            // AFTER TIMER ends:
            //      toggle rage mode, reenable tesla cannon
            //      apply cooldown to primal rage
        }
    }
}

// reg jump is slightly less than one second of hangtime
// horizontal walking movement speed is equivalent in as jumping