using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace Canvas
{
    public class Slides : MonoBehaviour
    {

        [Header("Configuration")]
        public Step[] steps;


        public void Setup()
        {
            Debug.Assert(steps != null, $"[Slides] Steps array is null on {name}");
            Debug.Assert(steps.Length > 0, $"[Slides] Steps array is empty on {name}");


            if (currentStep != -1)
                Debug.LogWarning($"[Slides] Setup() called multiple times on {name}. Previous state will be overridden.");

            // Ensure current step is valid before doing anything
            currentStep = 0;

            // Activate first step
            steps[currentStep].Activate();
        }



        public void Cleanup()
        {
            if (currentStep < 0 || steps == null || currentStep >= steps.Length)
                return; // Already cleaned or never set up

            steps[currentStep].Deactivate();
            currentStep = -1; // Return to initial state
        }


        public bool Next()
        {
            AssertIsValidState();
            if (currentStep == steps.Length - 1)
                return true;

            steps[currentStep].Deactivate();

            currentStep += 1;

            AssertStepIsNotNull(currentStep);
            steps[currentStep].Activate();
            return false;

        }


        public bool Previous()
        {
            AssertIsValidState();
            if (currentStep == 0)
                return true;

            steps[currentStep].Deactivate();

            currentStep = (currentStep - 1 + steps.Length) % steps.Length;

            AssertStepIsNotNull(currentStep);
            steps[currentStep].Activate();
            return false;
        }


        [SerializeField, HideInInspector]
        private int currentStep = -1; // -1 = not initialized

        // Helper methods to reduce duplication and improve readability
        private void AssertIsValidState()
        {
            Debug.Assert(steps != null, $"[Slides] Steps array is null on {name}");
            Debug.Assert(steps.Length > 0, $"[Slides] Steps array is empty on {name}");
            Debug.Assert(currentStep >= 0 && currentStep < steps.Length,
                $"[Slides] currentStep is invalid: {currentStep}. Valid range: 0–{steps.Length - 1}");
        }

        private void AssertStepIsNotNull(int index)
        {
            Debug.Assert(steps[index] != null,
                $"[Slides] Step at index {index} is null on {name}!");
        }
    }
}