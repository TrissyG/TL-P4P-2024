using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using UnityEditor;

public class SpectatorUI : MonoBehaviour
{
    private Button buttonSceneTutorial;
    private Button buttonScene1;
    private Button buttonScene2;
    private Button buttonScene3;
    private Button buttonCameraFirstPerson;
    private Button buttonCameraFixed;
    private Toggle toggleRadioVisibility;
    private Toggle toggleRadioSound;
    private Slider sliderRadioVolume;
    private TextField fieldRadioVolume;
    private string fieldRadioVolumePreviousValue;
    private Toggle toggleChimeSound;
    private Slider sliderChimeVolume;
    private TextField fieldChimeVolume;
    private string fieldChimeVolumePreviousValue;
    private Toggle toggleEnvironmentSound;
    private Slider sliderEnvironmentVolume;
    private TextField fieldEnvironmentVolume;
    private string fieldEnvironmentVolumePreviousValue;
    private Toggle toggleBandpassEnable;
    private Slider sliderBandpassFreq;
    private TextField fieldBandpassFreq;
    private string fieldBandpassFreqPreviousValue;
    private Slider sliderBandpassQ;
    private TextField fieldBandpassQ;
    private string fieldBandpassQPreviousValue;
    private Toggle toggleParamEQEnable;
    private Slider sliderParamEQFreq;
    private TextField fieldParamEQFreq;
    private string fieldParamEQFreqPreviousValue;
    private Slider sliderParamEQRange;
    private TextField fieldParamEQRange;
    private string fieldParamEQRangePreviousValue;
    private Slider sliderParamEQGain;
    private TextField fieldParamEQGain;
    private string fieldParamEQGainPreviousValue;
    private Toggle toggleShowTSNSTablet;
    private Toggle toggleShowRatingTablet;
    private Toggle toggleDoGhosting;
    private RadioButton selectorNewFile;
    private TextField fieldNewFile;
    private RadioButton selectorExistingFile;
    private DropdownField dropdownExistingFile;
    private bool newFileIsSelected = true;
    private Button buttonSave;
    private Button buttonLoad;
    private Button buttonRestoreHeadlockedObject;

    // audio source to control
    public GameObject radio;
    private AudioSource radioAudioSource;
    private Renderer radioRenderer;
    private AudioMixer radioMixer;
    private BandPassFilter bandPassFilter;

    
    public GameObject chimes;
    private AudioMixer chimeMixer;



    // Position where RatingTabletToggleable appears
    public Transform RatingTabletToggleableSpawnpoint;
    // Prefab of RatingTabletToggleable
    public GameObject RatingTabletToggleablePrefab;
    // Instance of RatingTabletToggleable
    private GameObject RatingTabletToggleable;

    public GameObject RatingTablet;

    public List<PositioningAssistGhost> ghosts;

    public AudioMixer EnvironmentAudioMixer;

    // spectator camera
    private SpectatorCamera spectator;
    private ScrollView listSoundPosition;

    public class SoundPosition{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    }

