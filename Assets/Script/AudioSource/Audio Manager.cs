/*
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // ทำให้สามารถเรียกใช้ได้จากทุกที่
    [Header("Audio Clips")]
    public AudioClip homePageClip; // เสียงหน้า Home page
    public AudioClip walkWithObjectClip; // เสียงเดินเวลามี obj ติดกับ Player
    public AudioClip gameOverClip; // เสียง GameOver
    public AudioClip buttonClickClip; // เสียง Clicking button
    public AudioClip checkpointClip; // เสียง CheckPoint
    public AudioClip backgroundMusicClip; // เสียง พื้นหลังของเกม หน้า Panel Game system
    public AudioClip cutsceneClip; // เสียง Cutscene
    public AudioClip winClip; // เสียง ชนะ
    public AudioClip howToPlayClip; // เสียง เปลี่ยนหน้ากระดาษเวลาเปลี่ยน หน้า HowToPlay

    [Header("Audio Source")]
    public AudioSource mainAudioSource; // ตัวควบคุมเสียงหลัก (Background Music, Game Sounds)
    
    // ฟังก์ชันในการเล่นเสียง
    public void PlayHomePageSound()
    {
        PlaySound(homePageClip);
    }

    public void PlayWalkWithObjectSound()
    {
        PlaySound(walkWithObjectClip);
    }

    public void PlayGameOverSound()
    {
        PlaySound(gameOverClip);
    }

    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickClip);
    }

    public void PlayCheckPointSound()
    {
        PlaySound(checkpointClip);
    }

    public void PlayBackgroundMusic()
    {
        PlaySound(backgroundMusicClip);
    }

    public void PlayCutsceneSound()
    {
        PlaySound(cutsceneClip);
    }

    public void PlayWinSound()
    {
        PlaySound(winClip);
    }

    public void PlayHowToPlaySound()
    {
        PlaySound(howToPlayClip);
    }

    // ฟังก์ชันทั่วไปในการเล่นเสียง
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            mainAudioSource.clip = clip;
            mainAudioSource.Play();
        }
    }

    // ฟังก์ชันสำหรับการปรับระดับเสียงตาม VolumeBar
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // ปรับระดับเสียงหลัก
        mainAudioSource.volume = volume; // ปรับเสียงใน AudioSource
    }
}
*/


using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton Instance

    [Header("Audio Clips")]
    public AudioClip homePageClip; // เสียงหน้า Home page
    public AudioClip walkWithObjectClip; // เสียงเดินเวลามี obj ติดกับ Player
    public AudioClip gameOverClip; // เสียง GameOver
    public AudioClip buttonClickClip; // เสียง Clicking button
    public AudioClip checkpointClip; // เสียง CheckPoint
    public AudioClip backgroundMusicClip; // เสียง พื้นหลังของเกม หน้า Panel Game system
    public AudioClip cutsceneClip; // เสียง Cutscene
    public AudioClip winClip; // เสียง ชนะ
    public AudioClip howToPlayClip; // เสียง เปลี่ยนหน้ากระดาษเวลาเปลี่ยน หน้า HowToPlay

    [Header("Audio Source")]
    public AudioSource mainAudioSource; // ใช้เล่นเสียงพื้นหลัง
    public AudioSource sfxAudioSource;  // ใช้เล่นเสียงเอฟเฟกต์ (เช่น เสียงคลิกปุ่ม)

    private void Awake()
    {
        // ตรวจสอบว่า Instance มีอยู่แล้วหรือไม่
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ทำให้ Object นี้ไม่ถูกทำลายเมื่อเปลี่ยนฉาก
        }
        else
        {
            Destroy(gameObject); // ป้องกันการมี AudioManager ซ้ำซ้อน
        }
    }

    private void Start()
    {
        PlayHomePageSound(); // เล่นเสียงหน้าแรก
    }

    // เล่นเสียงพื้นหลัง
    public void PlayHomePageSound() { PlayBackgroundMusic(homePageClip); }
    public void PlayBackgroundMusic() { PlayBackgroundMusic(backgroundMusicClip); }
    public void PlayCutsceneSound() { PlayBackgroundMusic(cutsceneClip); }

    // เล่นเสียงเอฟเฟกต์
    public void PlayWalkWithObjectSound() { PlaySFX(walkWithObjectClip); }
    public void PlayGameOverSound() { PlaySFX(gameOverClip); }
    public void PlayButtonClickSound() { PlaySFX(buttonClickClip); }
    public void PlayCheckPointSound() { PlaySFX(checkpointClip); }
    public void PlayWinSound() { PlaySFX(winClip); }
    public void PlayHowToPlaySound() { PlaySFX(howToPlayClip); }

    // เล่นเสียงพื้นหลัง (แทนที่เพลงเดิม)
    private void PlayBackgroundMusic(AudioClip clip)
    {
        if (clip != null && mainAudioSource.clip != clip)
        {
            mainAudioSource.clip = clip;
            mainAudioSource.loop = true; // ทำให้เล่นซ้ำ
            mainAudioSource.Play();
        }
    }

    // เล่นเสียงเอฟเฟกต์ (ไม่รบกวนเสียงพื้นหลัง)
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip); // ใช้ PlayOneShot เพื่อให้เล่นซ้อนกันได้
        }
    }

    // ปรับระดับเสียง
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        mainAudioSource.volume = volume;
        sfxAudioSource.volume = volume;
    }
    
    
}

