using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import นี้สำหรับการใช้งาน Slider

public class AudioManager : MonoBehaviour
{
    
    [Header("----------------- Audio Source --------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------------- Audio Clip --------")]
    // Background Music
    public AudioClip background; // เสียงพื้นหลัง

    // Sound Effects
    public AudioClip shoot; // เสียงยิง
    public AudioClip End_Win_; // เสียงจบเกมเมื่อผู้เล่นชนะ
    public AudioClip End_Over_; // เสียงจบเกมเมื่อผู้เล่นแพ้
    public AudioClip Hp; // เสียงลดเลือดPlayer
    public AudioClip SwitchPutDows; // เสียงเหยีบปุ่ม
    public AudioClip Lever; // เสียงของคัโยก
    //public AudioClip moveplatform; //เสียงเลื่อนที่ของ move platform
    public AudioClip gameStart; // เสียงเริ่มเกม
    public AudioClip CutScene;
    
    [Header("----------------- Audio dialog --------")]
    public AudioClip dialog11;
    public AudioClip dialog22;
    public AudioClip dialog33;
    
   
    
    /*private void Start()
    { 
        
        musicSource.clip = background;
        musicSource.Play();
    }*/

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
        
    }
    
    public bool IsPlayingSFX(AudioClip clip)
    {
        return SFXSource.isPlaying && SFXSource.clip == clip;
    }

    
    public void PlayBackgroundMusic()
    {
        if (musicSource != null && background != null && !musicSource.isPlaying)
        {
            musicSource.clip = background;  // ตั้งค่า AudioClip ที่จะเล่น
            musicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    
}