    private void OnEnable()
    {
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        buttonSceneTutorial = root.Q("buttonSceneTutorial") as Button;
        buttonScene1 = root.Q("buttonScene1") as Button;
        buttonScene2 = root.Q("buttonScene2") as Button;
        buttonScene3 = root.Q("buttonScene3") as Button;
        buttonCameraFirstPerson = root.Q("buttonCameraFirstPerson") as Button;
        buttonCameraFixed = root.Q("buttonCameraFixed") as Button;
        toggleRadioVisibility = root.Q("toggleRadioVisibility") as Toggle;
        toggleRadioSound = root.Q("toggleRadioSound") as Toggle;
        sliderRadioVolume = root.Q("sliderRadioVolume") as Slider;
        fieldRadioVolume = root.Q("fieldRadioVolume") as TextField;
        //sliderPitch = root.Q("sliderPitch") as Slider;
        toggleChimeSound = root.Q("toggleChimeSound") as Toggle;
        sliderChimeVolume = root.Q("sliderChimeVolume") as Slider;
        fieldChimeVolume = root.Q("fieldChimeVolume") as TextField;
        toggleEnvironmentSound = root.Q("toggleEnvironmentSound") as Toggle;
        sliderEnvironmentVolume = root.Q("sliderEnvironmentVolume") as Slider;
        fieldEnvironmentVolume = root.Q("fieldEnvironmentVolume") as TextField;
        toggleBandpassEnable = root.Q("toggleBandpassEnable") as Toggle;
        sliderBandpassFreq = root.Q("sliderBandpassFreq") as Slider;
        fieldBandpassFreq = root.Q("fieldBandpassFreq") as TextField;
        sliderBandpassQ = root.Q("sliderBandpassQ") as Slider;
        fieldBandpassQ = root.Q("fieldBandpassQ") as TextField;
        toggleParamEQEnable = root.Q("toggleParamEQEnable") as Toggle;
        sliderParamEQFreq = root.Q("sliderParamEQFreq") as Slider;
        fieldParamEQFreq = root.Q("fieldParamEQFreq") as TextField;
        sliderParamEQRange = root.Q("sliderParamEQRange") as Slider;
        fieldParamEQRange = root.Q("fieldParamEQRange") as TextField;
        sliderParamEQGain = root.Q("sliderParamEQGain") as Slider;
        fieldParamEQGain = root.Q("fieldParamEQGain") as TextField;
        listSoundPosition = root.Q("SoundObjectPositionList") as ScrollView;
        toggleShowTSNSTablet = root.Q("toggleShowTSNSTablet") as Toggle;
        toggleShowRatingTablet = root.Q("toggleShowRatingTablet") as Toggle;

        toggleDoGhosting = root.Q("toggleDoGhosting") as Toggle;


        selectorNewFile = root.Q("selectorNewFile") as RadioButton;
        fieldNewFile = root.Q("fieldNewFile") as TextField;
        selectorExistingFile = root.Q("selectorExistingFile") as RadioButton;
        dropdownExistingFile = root.Q("dropdownExistingFile") as DropdownField;
        buttonSave = root.Q("buttonSave") as Button;
        buttonLoad = root.Q("buttonLoad") as Button;

        buttonRestoreHeadlockedObject = root.Q("buttonRestoreHeadlockedObject") as Button;

        buttonSceneTutorial.RegisterCallback<ClickEvent>(ChangeSceneTutorial);
        buttonScene1.RegisterCallback<ClickEvent>(ChangeScene1);
        buttonScene2.RegisterCallback<ClickEvent>(ChangeScene2);
        buttonScene3.RegisterCallback<ClickEvent>(ChangeScene3);
        buttonCameraFirstPerson.RegisterCallback<ClickEvent>(ChangeCameraFirstPerson);
        buttonCameraFixed.RegisterCallback<ClickEvent>(ChangeCameraToFixed);
        toggleRadioVisibility.RegisterValueChangedCallback<bool>(ToggleRadioVisibility);
        toggleRadioSound.RegisterValueChangedCallback<bool>(ToggleRadioSound);
        sliderRadioVolume.RegisterValueChangedCallback<float>(ChangeRadioVolumeSlider);
        fieldRadioVolume.RegisterValueChangedCallback<string>(ChangeRadioVolumeField);
        if (EnvironmentAudioMixer != null) {
            toggleEnvironmentSound.RegisterValueChangedCallback<bool>(ToggleEnvironmentSound);
            sliderEnvironmentVolume.RegisterValueChangedCallback<float>(ChangeEnvironmentVolumeSlider);
            fieldEnvironmentVolume.RegisterValueChangedCallback<string>(ChangeEnvironmentVolumeField);
        }
        if (chimes != null) {
            toggleChimeSound.RegisterValueChangedCallback<bool>(ToggleChimeSound);
            sliderChimeVolume.RegisterValueChangedCallback<float>(ChangeChimeVolumeSlider);
            fieldChimeVolume.RegisterValueChangedCallback<string>(ChangeChimeVolumeField);
        }
        
        toggleBandpassEnable.RegisterValueChangedCallback<bool>(ToggleBandpass);
        sliderBandpassFreq.RegisterValueChangedCallback<float>(ChangeBandpassFreqSlider);
        fieldBandpassFreq.RegisterValueChangedCallback<string>(ChangeBandpassFreqField);
        sliderBandpassQ.RegisterValueChangedCallback<float>(ChangeBandpassQSlider);
        fieldBandpassQ.RegisterValueChangedCallback<string>(ChangeBandpassQField);
        toggleParamEQEnable.RegisterValueChangedCallback<bool>(ToggleParamEQ);
        sliderParamEQFreq.RegisterValueChangedCallback<float>(ChangeParamEQFreqSlider);
        fieldParamEQFreq.RegisterValueChangedCallback<string>(ChangeParamEQFreqField);
        sliderParamEQRange.RegisterValueChangedCallback<float>(ChangeParamEQRangeSlider);
        fieldParamEQRange.RegisterValueChangedCallback<string>(ChangeParamEQRangeField);
        sliderParamEQGain.RegisterValueChangedCallback<float>(ChangeParamEQGainSlider);
        fieldParamEQGain.RegisterValueChangedCallback<string>(ChangeParamEQGainField);
            // Create and populate the list of positions
        var items = new List<SoundPosition>
        {
            new SoundPosition { Name = "Position 1", ImagePath = "Assets/Images/position0.png"},
            new SoundPosition { Name = "Position 2", ImagePath = "Assets/Images/position30.png"},
            new SoundPosition { Name = "Position 2", ImagePath = "Assets/Images/position60.png"},
            new SoundPosition { Name = "Position 3", ImagePath = "Assets/Images/position90.png"},
            new SoundPosition { Name = "Position 4", ImagePath = "Assets/Images/position120.png"},
            new SoundPosition { Name = "Position 5", ImagePath = "Assets/Images/position150.png"},
            new SoundPosition { Name = "Position 6", ImagePath = "Assets/Images/position180.png"},
        };

        foreach (var item in items)
        {
            var button = new Button(() => Debug.Log(item.Name)) // Assuming you want to log the name when clicked
            {
                text = item.Name
            };
                    // Create an Image element and set its source
            var image = new Image();
            image.image = AssetDatabase.LoadAssetAtPath<Texture2D>(item.ImagePath); // Make sure to include using UnityEditor;
            image.scaleMode = ScaleMode.ScaleToFit; // Adjust scale mode as needed

            // Optionally, set the USS class for the image for further styling
            image.AddToClassList("button-image");

            // Add the Image element to the button
            button.Add(image);

            // Add the button to the container
            listSoundPosition.Add(button);
        }
        listSoundPosition.AddToClassList("grid-container"); 
        toggleShowTSNSTablet.RegisterValueChangedCallback<bool>(ToggleTSNSTablet);
        toggleShowRatingTablet.RegisterValueChangedCallback<bool>(ToggleRatingTablet);
        toggleDoGhosting.RegisterValueChangedCallback<bool>(ToggleDoGhosting);

        selectorNewFile.RegisterCallback<ClickEvent>(SelectNewFile);
        selectorExistingFile.RegisterCallback<ClickEvent>(SelectExistingFile);
        buttonSave.RegisterCallback<ClickEvent>(SaveSettings);
        buttonLoad.RegisterCallback<ClickEvent>(LoadSettings);

        buttonRestoreHeadlockedObject.RegisterCallback<ClickEvent>(TryRestoreHeadlockedObject);

        radioAudioSource = radio.GetComponent<AudioSource>();
        radioRenderer = radio.GetComponent<Renderer>();
        radioMixer = radioAudioSource.outputAudioMixerGroup.audioMixer;
        bandPassFilter = radio.GetComponent<BandPassFilter>();

        if (chimes != null) {
            chimeMixer = chimes.GetComponent<ChimeConfiguration>().audioMixerGroup.audioMixer;
        }

        // Force limits and init starting values
        sliderRadioVolume.lowValue = 0;
        sliderRadioVolume.highValue = 100;

        // Get a reference to the spectator camera
        spectator = GameObject.Find("Spectator Camera").GetComponent<SpectatorCamera>();
        if (spectator == null) {
            Debug.LogError("Failed to find the spectator camera!");
        }

        RetrieveSettings();
        
        if (chimes == null) {
            toggleChimeSound.SetEnabled(false);
            sliderChimeVolume.SetEnabled(false);
            fieldChimeVolume.SetEnabled(false);
        }
        if (EnvironmentAudioMixer == null) {
            toggleEnvironmentSound.SetEnabled(false);
            sliderEnvironmentVolume.SetEnabled(false);
            fieldEnvironmentVolume.SetEnabled(false);
        }
        if (RatingTablet == null) {
            toggleShowRatingTablet.SetEnabled(false);
        }

        if (ghosts == null) {
            ghosts = new List<PositioningAssistGhost>();
        }
        ToggleDoGhosting(null);

        dropdownExistingFile.choices.Clear();
        dropdownExistingFile.choices = Settings.Instance.GetSaveFilenames();

        // inital state of save/load
        SelectNewFile(null);

        // subscribe to event: RetrieveSettings is called when new settings are loaded.
        Settings.onLoad += RetrieveSettings;
        
    }

