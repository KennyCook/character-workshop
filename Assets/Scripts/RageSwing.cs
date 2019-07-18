using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageSwing : MonoBehaviour
{
    [SerializeField] private float _swingForce = 100f;
    [SerializeField] private float _swingDamage = 40f;

    private bool _swingInput = false;
    private float _swingCooldown, _swingDuration;
    private const float SWING_COOLDOWN = 0.8f;
    private const float SWING_DURATION = 0.2f;

    private BoxCollider _swingCollider;

    private void Start()
    {
        _swingCooldown = _swingDuration = 0;
        _swingCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (_swingInput && _swingCooldown <= 0)
        {
            _swingCollider.enabled = true;
            _swingDuration = SWING_DURATION;
            _swingCooldown = SWING_COOLDOWN;
        }

        if (_swingDuration <= 0)
        {
            _swingCollider.enabled = false;
        }

        _swingDuration -= Time.deltaTime;
        _swingCooldown -= Time.deltaTime;
    }

    public void Swing(bool value)
    {
        _swingInput = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_swingInput && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_swingDamage);
            try
            {
                var forceVector = other.transform.position - GetComponentInParent<Transform>().position;
                other.GetComponent<Rigidbody>().AddExplosionForce(_swingForce, GetComponentInParent<Transform>().position, 5f, 1.5f, ForceMode.Impulse);
            }
            catch (System.Exception e) { }
        }
    }

    private void OnGUI()
    {
        if (_swingDuration > 0)
        {
            //GUI.TextField(new Rect(0, 0, Screen.width / 2, Screen.height / 2), "SWING!");
            GUI.TextField(new Rect(0, 0, 100, 50), "SWING!");
        }
    }
}

// isSwinging
//      set by Winston
// time between swings
//      always decrement to 0
//      reset after collider activated
// duration of swing
//      at end of duration, turn collider off
//      reset after collider activated
//      must be less than cooldown
//      deactivate collider at 0