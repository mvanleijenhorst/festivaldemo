using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Players
{
    /// <summary>
    /// Select target for the agent, handle left mouse button and set target for the agents.
    /// </summary>
    public class TargetSelector : MonoBehaviour
    {
        [Header("Cursors")]
        /// <summary>
        /// Normal pointer.
        /// </summary>
        public Texture2D PointCursor; 

        [Header("Mouse")]
        /// <summary>
        /// Layer with all clickable objects.
        /// </summary>
        public LayerMask ClickableLayer;

        /// <summary>
        /// Update call of the game object.
        /// </summary>
        public void Update()
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 200, ClickableLayer))
            {

                if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                {
                    SetTargetCrowd(hitInfo.point);
                }

                UnityEngine.Cursor.SetCursor(PointCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        /// <summary>
        /// Set the target for the agents.
        /// </summary>
        /// <param name="target">Target location for the agents</param>
        private void SetTargetCrowd(Vector3 target)
        {
            var agents = gameObject.GetComponentsInChildren<PositionAgent>();
            foreach (var agent in agents)
            {
                agent.SetTarget(target);
            }
        }
    }
}
