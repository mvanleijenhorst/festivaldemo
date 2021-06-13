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
        /// <summary>
        /// Nav mesh agent.
        /// </summary>
        [Header("Agent")]
        [SerializeField]
        private NavMeshAgent _agent;

        /// <summary>
        /// Target location for the agent.
        /// </summary>
        [SerializeField]
        public Vector3 _target;

        /// <summary>
        /// Communication service.
        /// </summary>
        [Header("Communicator")]
        [SerializeField]
        public Communicator _communicator;

        /// <summary>
        /// Initial start of the game object.
        /// </summary>
        public void Start()
        {
            _agent = gameObject.GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Update call of the game object.
        /// </summary>
        public void Update()
        {
            if (_target != null && _agent != null)
            {
                _agent.SetDestination(_target);
            }

            if (_communicator != null)
            {
                var position = gameObject.transform.position;

                var bytes = new List<byte>();
                bytes.AddRange(BitConverter.GetBytes(4));
                bytes.AddRange(BitConverter.GetBytes(0));
                bytes.AddRange(BitConverter.GetBytes(position.x));
                bytes.AddRange(BitConverter.GetBytes(position.z));

                _communicator.SendRequest(bytes.ToArray());
            }
        }

        /// <summary>
        /// Set new target.
        /// </summary>
        /// <param name="target">Target</param>
        internal void SetTarget(Vector3 target)
        {
            _target = target;
        }
    }
}
