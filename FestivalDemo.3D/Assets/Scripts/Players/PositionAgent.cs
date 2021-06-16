using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Players
{
    /// <summary>
    /// Position the agent, to move the agent from current location to target location.
    /// </summary>
    public class PositionAgent : MonoBehaviour
    {
        private const float Distance = 20.0f;
        private const float Period = 1.0f;

        private readonly System.Random _random;
        private float _nextActionTime = 0.0f;
        private NavMeshAgent _agent;
        private Vector3 _target;

        public PositionAgent()
        {
            _random = new System.Random();
        }

        [SerializeField]
        public Communicator Communicator { get; set; }

        public bool IsFollower { get; internal set; }
        public int GuestId { get; internal set; }


        public void Start()
        {
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _target = Vector3.zero;
        }

        public void Update()
        {
            if (_target != null && _agent != null)
            {
                if (!IsFollower)
                {
                    if (Vector3.Distance(gameObject.transform.position, _target) < Distance)
                    {
                        _target = new Vector3(_random.Next(-100, 100), 0, _random.Next(-50, 50));
                    }
                }

                if (Vector3.Distance(gameObject.transform.position, _target) > Distance)
                {
                    _agent.SetDestination(_target);
                }
            }
            

            if (Time.time <= _nextActionTime)
            {
                return;
            }

            _nextActionTime += Period;

            if (Communicator != null)
            {
                var position = gameObject.transform.position;

                var bytes = new List<byte>();
                bytes.AddRange(BitConverter.GetBytes(4));
                bytes.AddRange(BitConverter.GetBytes(GuestId));
                bytes.AddRange(BitConverter.GetBytes(position.x));
                bytes.AddRange(BitConverter.GetBytes(position.z));

                Communicator.SendRequest(bytes.ToArray());
            }
        }

        public void SetTarget(Vector3 target)
        {
            if (IsFollower)
            {
                _target = target;
            }
        }
    }
}
