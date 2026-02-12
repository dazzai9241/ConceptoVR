using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    [Header("References")]
    public Transform visualTarget;
    public Vector3 localAxis = Vector3.up;

    [Header("Settings")]
    public float resetSpeed = 5;
    public float followAngleThreshold = 45;
    public bool isToggleable = false;

    [Header("Positional Markers")]
    [SerializeField]
    private Transform unpressedHeight;
    [SerializeField]
    private Transform pressedHeight;

    [Header("Events")]
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private Vector3 offset;
    private Vector3 initialLocalPos;
    private Transform pokeAttachTransform;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private bool isFollowing = false;

    private bool isValid = true;
    
    private bool isPressed = false;

    private bool wasIsPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure unpressedHeight and pressed Height is assigned;
        Debug.Assert(unpressedHeight != null);
        Debug.Assert(pressedHeight != null);

        initialLocalPos = visualTarget.localPosition;

        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnPoke);
        interactable.hoverExited.AddListener(Reset);
    }

    public void OnPoke(BaseInteractionEventArgs hover)
    {

        if(hover.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor interactor = (UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)hover.interactorObject;

            isFollowing = true;
         
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.InverseTransformPoint(pokeAttachTransform.position) - visualTarget.localPosition;

            // Check if finger is above the pressed marker
            if (Vector3.Dot(Vector3.up, visualTarget.InverseTransformDirection(pokeAttachTransform.position - pressedHeight.position)) > 0.0)
            {
                isValid = true;
                isFollowing = true;
            }
            else
            {
                isValid = false;
                isFollowing = false;
            }
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor)
        {
            isFollowing = false;
            isValid = true;
        }
    }


    public void CheckForPressed(Vector3 localPokePos, Vector3 localPressPos)
    {
        isPressed = localPokePos.y <= localPressPos.y;

        
        if (isPressed)
        {
            if (!wasIsPressed)
            {
                if (OnPressed != null)
                    OnPressed.Invoke();
            }
        }
        else
        {
            if (wasIsPressed)
            {
                if (OnReleased != null)
                    OnReleased.Invoke();
            }
        }
        wasIsPressed = isPressed;
    }

    // Update is called once per frame
    void Update()
    {

        if (isFollowing)
        {
            if (!isValid)
                return;

            Vector3 localPokePos = visualTarget.InverseTransformPoint(pokeAttachTransform.position);
            Vector3 localPressedPos = visualTarget.InverseTransformPoint(pressedHeight.position);

            CheckForPressed(localPokePos, localPressedPos);
            Debug.Log(isPressed);

            // Only allow the visual transform to move between inital pos, and pressed pos
            Vector3 pos = localPokePos;
            if (pos.y < localPressedPos.y)
                pos.y = localPressedPos.y;
            
            
            //Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pos - offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(pos - offset, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }
}