    private void RetrieveSettings()
    {
        // Change values to the ones present in Settings, or to a fallback/default value.
        // Called everytime new settings are loaded.
        if (Settings.Instance == null)
            {
                Debug.LogError("Settings.Instance is null. Ensure Settings is properly initialized.");
                return;
            }
        Settings settings = Settings.Instance;


        // Perspective setting
        bool spectatorIsFirstPerson;
        if (settings.Contains("spectatorIsFirstPerson")) {
            settings.GetValue("spectatorIsFirstPerson", out spectatorIsFirstPerson);
        } else {
            spectatorIsFirstPerson = false;
        }
        if (spectatorIsFirstPerson) {
            ChangeCameraFirstPerson(null);
        } else {
            ChangeCameraToFixed(null);
        }

        // bool radioIsVisible;
        // if (settings.Contains("radioIsVisible")) {
        //     settings.GetValue("radioIsVisible", out radioIsVisible);
        // } else {
        //     radioIsVisible = true;
        // }
        // if (radioIsVisible){
        //     radioRenderer.enabled = true;
        // } else {
        //     radioRenderer.enabled = false;
        // }

        // Radio settings
        bool radioIsPlaying;
        if (settings.Contains("radioIsPlaying")) {
            settings.GetValue("radioIsPlaying", out radioIsPlaying);
        } else {
            radioIsPlaying = false;
        }
        toggleRadioSound.value = radioIsPlaying;
        ToggleRadioSound(null);

        float radioVolume;
        if (settings.Contains("radioVolume")) {
            settings.GetValue("radioVolume", out radioVolume);
        } else {
            radioVolume = 50.0f;
        }
        sliderRadioVolume.value = radioVolume;
        ChangeRadioVolumeSlider(null);


        // Radio noise filter settings
        bool radioFilterBandpassIsOn;
        if (settings.Contains("radioFilterBandpassIsOn")) {
            settings.GetValue("radioFilterBandpassIsOn", out radioFilterBandpassIsOn);
        } else {
            radioFilterBandpassIsOn = false;
        }
        toggleBandpassEnable.value = radioFilterBandpassIsOn;
        ToggleBandpass(null);

        float radioFilterBandpassCenterFrequency;
        if (settings.Contains("radioFilterBandpassCenterFrequency")) {
            settings.GetValue("radioFilterBandpassCenterFrequency", out radioFilterBandpassCenterFrequency);
        } else {
            radioFilterBandpassCenterFrequency = 87.8136f;
        }
        sliderBandpassFreq.value = radioFilterBandpassCenterFrequency;
        ChangeBandpassFreqSlider(null);

        float radioFilterBandpassQFactor;
        if (settings.Contains("radioFilterBandpassQFactor")) {
            settings.GetValue("radioFilterBandpassQFactor", out radioFilterBandpassQFactor);
        } else {
            radioFilterBandpassQFactor = 1.0f;
        }
        sliderBandpassQ.value = radioFilterBandpassQFactor;
        ChangeBandpassQSlider(null);

        float radioFilterParamEQCenterFrequency;
        if (settings.Contains("radioFilterParamEQCenterFrequency")) {
            settings.GetValue("radioFilterParamEQCenterFrequency", out radioFilterParamEQCenterFrequency);
        } else {
            radioFilterParamEQCenterFrequency = 87.8136f;
        }
        sliderParamEQFreq.value = radioFilterParamEQCenterFrequency;
        ChangeParamEQFreqSlider(null);

        float radioFilterParamEQOctaveRange;
        if (settings.Contains("radioFilterParamEQOctaveRange")) {
            settings.GetValue("radioFilterParamEQOctaveRange", out radioFilterParamEQOctaveRange);
        } else {
            radioFilterParamEQOctaveRange = 1.0f;
        }
        sliderParamEQRange.value = radioFilterParamEQOctaveRange;
        ChangeParamEQRangeSlider(null);

        float radioFilterParamEQFrequencyGain;
        if (settings.Contains("radioFilterParamEQFrequencyGain")) {
            settings.GetValue("radioFilterParamEQFrequencyGain", out radioFilterParamEQFrequencyGain);
        } else {
            radioFilterParamEQFrequencyGain = 2.0f;
        }
        sliderParamEQGain.value = radioFilterParamEQFrequencyGain;
        ChangeParamEQGainSlider(null);

        bool radioFilterParamEQIsOn;
        if (settings.Contains("radioFilterParamEQIsOn")) {
            settings.GetValue("radioFilterParamEQIsOn", out radioFilterParamEQIsOn);
        } else {
            radioFilterParamEQIsOn = false;
        }
        toggleParamEQEnable.value = radioFilterParamEQIsOn;
        ToggleParamEQ(null);


        // Chimes settings
        bool chimesIsPlaying;
        if (settings.Contains("chimesIsPlaying")) {
            settings.GetValue("chimesIsPlaying", out chimesIsPlaying);
        } else {
            chimesIsPlaying = false;
        }
        toggleChimeSound.value = chimesIsPlaying;
        if (chimes != null) ToggleChimeSound(null);

        float chimesVolume;
        if (settings.Contains("chimesVolume")) {
            settings.GetValue("chimesVolume", out chimesVolume);
        } else {
            chimesVolume = 50.0f;
        }
        sliderChimeVolume.value = chimesVolume;
        if (chimes != null) ChangeChimeVolumeSlider(null);


        // Environment settings
        bool environmentIsPlaying;
        if (settings.Contains("environmentIsPlaying")) {
            settings.GetValue("environmentIsPlaying", out environmentIsPlaying);
        } else {
            environmentIsPlaying = false;
            settings.SetValue("environmentIsPlaying", false);
        }
        toggleEnvironmentSound.value = environmentIsPlaying;
        if (EnvironmentAudioMixer != null) ToggleEnvironmentSound(null);

        float environmentVolume;
        if (settings.Contains("environmentVolume")) {
            settings.GetValue("environmentVolume", out environmentVolume);
        } else {
            environmentVolume = 50.0f;
        }
        sliderEnvironmentVolume.value = environmentVolume;
        if (EnvironmentAudioMixer != null) ChangeEnvironmentVolumeSlider(null);


        // Feedback tablet active setting
        bool feedbackTabletIsPresent;
        if (settings.Contains("feedbackTabletIsPresent")) {
            settings.GetValue("feedbackTabletIsPresent", out feedbackTabletIsPresent);
        } else {
            feedbackTabletIsPresent = true;
        }
        toggleShowRatingTablet.value = feedbackTabletIsPresent;
        if (RatingTablet != null) ToggleRatingTablet(null);


        bool ghostsActive;
        if (settings.Contains("ghostsActive")) {
            settings.GetValue("ghostsActive", out ghostsActive);
        } else {
            ghostsActive = false;
        }
        toggleDoGhosting.value = ghostsActive;
        ToggleDoGhosting(null);


        // TSNS tablet is deliberately excluded(maybe?)
    }


