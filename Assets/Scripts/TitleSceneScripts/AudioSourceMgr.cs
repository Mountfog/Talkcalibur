using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioSourceMgr : MonoBehaviour , IEndDragHandler
{
    public Slider m_sliderSFX = null;
    public AudioSource m_audioSourceSFX = null;
    public void OnEndDrag(PointerEventData eventData)
    {
        float v = m_sliderSFX.value;
        m_audioSourceSFX.volume = v;
        m_audioSourceSFX.Play();
    }
}
