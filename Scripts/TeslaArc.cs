using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    // Original bezier script from: https://www.youtube.com/watch?v=tgCFzoG_BJM
    public class TeslaArc : MonoBehaviour
    {
        [SerializeField] public Transform _startPoint;
        [SerializeField] public Transform _midpoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private short _vertexCount = 5;
        [SerializeField] private GameObject _target;

        [SerializeField]
        [Range(1.0f, 10.0f)]
        private float _chaosFactor = 1.0f;

        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float _duration = 0.5f;

        private System.Random RandomGenerator = new System.Random();

        private bool _isActive;
        private float _timer;
        private List<Vector3> _pointList;

        private void Start()
        {
            _isActive = false;
            _timer = 0;
            _pointList = new List<Vector3>();
            _lineRenderer.GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (_isActive)
            {
                //if (_timer <= 0)
                //{
                    _timer = _duration + Mathf.Min(0f, _timer);
                    _pointList.Clear();

                    if (_target != null)
                    {
                        _endPoint = _target.transform;

                        for (float ratio = 0; ratio <= 1; ratio += 1.0f / _vertexCount)
                        {
                            var tangentLineVertex1 = Vector3.Lerp(_startPoint.position, _midpoint.position, ratio);
                            var tangentLineVertex2 = Vector3.Lerp(_midpoint.position, _endPoint.position, ratio);
                            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);

                        // TODO: put chaos element on a quadratic curve so the arc is more exagerated at the middle
                        // adjustedChaos = _chaosFactor * ratio x^2 + _vertexCount * ratio
                        var adjustedChaosFactor = (-_chaosFactor * Mathf.Pow(_vertexCount * ratio, 2) + _vertexCount * (_vertexCount * ratio)) / (_vertexCount * 10); // divided by dampenVal
                        print(adjustedChaosFactor + ", " + ratio + ", " + Time.time.ToString());

                            Vector3 randomVector;
                            RandomVector(ref bezierpoint, adjustedChaosFactor, out randomVector);
                            Debug.DrawLine(bezierpoint, randomVector, Color.cyan);
                            bezierpoint += randomVector;

                            _pointList.Add(bezierpoint);
                        }
                    }

                    _lineRenderer.positionCount = _pointList.Count;
                    _lineRenderer.SetPositions(_pointList.ToArray());
                //}

                _target.GetComponent<Enemy>().TakeDamage(0.2f);
                //_timer -= Time.deltaTime;
            }
        }

        public void SetTarget(Transform source, Transform midpoint, GameObject target)
        {
            _startPoint = source;
            _midpoint = midpoint;
            _target = target;
        }

        public GameObject GetTarget()
        {
            return _target;
        }

        public void RemoveTarget(GameObject target)
        {
            if (_target == target)
            {
                _startPoint = _endPoint = null;
                _target = null;
            }
        }

        public void ToggleActive(bool value)
        {
            _isActive = value;
            _lineRenderer.enabled = _isActive;

            if (!_isActive)
            {
                _startPoint = _endPoint = null;
                _target = null;
            }
        }

        // Original randomization code in "Lightning Bolt Effect for Unity" asset, LightningBoltScript.cs
        public void RandomVector(ref Vector3 start, float offsetAmount, out Vector3 result)
        {
            Vector3 directionNormalized = start.normalized;
            Vector3 side;
            GetPerpendicularVector(ref directionNormalized, out side);

            // generate random distance
            float distance = (((float)RandomGenerator.NextDouble() + 0.1f) * offsetAmount);

            // get random rotation angle to rotate around the current direction
            float rotationAngle = ((float)RandomGenerator.NextDouble() * 360.0f);

            // rotate around the direction and then offset by the perpendicular vector
            result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
        }

        private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
        {
            if (directionNormalized == Vector3.zero)
            {
                side = Vector3.right;
            }
            else
            {
                // use cross product to find any perpendicular vector around directionNormalized:
                // 0 = x * px + y * py + z * pz
                // => pz = -(x * px + y * py) / z
                // for computational stability use the component farthest from 0 to divide by
                float x = directionNormalized.x;
                float y = directionNormalized.y;
                float z = directionNormalized.z;
                float px, py, pz;
                float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
                if (ax >= ay && ay >= az)
                {
                    // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                    py = 1.0f;
                    pz = 1.0f;
                    px = -(y * py + z * pz) / x;
                }
                else if (ay >= az)
                {
                    // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                    px = 1.0f;
                    pz = 1.0f;
                    py = -(x * px + z * pz) / y;
                }
                else
                {
                    // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                    px = 1.0f;
                    py = 1.0f;
                    pz = -(x * px + y * py) / z;
                }
                side = new Vector3(px, py, pz).normalized;
            }
        }

        private void OnDrawGizmos()
        {
            //    Gizmos.color = Color.red;
            //    //for (float ratio = 0.5f / vertexCount; ratio < 1; ratio += 1.0f / vertexCount)            // curve
            //    for (float ratio = 0; ratio <= 1; ratio += 1.0f / _vertexCount)                            // tangent points
            //    {
            //        Gizmos.DrawLine(Vector3.Lerp(_startPoint.position, _midpoint.position, ratio),
            //                        Vector3.Lerp(_midpoint.position, _endPoint.position, ratio));
            //    }

            //    Gizmos.color = Color.yellow;
            //    Gizmos.DrawLine(_startPoint.position, _midpoint.position);

            //    Gizmos.color = Color.cyan;
            //    Gizmos.DrawLine(_midpoint.position, _endPoint.position);

            //try
            //{
            //    Gizmos.color = Color.green;
            //    Gizmos.DrawSphere(_midpoint, 0.1f);

            //    Gizmos.color = Color.blue;
            //    Gizmos.DrawSphere(_startPoint.position * 1.05f, 0.1f);
            //}
            //catch (System.Exception e) { }

            //Gizmos.color = Color.red;
            //Gizmos.DrawSphere(Camera.main.transform.forward, 0.1f);


        }
    }
}


/*
 * TODO: 
 * 
 * to make the arcs look less erratic, add the timer and also adjust the position[0] of the linerenderer when the player moves
 * 
 */