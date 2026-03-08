using UnityEngine;
using UnityEngine.Events;

namespace Canvas
{
    public abstract class Step : MonoBehaviour
    {
        // Event fired when a Step is completed
        public UnityEvent OnCompleted = new UnityEvent();

        public abstract void Activate();

        public abstract void Deactivate();

        /// <summary>
        /// Call this when your Step finishes what it needs to do.
        /// </summary>
        protected void Complete()
        {
            OnCompleted?.Invoke();
        }

        private void Start()
        {
            Deactivate();
        }
    }
}