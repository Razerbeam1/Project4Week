using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Vector3 originalScale;
    
    [SerializeField] private AudioClip hoverClip; // เสียงเมื่อเอาเมาส์ไปโดนปุ่ม
    
    void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.1f; // ขยายปุ่ม
        //button.image.color = Color.yellow; // เปลี่ยนสีปุ่ม
        
        // เล่นเสียงเมื่อเอาเมาส์ไปโดนปุ่ม
        if (AudioManager.Instance != null && hoverClip != null)
        {
            AudioManager.Instance.sfxAudioSource.PlayOneShot(hoverClip);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // กลับเป็นขนาดเดิม
        //button.image.color = Color.white; // กลับเป็นสีเดิม
    }
}