    private void OnDisable()
    {
        // unsubscribe from event
        Settings.onLoad -= RetrieveSettings;

        // likely unneccessary, since the UI is re-instantiated each scene
        /*
        buttonSceneTutorial.UnregisterCallback<ClickEvent>(ChangeSceneTutorial);
        buttonScene1.UnregisterCallback<ClickEvent>(ChangeScene1);
        buttonScene2.UnregisterCallback<ClickEvent>(ChangeScene1);
        buttonScene3.UnregisterCallback<ClickEvent>(ChangeScene1);
        buttonCameraFirstPerson.UnregisterCallback<ClickEvent>(ChangeCameraFirstPerson);
        buttonCameraFixed.UnregisterCallback<ClickEvent>(ChangeCameraToFixed);
        toggleSound.UnregisterValueChangedCallback<bool>(ToggleSound);
        sliderVolume.UnregisterValueChangedCallback<float>(ChangeVolumeSlider);
        //sliderPitch.UnregisterCallback<ClickEvent>(changePitch);
        toggleShowTSNSTablet.UnregisterValueChangedCallback<bool>(ToggleTSNSTablet);
        toggleShowRatingTablet.UnregisterValueChangedCallback<bool>(ToggleRatingTablet);
        */
    }
//     VisualElement MakeItem()
//     {
//         // Create a new button or any other VisualElement as the item template
//         var button = new Button();
//         button.AddToClassList("list-item-button"); // Add a USS class for styling
//         return button;
//     }

//     void BindItem(VisualElement element, int index)
//     {   
//         var button = (Button)element;
//         var item = (SoundPosition)listSoundPosition.itemsSource[index];
//         button.text = item.Name;
//         // Here you can also set the image for the button based on item.ImagePath
//         Debug.Log($"listSoundPosition count: {listSoundPosition.itemsSource.Count}");
//         foreach (var position in listSoundPosition.itemsSource) {
//         Debug.Log($"Position: {position}");
// }
//     }

