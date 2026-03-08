using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


namespace Canvas
{
    public class SlidesManager : MonoBehaviour
    {
        [Header("Slides to Manage")]
        [SerializeField] private Slides[] slides;
        [SerializeField, HideInInspector]
        private int currentSlideIndex = -1;

        [Header("Input Actions")]
        [SerializeField] private bool autoStart = true;
        [SerializeField] private InputActionReference nextStepAction;
        [SerializeField] private InputActionReference prevStepAction;
        [SerializeField] private InputActionReference nextSlideAction;
        [SerializeField] private InputActionReference prevSlideAction;
        [SerializeField] private InputActionReference toBeginningAction;
        [SerializeField] private InputActionReference toEndAction;

       

        public void Setup()
        {
            Debug.Assert(slides != null && slides.Length > 0,
                $"[SlidesManager] No Slides assigned on {name}");

            foreach (var slide in slides)
            {
                slide.manager = this;
            }


            currentSlideIndex = 0;
            ShowCurrentSlide();
        }

        public void NextSlide()
        {
            if (!IsValidState()) return;
            if (currentSlideIndex == slides.Length - 1)
                return;

            int nextIndex = currentSlideIndex + 1;
            GoToSlide(nextIndex);
        }

        public void PrevSlide()
        {
            if (!IsValidState()) return;
            if (currentSlideIndex == 0)
                return;


            int prevIndex = currentSlideIndex - 1;
            GoToSlide(prevIndex);
        }

        // Advances the current step of the slide and moves to the next slide if we reached the end limit of the steps
        public void NextStep()
        {
            if (!IsValidState()) return;
            bool endReached = slides[currentSlideIndex].Next();
            if (endReached)
                NextSlide();
        }


        // Moves the current step of the slide back, and moves to the prev slide if we reached the beginning limit of the steps
        public void PrevStep()
        {
            if (!IsValidState()) return;
            bool startReached = slides[currentSlideIndex].Previous();
            if (startReached)
                PrevSlide();
        }

        public Slides CurrentSlide => IsValidState() ? slides[currentSlideIndex] : null;
        public int CurrentSlideIndex => currentSlideIndex;
        public int SlideCount => slides?.Length ?? 0;


        // Private
        private void Start()
        {
            //Debug.Log(SystemInfo.graphicsDeviceName);

            if (autoStart)
                Setup();

            SetupInputActions();

        }

        private void Update()
        {
            
        }



        private void GoToSlide(int index)
        {
            if (currentSlideIndex == index) return;

            // Cleanup current
            if (IsValidState())
                slides[currentSlideIndex].Cleanup();

            currentSlideIndex = index;

            // Setup new
            slides[currentSlideIndex].Setup();
        }

        private void ShowCurrentSlide()
        {
            if (IsValidState())
                slides[currentSlideIndex].Setup();
        }

        private bool IsValidState()
        {
            return slides != null &&
                   slides.Length > 0 &&
                   currentSlideIndex >= 0 &&
                   currentSlideIndex < slides.Length &&
                   slides[currentSlideIndex] != null;
        }

        public void GoToBeginning()
        {
            if (!IsValidState()) return;
            GoToSlide(0);
        }

        public void GoToEnd()
        {
            if (!IsValidState()) return;
            GoToSlide(slides.Length - 1);
        }


        private void SetupInputActions()
        {
            if (nextStepAction != null) nextStepAction.action.performed += ctx => NextStep();
            if (prevStepAction != null) prevStepAction.action.performed += ctx => PrevStep();
            if (nextSlideAction != null) nextSlideAction.action.performed += ctx => NextSlide();
            if (prevSlideAction != null) prevSlideAction.action.performed += ctx => PrevSlide();
            if (toBeginningAction != null) toBeginningAction.action.performed += ctx => GoToBeginning();
            if (toEndAction != null) toEndAction.action.performed += ctx => GoToEnd();
        }
    }
}