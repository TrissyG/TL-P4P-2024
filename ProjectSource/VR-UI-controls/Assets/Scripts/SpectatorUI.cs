using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using AudioSourceManagement;

public class SpectatorUI : MonoBehaviour
{
    private Button buttonSceneTutorial;
    private Button buttonScene1;
    private Button buttonScene2;
    private Button buttonScene3;
    private Button buttonCameraFirstPerson;
    private Button buttonCameraFixed;

    private Toggle toggleRadioVisible;
    private Toggle toggleAudioSourceVisible;
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

    // Locationing mode buttons
    private Button buttonLocationingModeOn;
    private Button buttonLocationingModeOff;

    private TextField fieldLocationingPreset;
    private Button buttonSetRadioPosition1;
    private Button buttonSetRadioPosition2;
    private Button buttonSetRadioPosition3;
    private Button buttonSetRadioPosition4;
    private Button buttonSetRadioPosition5;

    private Slider sliderSoundOffsetRadius;
    private readonly int[] steps = { 0, 1, 2, 3, 4 };
    private float radius = 0.0f;
    private float azimuth = 0.0f;
    private float inclination = 0.0f;
    // Inclination buttons
    private Button buttonYZ_0;
    private Button buttonYZ_45;
    private Button buttonYZ_90;
    private Button buttonYZ_135;
    private Button buttonYZ_180;
    private Button buttonYZ_225;
    private Button buttonYZ_270;
    private Button buttonYZ_315;
    // Azimuth buttons
    private Button buttonXZ_0;
    private Button buttonXZ_45;
    private Button buttonXZ_90;
    private Button buttonXZ_135;
    private Button buttonXZ_180;
    private Button buttonXZ_225;
    private Button buttonXZ_270;
    private Button buttonXZ_315;




    // Exit application button
    private Button buttonExitApplication;


    // audio source to control
    public GameObject radioPolygon; // the rendered radio object 'Radio'
    public GameObject radio; // the 'Audio Source' GameObject child of the rendered object

    public GameObject audioSourceManagerPrefab;
    public AudioSourceManager audioSourceManager;
    private MeshRenderer radioAudioSourceRenderer; // if we want to toggle visibility of the audio source
    private AudioSource radioAudioSource;

    private AudioMixer radioMixer;
    private BandPassFilter bandPassFilter;

    private MeshRenderer radioMeshRenderer;

    public GameObject chimes;
    private AudioMixer chimeMixer;

    public GameObject locationingChair;

    public Transform RadioSpawnpoint;
    Vector3[] locationingPositions;


    // Position where RatingTabletToggleable appears
    public Transform RatingTabletToggleableSpawnpoint;
    // Prefab of RatingTabletToggleable
    public GameObject RatingTabletToggleablePrefab;
    // Instance of RatingTabletToggleable
    private GameObject RatingTabletToggleable;

    public GameObject RatingTablet;

    public List<PositioningAssistGhost> ghosts;

    public AudioMixer EnvironmentAudioMixer;

    // spectator cameras
    private SpectatorCamera spectator;
    private DataLoggingManager _dataLoggingManager;

    private DataLoggingManager _dataLoggingManager;
    private InputData _inputData;

    GameObject rightController;
    GameObject leftController;