    private void ChangeSceneTutorial(ClickEvent evt)
    {
        SceneManager.LoadScene("TutorialScene");
    }
    private void ChangeScene1(ClickEvent evt)
    {
        SceneManager.LoadScene("BlankWorld");
    }
    private void ChangeScene2(ClickEvent evt)
    {
        SceneManager.LoadScene("BeachWorld");
    }
    private void ChangeScene3(ClickEvent evt)
    {
        SceneManager.LoadScene("ForestWorld");
    }

    private void ChangeCameraFirstPerson(ClickEvent evt)
    {
        spectator.ToFirstPersonPerspective();
        Settings.Instance.SetValue("spectatorIsFirstPerson", true);
    }

    private void ChangeCameraToFixed(ClickEvent evt)
    {
        spectator.ToFixedPerspective();
        Settings.Instance.SetValue("spectatorIsFirstPerson", false);
    }

private void ToggleRadioVisibility(ChangeEvent<bool> evt)
{   
    if (toggleRadioVisibility.value == true) {
        radioRenderer.enabled = true;
    } else if (toggleRadioVisibility.value == false) {
        radioRenderer.enabled = false;
    }
    Settings.Instance.SetValue("radioIsVisible", toggleRadioVisibility.value);
}

    private void ToggleRadioSound(ChangeEvent<bool> evt)
    {
        // play/stop radio sound. If the radio isn't active, playOnAwake is set.
        if (toggleRadioSound.value == true) {
            if (radioAudioSource.isActiveAndEnabled) {
                radioAudioSource.Play();
            } else {
                radioAudioSource.playOnAwake = true;
            }
        } else if (toggleRadioSound.value == false) {
            if (radioAudioSource.isActiveAndEnabled) {
                radioAudioSource.Stop();
            } else {
                radioAudioSource.playOnAwake = false;
            }
        }
        Settings.Instance.SetValue("radioIsPlaying", toggleRadioSound.value);
    }

    private void ToggleTSNSTablet(ChangeEvent<bool> evt)
    {
        if (toggleShowTSNSTablet.value == true) {
            // Create a new rating tablet from prefab
            RatingTabletToggleable = Instantiate(RatingTabletToggleablePrefab, RatingTabletToggleableSpawnpoint.position, RatingTabletToggleableSpawnpoint.rotation);
        } else if (toggleShowTSNSTablet.value == false) {
            // Destroy the rating tablet
            Destroy(RatingTabletToggleable);
        }
    }

    private void ToggleRatingTablet(ChangeEvent<bool> evt)
    {
        if (toggleShowRatingTablet.value == true) {
            RatingTablet.SetActive(true);
        } else if (toggleShowRatingTablet.value == false) {
            RatingTablet.SetActive(false);
        }
        Settings.Instance.SetValue("feedbackTabletIsPresent", toggleShowRatingTablet.value);
    }

    private void ToggleDoGhosting(ChangeEvent<bool> evt)
    {
        if (toggleDoGhosting.value == true) {
            foreach (PositioningAssistGhost ghost in ghosts) {
                ghost.enabled = true;
            }
        } else if (toggleDoGhosting.value == false) {
            foreach (PositioningAssistGhost ghost in ghosts) {
                ghost.enabled = false;
            }
        }
        Settings.Instance.SetValue("ghostsActive", toggleDoGhosting.value);
    }

    private void ChangeRadioVolumeSlider(ChangeEvent<float> evt)
    {
        radioAudioSource.volume = sliderRadioVolume.value / 100.0f;
        // Change the text field to show the current volume
        fieldRadioVolume.value = Convert.ToInt32(sliderRadioVolume.value) + "%";
        fieldRadioVolumePreviousValue = fieldRadioVolume.value;


        Settings.Instance.SetValue("radioVolume", sliderRadioVolume.value);
    }

