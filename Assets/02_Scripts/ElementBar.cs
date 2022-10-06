using UnityEngine;
using UnityEngine.UI;

public class ElementBar : MonoBehaviour
{
    [SerializeField]
    private Image m_image;
    private int m_item;
    public int Item
    {
        get => m_item;
        set
        {
            m_item = value;
            m_image.rectTransform.sizeDelta = new Vector2(7, value);
        }
    }

    public ElementBar Init(int item)
    {
        Item = item;
        SetDefaultColor();

        return this;
    }

    public void Deactivate() => gameObject.SetActive(false);
    public void SetLowColor() => m_image.color = Color.red;
    public void SetHighColor() => m_image.color = Color.blue;
    public void SetPivotColor() => m_image.color = Color.cyan;
    public void SetDefaultColor() => m_image.color = Color.white;
    public void SetCompleteColor() => m_image.color = Color.green;
}
