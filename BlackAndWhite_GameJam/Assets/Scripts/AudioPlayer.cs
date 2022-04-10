using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Customer Orders")]
    [SerializeField] AudioClip plateShatter;
    [SerializeField] float plateShatterVolume;
    [SerializeField] AudioClip customerOrder;
    [SerializeField] float customerOrderVolume;
    [SerializeField] AudioClip correctCustomerOrder;
    [SerializeField] float correctCustomerOrderVolume;
    [SerializeField] AudioClip incorrectCustomerOrder;
    [SerializeField] float incorrectCustomerOrderVolume;
    [SerializeField] AudioClip correctPay;
    [SerializeField] float correctPayVolume;
    [SerializeField] AudioClip levelCompleted;
    [SerializeField] float levelCompletedVolume;
    [SerializeField] AudioClip levelFailed;
    [SerializeField] float levelFailedVolume;
    [SerializeField] AudioClip orderPlaced;
    [SerializeField] float orderPlacedVolume;

    [Header("Conveyors")]
    [SerializeField] AudioClip conveyorRemove;
    [SerializeField] float conveyorRemoveVolume;
    [SerializeField] AudioClip conveyorSet;
    [SerializeField] float conveyorSetVolume;

    // disctionary
    public static Dictionary<EnumSoundEffects, (AudioClip, float)> soundEffects;

    // static persists through all instances of a class
    static AudioPlayer instance;
    public AudioSource audioSource;

    private void Awake()
    {
        ManageSingleton();
        audioSource = GetComponent<AudioSource>();
        soundEffects = new Dictionary<EnumSoundEffects, (AudioClip, float)>
            {
                { EnumSoundEffects.PlateShatter, (plateShatter, plateShatterVolume) },
                { EnumSoundEffects.CustomerOrder, (customerOrder, customerOrderVolume) },
                { EnumSoundEffects.OrderCorrect, (correctCustomerOrder, correctCustomerOrderVolume) },
                { EnumSoundEffects.OrderIncorrect, (incorrectCustomerOrder, incorrectCustomerOrderVolume) },
                { EnumSoundEffects.CustomerPays, (correctPay, correctPayVolume) },
                { EnumSoundEffects.TileRemove, (conveyorRemove, conveyorRemoveVolume) },
                { EnumSoundEffects.TileSet, (conveyorSet, conveyorSetVolume) },
                { EnumSoundEffects.LevelWinSound, (levelCompleted, levelCompletedVolume) },
                { EnumSoundEffects.LevelLostSound, (levelFailed, levelFailedVolume) },
                { EnumSoundEffects.OrderPlaced, (orderPlaced, orderPlacedVolume) }
            };
    }
    void ManageSingleton()
    {
        if (instance != null)
        {
            // need to disable this so other objects don't try to access
            gameObject.SetActive(false);

            // now destroy
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void PlaySoundEffect(EnumSoundEffects soundEffectName)
    {
        Vector3 soundPos = Camera.main.transform.position; // (soundEffectName.ToString().Contains("Player") ? player.transform.position : Camera.main.transform.position);
        (AudioClip, float) soundEffect = soundEffects[soundEffectName];
        AudioClip audioClip = soundEffect.Item1;
        float volume = soundEffect.Item2;

        AudioSource.PlayClipAtPoint(audioClip, soundPos, volume);
    }
}