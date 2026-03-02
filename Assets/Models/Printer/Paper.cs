using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Paper : MonoBehaviour
{
    public enum PAPER_TYPE
    {
        Data = 0,
        Hashkey,
    };

    public PAPER_TYPE PaperType
    {
        get { return paperType; }
        set
        {
            if (paperType != value)
            {
                paperType = value;
                UpdateMaterial();
            }
        }
    }

    [SerializeField] private Material hashkeyMat;
    [SerializeField] private Material dataMat;

    public string data = "$$$";

    private MeshRenderer meshRenderer;
    private PAPER_TYPE paperType = PAPER_TYPE.Data;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (meshRenderer == null) return;

        switch (paperType)
        {
            case PAPER_TYPE.Data:
                meshRenderer.material = dataMat;
                break;

            case PAPER_TYPE.Hashkey:
                meshRenderer.material = hashkeyMat;
                break;
        }
    }
}