    private void ChangeRadioVolumeField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldRadioVolume.value == fieldRadioVolumePreviousValue) {
            return;
        }

        // ignore % sign
        string input = fieldRadioVolume.value;
        input = input.TrimEnd('%');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            if (value < sliderRadioVolume.lowValue) {
                value = Convert.ToInt32(sliderRadioVolume.lowValue);
            } else if (value > sliderRadioVolume.highValue) {
                value = Convert.ToInt32(sliderRadioVolume.highValue);
            }

            // pass the value to the slider
            sliderRadioVolume.value = value;
            
            // append % sign
            input += '%';
            fieldRadioVolumePreviousValue = input;
            fieldRadioVolume.value = input;

        } else {
            // invalid, restore previous value
            fieldRadioVolume.value = fieldRadioVolumePreviousValue;
        }
    }

    private void ToggleChimeSound(ChangeEvent<bool> evt)
    {
        // Enable/disable the chimes playing by themselves.
        if (toggleChimeSound.value == true) {
            // on
            chimes.GetComponent<ChimeRandomRinger>().ForceEnabled(true);
        } else if (toggleChimeSound.value == false) {
            // off
            chimes.GetComponent<ChimeRandomRinger>().ForceEnabled(false);

        }

        Settings.Instance.SetValue("chimesIsPlaying", toggleChimeSound.value);
    }

    private void ChangeChimeVolumeSlider(ChangeEvent<float> evt)
    {
        chimeMixer.SetFloat("ChimeVolume", ToDecibel(sliderChimeVolume.value));

        // Change the text field to show the current volume
        fieldChimeVolume.value = Convert.ToInt32(sliderChimeVolume.value) + "%";
        fieldChimeVolumePreviousValue = fieldChimeVolume.value;

        Settings.Instance.SetValue("chimesVolume", sliderChimeVolume.value);
    }

    private void ChangeChimeVolumeField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldChimeVolume.value == fieldChimeVolumePreviousValue) {
            return;
        }

        // ignore % sign
        string input = fieldChimeVolume.value;
        input = input.TrimEnd('%');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            if (value < sliderChimeVolume.lowValue) {
                value = Convert.ToInt32(sliderChimeVolume.lowValue);
            } else if (value > sliderChimeVolume.highValue) {
                value = Convert.ToInt32(sliderChimeVolume.highValue);
            }

            // pass the value to the slider
            sliderChimeVolume.value = value;

            // append % sign
            input += '%';
            fieldChimeVolumePreviousValue = input;
            fieldChimeVolume.value = input;

        } else {
            // invalid, restore previous value
            fieldChimeVolume.value = fieldChimeVolumePreviousValue;
        }
    }

    private void ToggleEnvironmentSound(ChangeEvent<bool> evt)
    {
        // mute all environment sound using the AudioMixer.

        // known issue: environment sounds are not properly muted here when entering a new scene, currently doing it in Update()
        if (toggleEnvironmentSound.value == true) {
            // on
            //Debug.Log("Environment sound on");
            EnvironmentAudioMixer.SetFloat("EnvironmentVolume", ToDecibel(sliderEnvironmentVolume.value));
        } else if (toggleEnvironmentSound.value == false) {
            // off
            //Debug.Log("Environment sound off");
            EnvironmentAudioMixer.SetFloat("EnvironmentVolume", -80.00f);
        }
        Settings.Instance.SetValue("environmentIsPlaying", toggleEnvironmentSound.value);
    }

    private void ChangeEnvironmentVolumeSlider(ChangeEvent<float> evt)
    {
        EnvironmentAudioMixer.SetFloat("EnvironmentVolume", ToDecibel(sliderEnvironmentVolume.value));

        // Change the text field to show the current volume
        fieldEnvironmentVolume.value = Convert.ToInt32(sliderEnvironmentVolume.value) + "%";
        fieldEnvironmentVolumePreviousValue = fieldEnvironmentVolume.value;

        Settings.Instance.SetValue("environmentVolume", sliderEnvironmentVolume.value);
    }

    private void ChangeEnvironmentVolumeField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldEnvironmentVolume.value == fieldEnvironmentVolumePreviousValue) {
            return;
        }

        // ignore % sign
        string input = fieldEnvironmentVolume.value;
        input = input.TrimEnd('%');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            if (value < sliderEnvironmentVolume.lowValue) {
                value = Convert.ToInt32(sliderEnvironmentVolume.lowValue);
            } else if (value > sliderEnvironmentVolume.highValue) {
                value = Convert.ToInt32(sliderEnvironmentVolume.highValue);
            }

            // pass the value to the slider
            sliderEnvironmentVolume.value = value;

            // append % sign
            input += '%';
            fieldEnvironmentVolumePreviousValue = input;
            fieldEnvironmentVolume.value = input;

        } else {
            // invalid, restore previous value
            fieldEnvironmentVolume.value = fieldEnvironmentVolumePreviousValue;
        }
    }

    private void ToggleBandpass(ChangeEvent<bool> evt)
    {
        if (toggleBandpassEnable.value == true) {
            bandPassFilter.enabled = true;
        } else {
            bandPassFilter.enabled = false;
        }
        Settings.Instance.SetValue("radioFilterBandpassIsOn", toggleBandpassEnable.value);
    }

    private void ChangeBandpassFreqSlider(ChangeEvent<float> evt)
    {
        // the slider value is the exponent of the actual value.
        // this is done so that the slider is biased toward lower values
        // and because human hearing interprets pitch logarithmically
        float value = Mathf.Pow(2, sliderBandpassFreq.value / 10);

        bandPassFilter.CutoffFrequency = value;
        // Change the text field to show the frequency
        fieldBandpassFreq.value = Convert.ToInt32(value) + "Hz";
        fieldBandpassFreqPreviousValue = fieldBandpassFreq.value;
        Settings.Instance.SetValue("radioFilterBandpassCenterFrequency", sliderBandpassFreq.value);
    }

    private void ChangeBandpassFreqField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldBandpassFreq.value == fieldBandpassFreqPreviousValue) {
            return;
        }

        // ignore Hz (units)
        string input = fieldBandpassFreq.value;
        input = input.TrimEnd('z');
        input = input.TrimEnd('H');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            value = Convert.ToInt32(Mathf.Clamp((float)value, Mathf.Pow(2, sliderBandpassFreq.lowValue / 10), Mathf.Pow(2, sliderBandpassFreq.highValue / 10)));

            // pass the value to the slider
            sliderBandpassFreq.value = 10 * (Mathf.Log10(value) / Mathf.Log10(2));

            // append Hz (units)
            fieldBandpassFreq.value = value.ToString() + "Hz";
            fieldBandpassFreqPreviousValue = fieldBandpassFreq.value;

        } else {
            // invalid, restore previous value
            fieldBandpassFreq.value = fieldBandpassFreqPreviousValue;
        }

    }

    private void ChangeBandpassQSlider(ChangeEvent<float> evt)
    {
        bandPassFilter.q = sliderBandpassQ.value;
        // Change the text field to show the Q factor
        fieldBandpassQ.value = bandPassFilter.q.ToString("0.00");
        fieldBandpassQPreviousValue = fieldBandpassQ.value;
        Settings.Instance.SetValue("radioFilterBandpassQFactor", sliderBandpassQ.value);
    }

    private void ChangeBandpassQField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldBandpassQ.value == fieldBandpassQPreviousValue) {
            return;
        }

        // Check if the input is valid
        string input = fieldBandpassQ.value;
        float value;
        if (float.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            if (value < sliderBandpassQ.lowValue) {
                value = sliderBandpassQ.lowValue;
            } else if (value > sliderBandpassQ.highValue) {
                value = sliderBandpassQ.highValue;
            }

            fieldBandpassQ.value = value.ToString("0.00");
            fieldBandpassQPreviousValue = fieldBandpassQ.value;

            // pass the value to the slider
            sliderBandpassQ.value = value;

        } else {
            // invalid, restore previous value
            fieldBandpassQ.value = fieldBandpassQPreviousValue;
        }
    }

    private void ToggleParamEQ(ChangeEvent<bool> evt)
    {
        // Disable ParamEQ by setting gain to 1.
        if (toggleParamEQEnable.value == true) {
            radioMixer.SetFloat("ParamEQGain", sliderParamEQGain.value);
        } else {
            radioMixer.SetFloat("ParamEQGain", 1.00f);
        }
        Settings.Instance.SetValue("radioFilterParamEQIsOn", toggleParamEQEnable.value);
    }
    
    private void ChangeParamEQFreqSlider(ChangeEvent<float> evt)
    {
        radioMixer.SetFloat("ParamEQFreq", sliderParamEQFreq.value);

        // the slider value is the exponent of the actual value.
        // this is done so that the slider is biased toward lower values
        // and because human hearing interprets pitch logarithmically
        float value = Mathf.Pow(2, sliderParamEQFreq.value / 10);

        radioMixer.SetFloat("ParamEQFreq", value);
        // Change the text field to show the frequency
        fieldParamEQFreq.value = Convert.ToInt32(value) + "Hz";
        fieldParamEQFreqPreviousValue = fieldBandpassFreq.value;
        Settings.Instance.SetValue("radioFilterParamEQCenterFrequency", sliderParamEQFreq.value);
    }

    private void ChangeParamEQFreqField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldParamEQFreq.value == fieldParamEQFreqPreviousValue) {
            return;
        }

        // ignore Hz (units)
        string input = fieldParamEQFreq.value;
        input = input.TrimEnd('z');
        input = input.TrimEnd('H');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            value = Convert.ToInt32(Mathf.Clamp((float)value, Mathf.Pow(2, sliderParamEQFreq.lowValue / 10), Mathf.Pow(2, sliderParamEQFreq.highValue / 10)));

            // pass the value to the slider
            sliderParamEQFreq.value = 10 * (Mathf.Log10(value) / Mathf.Log10(2));

            // append Hz (units)
            fieldParamEQFreq.value = value.ToString() + "Hz";
            fieldParamEQFreqPreviousValue = fieldParamEQFreq.value;

        } else {
            // invalid, restore previous value
            fieldParamEQFreq.value = fieldParamEQFreqPreviousValue;
        }
    }

    private void ChangeParamEQRangeSlider(ChangeEvent<float> evt)
    {
        radioMixer.SetFloat("ParamEQRange", sliderParamEQRange.value);
        // Change the text field to show the octave range
        fieldParamEQRange.value = sliderParamEQRange.value.ToString("0.00");
        fieldParamEQRangePreviousValue = fieldParamEQRange.value;
        Settings.Instance.SetValue("radioFilterParamEQOctaveRange", sliderParamEQRange.value);
    }

    private void ChangeParamEQRangeField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldParamEQRange.value == fieldParamEQRangePreviousValue) {
            return;
        }

        // Check if the input is valid
        string input = fieldParamEQRange.value;
        float value;
        if (float.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            if (value < sliderParamEQRange.lowValue) {
                value = sliderParamEQRange.lowValue;
            } else if (value > sliderParamEQRange.highValue) {
                value = sliderParamEQRange.highValue;
            }

            fieldParamEQRange.value = value.ToString("0.00");
            fieldParamEQRangePreviousValue = fieldParamEQRange.value;

            // pass the value to the slider
            sliderParamEQRange.value = value;

        } else {
            // invalid, restore previous value
            fieldParamEQRange.value = fieldParamEQRangePreviousValue;
        }
    }

    private void ChangeParamEQGainSlider(ChangeEvent<float> evt)
    {
        radioMixer.SetFloat("ParamEQGain", sliderParamEQGain.value);
        // Change the text field to show the gain factor
        fieldParamEQGain.value = sliderParamEQGain.value.ToString("0.00");
        fieldParamEQGainPreviousValue = fieldParamEQGain.value;
        Settings.Instance.SetValue("radioFilterParamEQFrequencyGain", sliderParamEQGain.value);
    }

    private void ChangeParamEQGainField(ChangeEvent<string> evt)
    {
        // do nothing if the value hasn't changed
        if (fieldParamEQGain.value == fieldParamEQGainPreviousValue) {
            return;
        }

        // Check if the input is valid
        string input = fieldParamEQGain.value;
        float value;
        if (float.TryParse(input, out value)) {
            // input is valid

            // enforce limits
            if (value < sliderParamEQGain.lowValue) {
                value = sliderParamEQGain.lowValue;
            } else if (value > sliderParamEQGain.highValue) {
                value = sliderParamEQGain.highValue;
            }

            fieldParamEQGain.value = value.ToString("0.00");
            fieldParamEQGainPreviousValue = fieldParamEQGain.value;

            // pass the value to the slider
            sliderParamEQGain.value = value;

        } else {
            // invalid, restore previous value
            fieldParamEQGain.value = fieldParamEQGainPreviousValue;
        }
    }

    private float ToDecibel(float percentage)
    {
        // Convert a percentage (0 to 100) to decibels as used by AudioMixer (-80 to 20)
        // piecewise interpolation such that 50% corresponds to 0 decibels gain.
        if (percentage < 50) {
            return Mathf.Lerp(-80, 0, (-0.026f * percentage * percentage + 3.3f * percentage) / 100);
        } else {
            return Mathf.Lerp(0, 20, (percentage - 50) / 50);
        }
    }

    private void SelectNewFile(ClickEvent evt)
    {
        newFileIsSelected = true;
        selectorNewFile.value = true;
        selectorExistingFile.value = false;
        fieldNewFile.SetEnabled(true);
        dropdownExistingFile.SetEnabled(false);
    }

    private void SelectExistingFile(ClickEvent evt)
    {
        newFileIsSelected = false;
        selectorNewFile.value = false;
        selectorExistingFile.value = true;
        fieldNewFile.SetEnabled(false);
        dropdownExistingFile.SetEnabled(true);
    }

    private void SaveSettings(ClickEvent evt)
    {
        // save settings with a name chosen by either the New File text field or Existing File dropdown
        if (newFileIsSelected) {
            if (fieldNewFile.value.Length > 1) {
                Settings.Instance.Save(fieldNewFile.value);
                // update the dropdown so it shows the new file
                dropdownExistingFile.choices.Clear();
                dropdownExistingFile.choices = Settings.Instance.GetSaveFilenames();
            }
        } else {
            if ((dropdownExistingFile.value != null) && (dropdownExistingFile.value.Length > 1)) {
                Settings.Instance.Save(dropdownExistingFile.value);
            }
        }
    }

    private void LoadSettings(ClickEvent evt)
    {
        // load settings with a name chosen by either the New File text field or Existing File dropdown
        if (newFileIsSelected) {
            if (fieldNewFile.value.Length > 1) {
                Settings.Instance.Load(fieldNewFile.value);
            }
        } else {
            if ((dropdownExistingFile.value != null) && (dropdownExistingFile.value.Length > 1)) {
                Settings.Instance.Load(dropdownExistingFile.value);
            }
        }
    }

    private void TryRestoreHeadlockedObject(ClickEvent evt)
    {
        if (Settings.Instance.Contains("lastHeadlockedObject")) {
            string objectName;
            Settings.Instance.GetValue("lastHeadlockedObject", out objectName);
            ParentSetter headlock = GameObject.Find(objectName).GetComponent<ParentSetter>();
            // Ensure the object isn't already headlocked
            if (headlock.parent == null) {
                headlock.TryRestoreLastHeadlock();
            }
        }
    }

    private void Update()
    {
        // cheap fix to ensure evironment sounds are properly muted when loading scene
        if (EnvironmentAudioMixer != null) {
            if (toggleEnvironmentSound.value == true) {
                EnvironmentAudioMixer.SetFloat("EnvironmentVolume", ToDecibel(sliderEnvironmentVolume.value));
            } else {
                EnvironmentAudioMixer.SetFloat("EnvironmentVolume", -80.00f);
            }
        }
    }


}