    private void OnEnable()
    {   
        _dataLoggingManager = FindObjectOfType<DataLoggingManager>();
        // The UXML is already instantiated by the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;


        buttonSceneTutorial = root.Q("buttonSceneTutorial") as Button;
        buttonScene1 = root.Q("buttonScene1") as Button;
        buttonScene2 = root.Q("buttonScene2") as Button;
        buttonScene3 = root.Q("buttonScene3") as Button;
        buttonCameraFirstPerson = root.Q("buttonCameraFirstPerson") as Button;
        buttonCameraFixed = root.Q("buttonCameraFixed") as Button;
        toggleRadioVisible = root.Q("toggleRadioVisible") as Toggle;
        toggleAudioSourceVisible = root.Q("toggleAudioSourceVisible") as Toggle;
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

        buttonLocationingModeOn = root.Q("buttonLocationingModeOn") as Button;
        buttonLocationingModeOff = root.Q("buttonLocationingModeOff") as Button;

        fieldLocationingPreset = root.Q("fieldLocationingPreset") as TextField;

        buttonSetRadioPosition1 = root.Q<Button>("buttonSetRadioPosition1");
        buttonSetRadioPosition2 = root.Q<Button>("buttonSetRadioPosition2");
        buttonSetRadioPosition3 = root.Q<Button>("buttonSetRadioPosition3");
        buttonSetRadioPosition4 = root.Q<Button>("buttonSetRadioPosition4");
        buttonSetRadioPosition5 = root.Q<Button>("buttonSetRadioPosition5");

        sliderSoundOffsetRadius = root.Q<Slider>("sliderSoundOffsetRadius");
        // YZ rotation buttons (inclination - perpendicular to upright radio)
        buttonYZ_0 = root.Q<Button>("buttonYZ_0");
        buttonYZ_45 = root.Q<Button>("buttonYZ_45");
        buttonYZ_90 = root.Q<Button>("buttonYZ_90");
        buttonYZ_135 = root.Q<Button>("buttonYZ_135");
        buttonYZ_180 = root.Q<Button>("buttonYZ_180");
        buttonYZ_225 = root.Q<Button>("buttonYZ_225");
        buttonYZ_270 = root.Q<Button>("buttonYZ_270");
        buttonYZ_315 = root.Q<Button>("buttonYZ_315");
        // XZ rotation buttons (azimuth - parallel to upright radio)
        buttonXZ_0 = root.Q<Button>("buttonXZ_0");
        buttonXZ_45 = root.Q<Button>("buttonXZ_45");
        buttonXZ_90 = root.Q<Button>("buttonXZ_90");
        buttonXZ_135 = root.Q<Button>("buttonXZ_135");
        buttonXZ_180 = root.Q<Button>("buttonXZ_180");
        buttonXZ_225 = root.Q<Button>("buttonXZ_225");
        buttonXZ_270 = root.Q<Button>("buttonXZ_270");
        buttonXZ_315 = root.Q<Button>("buttonXZ_315");


        buttonExitApplication = root.Q("buttonExitApplication") as Button;

        buttonSceneTutorial.RegisterCallback<ClickEvent>(ChangeSceneTutorial);
        buttonScene1.RegisterCallback<ClickEvent>(ChangeScene1);
        buttonScene2.RegisterCallback<ClickEvent>(ChangeScene2);
        buttonScene3.RegisterCallback<ClickEvent>(ChangeScene3);
        buttonCameraFirstPerson.RegisterCallback<ClickEvent>(ChangeCameraFirstPerson);
        buttonCameraFixed.RegisterCallback<ClickEvent>(ChangeCameraToFixed);
        toggleRadioVisible.RegisterValueChangedCallback<bool>(ToggleRadioVisible);
        toggleAudioSourceVisible.RegisterValueChangedCallback<bool>(ToggleAudioSourceVisible);
        toggleRadioSound.RegisterValueChangedCallback<bool>(ToggleRadioSound);
        sliderRadioVolume.RegisterValueChangedCallback<float>(ChangeRadioVolumeSlider);
        fieldRadioVolume.RegisterValueChangedCallback<string>(ChangeRadioVolumeField);
        if (EnvironmentAudioMixer != null)
        {
            toggleEnvironmentSound.RegisterValueChangedCallback<bool>(ToggleEnvironmentSound);
            sliderEnvironmentVolume.RegisterValueChangedCallback<float>(ChangeEnvironmentVolumeSlider);
            fieldEnvironmentVolume.RegisterValueChangedCallback<string>(ChangeEnvironmentVolumeField);
        }
        if (chimes != null)
        {
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


        toggleShowTSNSTablet.RegisterValueChangedCallback<bool>(ToggleTSNSTablet);
        toggleShowRatingTablet.RegisterValueChangedCallback<bool>(ToggleRatingTablet);
        toggleDoGhosting.RegisterValueChangedCallback<bool>(ToggleDoGhosting);

        selectorNewFile.RegisterCallback<ClickEvent>(SelectNewFile);
        selectorExistingFile.RegisterCallback<ClickEvent>(SelectExistingFile);
        buttonSave.RegisterCallback<ClickEvent>(SaveSettings);
        buttonLoad.RegisterCallback<ClickEvent>(LoadSettings);

        buttonRestoreHeadlockedObject.RegisterCallback<ClickEvent>(TryRestoreHeadlockedObject);

        buttonLocationingModeOn.RegisterCallback<ClickEvent>(ActivateLocationingMode);
        buttonLocationingModeOff.RegisterCallback<ClickEvent>(DeactivateLocationingMode);

        fieldLocationingPreset.RegisterValueChangedCallback<string>(SetOffsetPreset);

        buttonSetRadioPosition1.clicked += () => changeRadioLocation(4);
        buttonSetRadioPosition2.clicked += () => changeRadioLocation(3);
        buttonSetRadioPosition3.clicked += () => changeRadioLocation(2);
        buttonSetRadioPosition4.clicked += () => changeRadioLocation(1);
        buttonSetRadioPosition5.clicked += () => changeRadioLocation(0);

        buttonYZ_0.clicked += () => SetInclination(0);
        buttonYZ_45.clicked += () => SetInclination(45);
        buttonYZ_90.clicked += () => SetInclination(90);
        buttonYZ_135.clicked += () => SetInclination(135);
        buttonYZ_180.clicked += () => SetInclination(180);
        buttonYZ_225.clicked += () => SetInclination(225);
        buttonYZ_270.clicked += () => SetInclination(270);
        buttonYZ_315.clicked += () => SetInclination(315);

        buttonXZ_0.clicked += () => SetAzimuth(0);
        buttonXZ_45.clicked += () => SetAzimuth(45);
        buttonXZ_90.clicked += () => SetAzimuth(90);
        buttonXZ_135.clicked += () => SetAzimuth(135);
        buttonXZ_180.clicked += () => SetAzimuth(180);
        buttonXZ_225.clicked += () => SetAzimuth(225);
        buttonXZ_270.clicked += () => SetAzimuth(270);
        buttonXZ_315.clicked += () => SetAzimuth(315);

        if (SceneManager.GetActiveScene().name == "BlankWorld")
        {
            //_inputData = FindObjectOfType<InputData>();
            buttonLocationingModeOn.SetEnabled(true);
            buttonLocationingModeOff.SetEnabled(true);
            buttonSetRadioPosition1.SetEnabled(true);
            buttonSetRadioPosition2.SetEnabled(true);
            buttonSetRadioPosition3.SetEnabled(true);
            buttonSetRadioPosition4.SetEnabled(true);
            buttonSetRadioPosition5.SetEnabled(true);
            sliderSoundOffsetRadius.SetEnabled(true);
            buttonYZ_0.SetEnabled(true);
            buttonYZ_45.SetEnabled(true);
            buttonYZ_90.SetEnabled(true);
            buttonYZ_135.SetEnabled(true);
            buttonYZ_180.SetEnabled(true);
            buttonYZ_225.SetEnabled(true);
            buttonYZ_270.SetEnabled(true);
            buttonYZ_315.SetEnabled(true);
            buttonXZ_0.SetEnabled(true);
            buttonXZ_45.SetEnabled(true);
            buttonXZ_90.SetEnabled(true);
            buttonXZ_135.SetEnabled(true);
            buttonXZ_180.SetEnabled(true);
            buttonXZ_225.SetEnabled(true);
            buttonXZ_270.SetEnabled(true);
            buttonXZ_315.SetEnabled(true);

            //audioSourceManager = Instantiate(audioSourceManagerPrefab).GetComponent<AudioSourceManager>();
            if (audioSourceManager == null)
            {
                Debug.LogWarning("AudioSourceManager not found in the scene.");
                return;
            }

            SetSphericalCoordinates(radius, inclination, azimuth); // should be zero
        }
        else
        {
            buttonLocationingModeOn.SetEnabled(false);
            buttonLocationingModeOff.SetEnabled(false);
            buttonSetRadioPosition1.SetEnabled(false);
            buttonSetRadioPosition2.SetEnabled(false);
            buttonSetRadioPosition3.SetEnabled(false);
            buttonSetRadioPosition4.SetEnabled(false);
            buttonSetRadioPosition5.SetEnabled(false);
            sliderSoundOffsetRadius.SetEnabled(false);
            buttonYZ_0.SetEnabled(false);
            buttonYZ_45.SetEnabled(false);
            buttonYZ_90.SetEnabled(false);
            buttonYZ_135.SetEnabled(false);
            buttonYZ_180.SetEnabled(false);
            buttonYZ_225.SetEnabled(false);
            buttonYZ_270.SetEnabled(false);
            buttonYZ_315.SetEnabled(false);
            buttonXZ_0.SetEnabled(false);
            buttonXZ_45.SetEnabled(false);
            buttonXZ_90.SetEnabled(false);
            buttonXZ_135.SetEnabled(false);
            buttonXZ_180.SetEnabled(false);
            buttonXZ_225.SetEnabled(false);
            buttonXZ_270.SetEnabled(false);
            buttonXZ_315.SetEnabled(false);
        }

        buttonExitApplication.RegisterCallback<ClickEvent>(ExitApplication);

        // Get the mesh renderer of the radio, which allows us to toggle visibility without disabling the object.
        // meshrenderer of parent object, not the audio source "Radio" in this script
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        { // Tutorial scene doesn't have a radio
            if (radioPolygon == null)
            {
                Debug.LogError("Radio GameObject not found.");
                return;
            }
            radioMeshRenderer = radioPolygon.GetComponent<MeshRenderer>();

        }

        radioAudioSource = radio.GetComponent<AudioSource>();
        if (radioAudioSource == null)
        {
            Debug.LogError("AudioSource component not found on the Audio Source GameObject.");
            return;
        }
        radioAudioSourceRenderer = radio.GetComponent<MeshRenderer>();
        radioMixer = radioAudioSource.outputAudioMixerGroup.audioMixer;
        bandPassFilter = radio.GetComponent<BandPassFilter>();

        if (chimes != null)
        {
            chimeMixer = chimes.GetComponent<ChimeConfiguration>().audioMixerGroup.audioMixer;
        }

        if (sliderSoundOffsetRadius != null)
        {
            sliderSoundOffsetRadius.RegisterValueChangedCallback(evt =>
            {
                sliderSoundOffsetRadius.value = GetNearestRadiusStep(evt.newValue);
                radius = steps[Convert.ToInt32(sliderSoundOffsetRadius.value)];
                SetSphericalCoordinates(radius, inclination, azimuth);
            });
        }

        // Force limits and init starting values
        sliderRadioVolume.lowValue = 0;
        sliderRadioVolume.highValue = 100;

        // Get a reference to the spectator camera
        spectator = GameObject.Find("Spectator Camera").GetComponent<SpectatorCamera>();
        if (spectator == null)
        {
            Debug.LogError("Failed to find the spectator camera!");
        }


        if (SceneManager.GetActiveScene().name == "BlankWorld")
        {
            // // Register locationing mode assets
            // RadioSpawnpoint.position = new Vector3(0.51f, 0.766f, 1.744f);
            // RadioSpawnpoint.rotation = Quaternion.Euler(0.0f, 212.06f, 0.0f);
            locationingChair = GameObject.Find("LocationingChair");
            if (locationingChair == null)
            {
                Debug.LogError("Failed to find the locationing chair!");
            }
            else
            {
                locationingChair.SetActive(false);
            }
        }


        RetrieveSettings();

        if (chimes == null)
        {
            toggleChimeSound.SetEnabled(false);
            sliderChimeVolume.SetEnabled(false);
            fieldChimeVolume.SetEnabled(false);
        }
        if (EnvironmentAudioMixer == null)
        {
            toggleEnvironmentSound.SetEnabled(false);
            sliderEnvironmentVolume.SetEnabled(false);
            fieldEnvironmentVolume.SetEnabled(false);
        }
        if (RatingTablet == null)
        {
            toggleShowRatingTablet.SetEnabled(false);
        }

        if (ghosts == null)
        {
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

        Settings settings = Settings.Instance;


        // Perspective setting
        bool spectatorIsFirstPerson;
        if (settings.Contains("spectatorIsFirstPerson"))
        {
            settings.GetValue("spectatorIsFirstPerson", out spectatorIsFirstPerson);
        }
        else
        {
            spectatorIsFirstPerson = false;
        }
        if (spectatorIsFirstPerson)
        {
            ChangeCameraFirstPerson(null);
        }
        else
        {
            ChangeCameraToFixed(null);
        }


        // Radio settings
        bool radioIsPlaying;
        if (settings.Contains("radioIsPlaying"))
        {
            settings.GetValue("radioIsPlaying", out radioIsPlaying);
        }
        else
        {
            radioIsPlaying = false;
        }
        toggleRadioSound.value = radioIsPlaying;
        ToggleRadioSound(null);

        float radioVolume;
        if (settings.Contains("radioVolume"))
        {
            settings.GetValue("radioVolume", out radioVolume);
        }
        else
        {
            radioVolume = 50.0f;
        }
        sliderRadioVolume.value = radioVolume;
        ChangeRadioVolumeSlider(null);


        // Radio noise filter settings
        bool radioFilterBandpassIsOn;
        if (settings.Contains("radioFilterBandpassIsOn"))
        {
            settings.GetValue("radioFilterBandpassIsOn", out radioFilterBandpassIsOn);
        }
        else
        {
            radioFilterBandpassIsOn = false;
        }
        toggleBandpassEnable.value = radioFilterBandpassIsOn;
        ToggleBandpass(null);

        float radioFilterBandpassCenterFrequency;
        if (settings.Contains("radioFilterBandpassCenterFrequency"))
        {
            settings.GetValue("radioFilterBandpassCenterFrequency", out radioFilterBandpassCenterFrequency);
        }
        else
        {
            radioFilterBandpassCenterFrequency = 87.8136f;
        }
        sliderBandpassFreq.value = radioFilterBandpassCenterFrequency;
        ChangeBandpassFreqSlider(null);

        float radioFilterBandpassQFactor;
        if (settings.Contains("radioFilterBandpassQFactor"))
        {
            settings.GetValue("radioFilterBandpassQFactor", out radioFilterBandpassQFactor);
        }
        else
        {
            radioFilterBandpassQFactor = 1.0f;
        }
        sliderBandpassQ.value = radioFilterBandpassQFactor;
        ChangeBandpassQSlider(null);

        float radioFilterParamEQCenterFrequency;
        if (settings.Contains("radioFilterParamEQCenterFrequency"))
        {
            settings.GetValue("radioFilterParamEQCenterFrequency", out radioFilterParamEQCenterFrequency);
        }
        else
        {
            radioFilterParamEQCenterFrequency = 87.8136f;
        }
        sliderParamEQFreq.value = radioFilterParamEQCenterFrequency;
        ChangeParamEQFreqSlider(null);

        float radioFilterParamEQOctaveRange;
        if (settings.Contains("radioFilterParamEQOctaveRange"))
        {
            settings.GetValue("radioFilterParamEQOctaveRange", out radioFilterParamEQOctaveRange);
        }
        else
        {
            radioFilterParamEQOctaveRange = 1.0f;
        }
        sliderParamEQRange.value = radioFilterParamEQOctaveRange;
        ChangeParamEQRangeSlider(null);

        float radioFilterParamEQFrequencyGain;
        if (settings.Contains("radioFilterParamEQFrequencyGain"))
        {
            settings.GetValue("radioFilterParamEQFrequencyGain", out radioFilterParamEQFrequencyGain);
        }
        else
        {
            radioFilterParamEQFrequencyGain = 2.0f;
        }
        sliderParamEQGain.value = radioFilterParamEQFrequencyGain;
        ChangeParamEQGainSlider(null);

        bool radioFilterParamEQIsOn;
        if (settings.Contains("radioFilterParamEQIsOn"))
        {
            settings.GetValue("radioFilterParamEQIsOn", out radioFilterParamEQIsOn);
        }
        else
        {
            radioFilterParamEQIsOn = false;
        }
        toggleParamEQEnable.value = radioFilterParamEQIsOn;
        ToggleParamEQ(null);


        // Chimes settings
        bool chimesIsPlaying;
        if (settings.Contains("chimesIsPlaying"))
        {
            settings.GetValue("chimesIsPlaying", out chimesIsPlaying);
        }
        else
        {
            chimesIsPlaying = false;
        }
        toggleChimeSound.value = chimesIsPlaying;
        if (chimes != null) ToggleChimeSound(null);

        float chimesVolume;
        if (settings.Contains("chimesVolume"))
        {
            settings.GetValue("chimesVolume", out chimesVolume);
        }
        else
        {
            chimesVolume = 50.0f;
        }
        sliderChimeVolume.value = chimesVolume;
        if (chimes != null) ChangeChimeVolumeSlider(null);


        // Environment settings
        bool environmentIsPlaying;
        if (settings.Contains("environmentIsPlaying"))
        {
            settings.GetValue("environmentIsPlaying", out environmentIsPlaying);
        }
        else
        {
            environmentIsPlaying = false;
            settings.SetValue("environmentIsPlaying", false);
        }
        toggleEnvironmentSound.value = environmentIsPlaying;
        if (EnvironmentAudioMixer != null) ToggleEnvironmentSound(null);

        float environmentVolume;
        if (settings.Contains("environmentVolume"))
        {
            settings.GetValue("environmentVolume", out environmentVolume);
        }
        else
        {
            environmentVolume = 50.0f;
        }
        sliderEnvironmentVolume.value = environmentVolume;
        if (EnvironmentAudioMixer != null) ChangeEnvironmentVolumeSlider(null);


        // Feedback tablet active setting
        bool feedbackTabletIsPresent;
        if (settings.Contains("feedbackTabletIsPresent"))
        {
            settings.GetValue("feedbackTabletIsPresent", out feedbackTabletIsPresent);
        }
        else
        {
            feedbackTabletIsPresent = false;
        }
        toggleShowRatingTablet.value = feedbackTabletIsPresent;
        if (RatingTablet != null) ToggleRatingTablet(null);


        bool ghostsActive;
        if (settings.Contains("ghostsActive"))
        {
            settings.GetValue("ghostsActive", out ghostsActive);
        }
        else
        {
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

    private void ToggleRadioVisible(ChangeEvent<bool> evt)
    {

        // toggle radio visibility - if the radio isn't active, default to visible when it is activated.
        if (toggleRadioVisible.value == true)
        {
            radioMeshRenderer.enabled = true;
        }
        else if (toggleRadioVisible.value == false)
        {
            radioMeshRenderer.enabled = false;
        }
        Settings.Instance.SetValue("radioIsVisible", toggleRadioVisible.value);
    }

    private void ToggleAudioSourceVisible(ChangeEvent<bool> evt)
    {
        // toggle audio source visibility - if the radio isn't active, default to visible when it is activated.
        if (toggleAudioSourceVisible.value == true)
        {
            radioAudioSourceRenderer.enabled = true;
        }
        else if (toggleAudioSourceVisible.value == false)
        {
            radioAudioSourceRenderer.enabled = false;
        }
        Settings.Instance.SetValue("audioSourceIsVisible", toggleAudioSourceVisible.value);
    }

    private void ToggleRadioSound(ChangeEvent<bool> evt)
    {
        // play/stop radio sound. If the radio isn't active, playOnAwake is set.
        if (toggleRadioSound.value == true)
        {
            if (radioAudioSource.isActiveAndEnabled)
            {
                radioAudioSource.Play();
            }
            else
            {
                radioAudioSource.playOnAwake = true;
            }
        }
        else if (toggleRadioSound.value == false)
        {
            if (radioAudioSource.isActiveAndEnabled)
            {
                radioAudioSource.Stop();
            }
            else
            {
                radioAudioSource.playOnAwake = false;
            }
        }
        Settings.Instance.SetValue("radioIsPlaying", toggleRadioSound.value);
    }

    private void ToggleTSNSTablet(ChangeEvent<bool> evt)
    {
        if (toggleShowTSNSTablet.value == true)
        {
            // Create a new rating tablet from prefab
            RatingTabletToggleable = Instantiate(RatingTabletToggleablePrefab, RatingTabletToggleableSpawnpoint.position, RatingTabletToggleableSpawnpoint.rotation);
        }
        else if (toggleShowTSNSTablet.value == false)
        {
            // Destroy the rating tablet
            Destroy(RatingTabletToggleable);
        }
    }

    private void ToggleRatingTablet(ChangeEvent<bool> evt)
    {
        if (toggleShowRatingTablet.value == true)
        {
            RatingTablet.SetActive(true);
        }
        else if (toggleShowRatingTablet.value == false)
        {
            RatingTablet.SetActive(false);
        }
        Settings.Instance.SetValue("feedbackTabletIsPresent", toggleShowRatingTablet.value);
    }

    private void ToggleDoGhosting(ChangeEvent<bool> evt)
    {
        if (toggleDoGhosting.value == true)
        {
            foreach (PositioningAssistGhost ghost in ghosts)
            {
                ghost.enabled = true;
            }
        }
        else if (toggleDoGhosting.value == false)
        {
            foreach (PositioningAssistGhost ghost in ghosts)
            {
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
        if (fieldRadioVolume.value == fieldRadioVolumePreviousValue)
        {
            return;
        }

        // ignore % sign
        string input = fieldRadioVolume.value;
        input = input.TrimEnd('%');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            if (value < sliderRadioVolume.lowValue)
            {
                value = Convert.ToInt32(sliderRadioVolume.lowValue);
            }
            else if (value > sliderRadioVolume.highValue)
            {
                value = Convert.ToInt32(sliderRadioVolume.highValue);
            }

            // pass the value to the slider
            sliderRadioVolume.value = value;

            // append % sign
            input += '%';
            fieldRadioVolumePreviousValue = input;
            fieldRadioVolume.value = input;

        }
        else
        {
            // invalid, restore previous value
            fieldRadioVolume.value = fieldRadioVolumePreviousValue;
        }
    }

    private void ToggleChimeSound(ChangeEvent<bool> evt)
    {
        // Enable/disable the chimes playing by themselves.
        if (toggleChimeSound.value == true)
        {
            // on
            chimes.GetComponent<ChimeRandomRinger>().ForceEnabled(true);
        }
        else if (toggleChimeSound.value == false)
        {
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
        if (fieldChimeVolume.value == fieldChimeVolumePreviousValue)
        {
            return;
        }

        // ignore % sign
        string input = fieldChimeVolume.value;
        input = input.TrimEnd('%');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            if (value < sliderChimeVolume.lowValue)
            {
                value = Convert.ToInt32(sliderChimeVolume.lowValue);
            }
            else if (value > sliderChimeVolume.highValue)
            {
                value = Convert.ToInt32(sliderChimeVolume.highValue);
            }

            // pass the value to the slider
            sliderChimeVolume.value = value;

            // append % sign
            input += '%';
            fieldChimeVolumePreviousValue = input;
            fieldChimeVolume.value = input;

        }
        else
        {
            // invalid, restore previous value
            fieldChimeVolume.value = fieldChimeVolumePreviousValue;
        }
    }

    private void ToggleEnvironmentSound(ChangeEvent<bool> evt)
    {
        // mute all environment sound using the AudioMixer.

        // known issue: environment sounds are not properly muted here when entering a new scene, currently doing it in Update()
        if (toggleEnvironmentSound.value == true)
        {
            // on
            //Debug.Log("Environment sound on");
            EnvironmentAudioMixer.SetFloat("EnvironmentVolume", ToDecibel(sliderEnvironmentVolume.value));
        }
        else if (toggleEnvironmentSound.value == false)
        {
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
        if (fieldEnvironmentVolume.value == fieldEnvironmentVolumePreviousValue)
        {
            return;
        }

        // ignore % sign
        string input = fieldEnvironmentVolume.value;
        input = input.TrimEnd('%');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            if (value < sliderEnvironmentVolume.lowValue)
            {
                value = Convert.ToInt32(sliderEnvironmentVolume.lowValue);
            }
            else if (value > sliderEnvironmentVolume.highValue)
            {
                value = Convert.ToInt32(sliderEnvironmentVolume.highValue);
            }

            // pass the value to the slider
            sliderEnvironmentVolume.value = value;

            // append % sign
            input += '%';
            fieldEnvironmentVolumePreviousValue = input;
            fieldEnvironmentVolume.value = input;

        }
        else
        {
            // invalid, restore previous value
            fieldEnvironmentVolume.value = fieldEnvironmentVolumePreviousValue;
        }
    }

    private void ToggleBandpass(ChangeEvent<bool> evt)
    {
        if (toggleBandpassEnable.value == true)
        {
            bandPassFilter.enabled = true;
        }
        else
        {
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
        if (fieldBandpassFreq.value == fieldBandpassFreqPreviousValue)
        {
            return;
        }

        // ignore Hz (units)
        string input = fieldBandpassFreq.value;
        input = input.TrimEnd('z');
        input = input.TrimEnd('H');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            value = Convert.ToInt32(Mathf.Clamp((float)value, Mathf.Pow(2, sliderBandpassFreq.lowValue / 10), Mathf.Pow(2, sliderBandpassFreq.highValue / 10)));

            // pass the value to the slider
            sliderBandpassFreq.value = 10 * (Mathf.Log10(value) / Mathf.Log10(2));

            // append Hz (units)
            fieldBandpassFreq.value = value.ToString() + "Hz";
            fieldBandpassFreqPreviousValue = fieldBandpassFreq.value;

        }
        else
        {
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
        if (fieldBandpassQ.value == fieldBandpassQPreviousValue)
        {
            return;
        }

        // Check if the input is valid
        string input = fieldBandpassQ.value;
        float value;
        if (float.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            if (value < sliderBandpassQ.lowValue)
            {
                value = sliderBandpassQ.lowValue;
            }
            else if (value > sliderBandpassQ.highValue)
            {
                value = sliderBandpassQ.highValue;
            }

            fieldBandpassQ.value = value.ToString("0.00");
            fieldBandpassQPreviousValue = fieldBandpassQ.value;

            // pass the value to the slider
            sliderBandpassQ.value = value;

        }
        else
        {
            // invalid, restore previous value
            fieldBandpassQ.value = fieldBandpassQPreviousValue;
        }
    }

    private void ToggleParamEQ(ChangeEvent<bool> evt)
    {
        // Disable ParamEQ by setting gain to 1.
        if (toggleParamEQEnable.value == true)
        {
            radioMixer.SetFloat("ParamEQGain", sliderParamEQGain.value);
        }
        else
        {
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
        if (fieldParamEQFreq.value == fieldParamEQFreqPreviousValue)
        {
            return;
        }

        // ignore Hz (units)
        string input = fieldParamEQFreq.value;
        input = input.TrimEnd('z');
        input = input.TrimEnd('H');

        // Check if the input is valid
        int value;
        if (int.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            value = Convert.ToInt32(Mathf.Clamp((float)value, Mathf.Pow(2, sliderParamEQFreq.lowValue / 10), Mathf.Pow(2, sliderParamEQFreq.highValue / 10)));

            // pass the value to the slider
            sliderParamEQFreq.value = 10 * (Mathf.Log10(value) / Mathf.Log10(2));

            // append Hz (units)
            fieldParamEQFreq.value = value.ToString() + "Hz";
            fieldParamEQFreqPreviousValue = fieldParamEQFreq.value;

        }
        else
        {
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
        if (fieldParamEQRange.value == fieldParamEQRangePreviousValue)
        {
            return;
        }

        // Check if the input is valid
        string input = fieldParamEQRange.value;
        float value;
        if (float.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            if (value < sliderParamEQRange.lowValue)
            {
                value = sliderParamEQRange.lowValue;
            }
            else if (value > sliderParamEQRange.highValue)
            {
                value = sliderParamEQRange.highValue;
            }

            fieldParamEQRange.value = value.ToString("0.00");
            fieldParamEQRangePreviousValue = fieldParamEQRange.value;

            // pass the value to the slider
            sliderParamEQRange.value = value;

        }
        else
        {
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
        if (fieldParamEQGain.value == fieldParamEQGainPreviousValue)
        {
            return;
        }

        // Check if the input is valid
        string input = fieldParamEQGain.value;
        float value;
        if (float.TryParse(input, out value))
        {
            // input is valid

            // enforce limits
            if (value < sliderParamEQGain.lowValue)
            {
                value = sliderParamEQGain.lowValue;
            }
            else if (value > sliderParamEQGain.highValue)
            {
                value = sliderParamEQGain.highValue;
            }

            fieldParamEQGain.value = value.ToString("0.00");
            fieldParamEQGainPreviousValue = fieldParamEQGain.value;

            // pass the value to the slider
            sliderParamEQGain.value = value;

        }
        else
        {
            // invalid, restore previous value
            fieldParamEQGain.value = fieldParamEQGainPreviousValue;
        }
    }

    private float ToDecibel(float percentage)
    {
        // Convert a percentage (0 to 100) to decibels as used by AudioMixer (-80 to 20)
        // piecewise interpolation such that 50% corresponds to 0 decibels gain.
        if (percentage < 50)
        {
            return Mathf.Lerp(-80, 0, (-0.026f * percentage * percentage + 3.3f * percentage) / 100);
        }
        else
        {
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
        if (newFileIsSelected)
        {
            if (fieldNewFile.value.Length > 1)
            {
                Settings.Instance.Save(fieldNewFile.value);
                // update the dropdown so it shows the new file
                dropdownExistingFile.choices.Clear();
                dropdownExistingFile.choices = Settings.Instance.GetSaveFilenames();
            }
        }
        else
        {
            if ((dropdownExistingFile.value != null) && (dropdownExistingFile.value.Length > 1))
            {
                Settings.Instance.Save(dropdownExistingFile.value);
            }
        }
    }

    private void LoadSettings(ClickEvent evt)
    {
        // load settings with a name chosen by either the New File text field or Existing File dropdown
        if (newFileIsSelected)
        {
            if (fieldNewFile.value.Length > 1)
            {
                Settings.Instance.Load(fieldNewFile.value);
            }
        }
        else
        {
            if ((dropdownExistingFile.value != null) && (dropdownExistingFile.value.Length > 1))
            {
                Settings.Instance.Load(dropdownExistingFile.value);
            }
        }
    }

    private void TryRestoreHeadlockedObject(ClickEvent evt)
    {
        if (Settings.Instance.Contains("lastHeadlockedObject"))
        {
            string objectName;
            Settings.Instance.GetValue("lastHeadlockedObject", out objectName);
            ParentSetter headlock = GameObject.Find(objectName).GetComponent<ParentSetter>();
            // Ensure the object isn't already headlocked
            if (headlock.parent == null)
            {
                headlock.TryRestoreLastHeadlock();
            }
        }
    }

    private void ActivateLocationingMode(ClickEvent evt)
    {
        if (locationingChair != null)
        {
            locationingChair.SetActive(true);
            // relocate fixed spectator camera to locationing position 
            ChangeCameraToLocationing();
            // collect a 120 degree arc of 5 positions around the locationing chair with a 3m radius
            GenerateLocations();
            // relocate the radio to the middle flag
            radioPolygon.GetComponent<Rigidbody>().useGravity = false;
            changeRadioLocation(2);
            radioPolygon.layer = 2; // ignore raycast

            _dataLoggingManager.setRadioPositionIndex(3);
            _dataLoggingManager.activateLocationingMode();

            // relocate the user to the chair
            // TODO / Just get user to navigate and adjust fixed radio height to headset
        }
    }

    private void DeactivateLocationingMode(ClickEvent evt)
    {

        if (locationingChair != null)
        {
            locationingChair.SetActive(false);
            // relocate fixed spectator camera to locationing position 
            spectator.ToFixedPerspective();
            Settings.Instance.SetValue("spectatorIsFirstPerson", false);

            radioPolygon.GetComponent<Rigidbody>().useGravity = true;
            // respawn the radio back to the spawnpoint
            radioPolygon.transform.position = RadioSpawnpoint.position;
            radioPolygon.transform.rotation = RadioSpawnpoint.rotation;
            radioPolygon.layer = 8; // right-hand grab only (default layer)

            _dataLoggingManager.setRadioPositionIndex(-1);
            _dataLoggingManager.deactivateLocationingMode();
            // TODO - relocate user??
        }
    }


    private void changeRadioLocation(int locationIndex)
    {
        radioPolygon.transform.position = locationingPositions[locationIndex];
        radio.transform.position = locationingPositions[locationIndex];
        // rotate the radio to face the user's intended position
        radioPolygon.transform.LookAt(locationingChair.transform.position);
        radio.transform.LookAt(locationingChair.transform.position);
        // set the x rotation to 0
        radioPolygon.transform.rotation = Quaternion.Euler(0, radioPolygon.transform.rotation.eulerAngles.y, radioPolygon.transform.rotation.eulerAngles.z);
        radio.transform.rotation = Quaternion.Euler(0, radio.transform.rotation.eulerAngles.y, radio.transform.rotation.eulerAngles.z);
    }

    private void ChangeCameraToLocationing()
    {
        spectator.ToLocationingPerspective();
        Settings.Instance.SetValue("spectatorIsFirstPerson", false);
    }

    private void GenerateLocations()
    {
        // collect a 150 degree arc of 5 positions around the locationing chair with a 3m radius
        float radius = 5.0f;
        int numberOfLocations = 5;
        // angle between each position = 120 / (n - 1)
        float angleStep = 150.0f / (numberOfLocations - 1);

        locationingPositions = new Vector3[numberOfLocations];

        for (int i = 0; i < numberOfLocations; i++)
        {
            float angle = -165.0f + (i * angleStep);
            float radian = angle * Mathf.Deg2Rad;
            Vector3 locationingPosition = new Vector3(
                locationingChair.transform.position.x + radius * Mathf.Cos(radian),
                1.0f, // fixed height
                locationingChair.transform.position.z + radius * Mathf.Sin(radian)
            );
            locationingPositions[i] = locationingPosition;
        }
    }

    // Tracks the radius slider steps
    private float GetNearestRadiusStep(float value)
    {
        float nearestStep = steps[0];
        float minDifference = Mathf.Abs(value - steps[0]);

        foreach (var step in steps)
        {
            float difference = Mathf.Abs(value - step);
            if (difference < minDifference)
            {
                minDifference = difference;
                nearestStep = step;
            }
        }

        return nearestStep;
    }


    private void SetInclination(float newInclination)
    {
        inclination = newInclination;
        SetSphericalCoordinates(radius, inclination, azimuth);
    }

    private void SetAzimuth(float newAzimuth)
    {
        azimuth = newAzimuth;
        SetSphericalCoordinates(radius, inclination, azimuth);
    }

    public void SetSphericalCoordinates(float radius, float inclination, float azimuth)
    {
        if (audioSourceManager != null)
        {

            audioSourceManager.SetSphericalCoordinates(radius, inclination, azimuth);

            _dataLoggingManager.setAudioSourceOffset(radius, inclination, azimuth);
        }
    }

    public void SetOffsetPreset(ChangeEvent<string> evt)
    {
        string input = evt.newValue;

        // Check if the input is valid
        int index;
        if (int.TryParse(input, out index))
        {
            Debug.Log("Offset preset index: " + index);
            // Input is valid, enforce limits if necessary
            // Assuming you have some predefined limits for the index
            int minIndex = 0; // Replace with your actual minimum index
            int maxIndex = 36; // Replace with your actual maximum index

            if (index < minIndex)
            {
                index = minIndex;
            }
            else if (index > maxIndex)
            {
                index = maxIndex;
            }

            // Call the setOffsetPreset method on the audioSourceManager and _dataLoggingManager
            if (audioSourceManager != null)
            {
                if (index <= 18)
                {
                    toggleRadioVisible.value = true;
                }
                else
                {
                    toggleRadioVisible.value = false;
                }

                if (((index > 0) && (index <= 6)) || ((index > 18) && (index <= 24)))
                {
                    changeRadioLocation(1);
                }
                else if (((index > 6) && (index <= 12)) || ((index > 24) && (index <= 30)))
                {
                    changeRadioLocation(2);
                }
                else if (((index > 12) && (index <= 18)) || ((index > 30) && (index <= 36)))
                {
                    changeRadioLocation(3);
                }
                audioSourceManager.setOffsetPreset(index);
                _dataLoggingManager.setOffsetPreset(index);
            }
        }
        else
        {
            // Invalid input, handle accordingly
            Debug.LogWarning("Invalid input for offset preset.");
        }
    }

    private void ExitApplication(ClickEvent evt)
    {
        Application.Quit();
    }

    private void Update()
    {
        // cheap fix to ensure evironment sounds are properly muted when loading scene
        if (EnvironmentAudioMixer != null)
        {
            if (toggleEnvironmentSound.value == true)
            {
                EnvironmentAudioMixer.SetFloat("EnvironmentVolume", ToDecibel(sliderEnvironmentVolume.value));
            }
            else
            {
                EnvironmentAudioMixer.SetFloat("EnvironmentVolume", -80.00f);
            }
        }
    }


}