using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TMP_Text textMesh;
    private string originalText;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private bool useBracket = true;

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        textMesh.color = normalColor;
        originalText = textMesh.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.DOColor(hoverColor, 0.2f);
        transform.DOScale(1.1f, 0.2f);
        if (useBracket) textMesh.text = "> " + originalText + " <";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.DOColor(normalColor, 0.2f);
        transform.DOScale(1f, 0.2f);
        if (useBracket) textMesh.text = originalText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 1).SetLink(gameObject);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
