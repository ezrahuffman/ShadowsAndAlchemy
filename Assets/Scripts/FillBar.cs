using UnityEngine;

public class FillBar : MonoBehaviour
{
    [SerializeField] RectTransform fillTrans;

    public void SetFill(float percentage)
    {
        if (percentage < 0 || percentage > 1)
        {
            Debug.LogError("Fill bar value out of range [0, 1]");
            return;
        }
        

        fillTrans.localScale = new Vector3(percentage, fillTrans.localScale.y, fillTrans.localScale.y);
    }

}
