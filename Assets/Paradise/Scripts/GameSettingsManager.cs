﻿using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages settings that could be changed while the game.
/// </summary>
public class GameSettingsManager : MonoBehaviour
{
    // Control buttons names
    public const string MoveForwardButton = "MoveForwardButton";
    public const string MoveBackButton = "MoveBackButton";
    public const string MoveLeftButton = "MoveLeftButton";
    public const string MoveRightButton = "MoveRightButton";
    public const string ClimbButton = "ClimbButton";
    public const string DropButton = "DropButton";
    public const string MoveFasterButton = "MoveFasterButton";
    public const string MoveSlowerButton = "MoveSlowerButton";
    // List of game states
    public enum GameState
    {
        Game,
        Menu,
        Settings,
        Control,
        Key,
        Error
    };
    // Post-processing profiles
    public PostProcessProfile[] Profiles;
    // Post-processing volume
    public PostProcessVolume Volume { get; set; }
    // Post-processing layer
    public PostProcessLayer Layer { get; set; }
    // Terrain
    public Terrain Terrain { get; set; }

    //--- Images ---//

    // Background image
    private Image _backgroundImage;
    // Menu panel
    private Image _menuPanel;
    // Settings panel
    private Image _settingsPanel;
    // Control panel
    private Image _controlPanel;
    // Warning panel
    private Image _warningPanel;
    // Key warning panel
    private Image _keyWarningPanel;
    // Message panel
    private Image _messagePanel;
    // Post-processing panel
    public Image PostProcessPanel { get; set; }
    // Post-processing checkmark (post-processing button!)
    public Image PostProcessCheck { get; set; }

    //--- Increase/Decrease buttons ---//

    // Increase quality button
    public Button IncreaseQualityBtn { get; set; }
    // Decrease quality button
    public Button DecreaseQualityBtn { get; set; }
    // Increase post-processing button
    public Button IncreaseProcessBtn { get; set; }
    // Decrease post-processing button
    public Button DecreaseProcessBtn { get; set; }
    // Increase pedestrians button
    public Button IncreasePedestriansBtn { get; set; }
    // Decrease pedestrians button
    public Button DecreasePedestriansBtn { get; set; }
    // Increase static vehicles button
    public Button IncreaseStaticVehiclesBtn { get; set; }
    // Decrease static vehicles button
    public Button DecreaseStaticVehiclesBtn { get; set; }
    // Increase vehicles button
    public Button IncreaseVehiclesBtn { get; set; }
    // Decrease vehicles button
    public Button DecreaseVehiclesBtn { get; set; }
    // Increase grass button
    public Button IncreaseGrassBtn { get; set; }
    // Decrease grass button
    public Button DecreaseGrassBtn { get; set; }
    // Increase distance button
    public Button IncreaseDistBtn { get; set; }
    // Decrease distance button
    public Button DecreaseDistBtn { get; set; }

    //--- Keys buttons ---//

    public Button MoveForwardBtn { get; set; }
    public Button MoveBackBtn { get; set; }
    public Button MoveLeftBtn { get; set; }
    public Button MoveRightBtn { get; set; }
    public Button ClimbBtn { get; set; }
    public Button DropBtn { get; set; }
    public Button MoveFasterBtn { get; set; }
    public Button MoveSlowerBtn { get; set; }

    //--- Different buttons ---//

    // Return button from control menu
    public Button ControlBackBtn { get; set; }
    // Last clicked button in control menu
    public Button LastClickedBtn { get; set; }

    //--- Keys texts ---//

    internal Text MoveForwardText;
    internal Text MoveBackText;
    internal Text MoveLeftText;
    internal Text MoveRightText;
    internal Text ClimbText;
    internal Text DropText;
    internal Text MoveFasterText;
    internal Text MoveSlowerText;

    //--- Different texts ---//

    // Current quality text
    public Text CurQualityText { get; set; }
    // Current processing text
    public Text CurProcessText { get; set; }
    // Current pedestrians text
    public Text CurPedestriansText { get; set; }
    // Current static vehicles text
    public Text CurStaticVehiclesText { get; set; }
    // Current vehicles text
    public Text CurVehiclesText { get; set; }
    // Current grass text
    public Text CurGrassText { get; set; }
    // Current distance text
    public Text CurDistText { get; set; }
    // Clock text
    public Text ClockText { get; set; }
    // Day text
    public Text CurDayText { get; set; }
    // Time label
    private Text _timeLabel;
    // Audio label
    private Text _audioLabel;

    //--- Different ---//

    // Time slider
    public Slider TimeSld { get; set; }
    // Audio slider
    public Slider AudioSld { get; set; }
    // Menu key
    private KeyCode _menuKey = KeyCode.Escape;
    // Time manager
    public DayAndNightCycle DayAndNightCycle { get; set; }
    // Current post processing profile
    public int CurProfile { get; set; }
    // Current game state
    private GameState _curGameState;
    // Check if post-processing is active
    public bool IsPostProcess { get; set; }
    // Click sound
    private AudioClip _click;
    // All people
    private GameObject[] _allPeople;
    // All mobile vehicles
    private GameObject[] _allMobileVehicles;
    // All static vehicles
    private GameObject[] _allStaticVehicles;
    // All audio sources
    private AudioSource[] _allAudioSources;
    // Click audio source
    private AudioSource _clickSrc;
    // Camera movement script
    private CameraMovement _cameraMovement;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    private void Start()
    {
        CollectGameObjects();
        SetEventListeners();
        SetGameConfiguration();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateClock();
        SwitchCursor();
        SwitchGameMenu();
        GetNewKey();
    }

    // Initializate parameters
    private void Init()
    {
        Terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        Volume = GameObject.Find("Post Process Volume").GetComponent<PostProcessVolume>();
        Layer = Camera.main.GetComponent<PostProcessLayer>();
        // Images
        _backgroundImage = GameObject.Find("BackgroundImage").GetComponent<Image>();
        _menuPanel = GameObject.Find("MenuPanel").GetComponent<Image>();
        _settingsPanel = GameObject.Find("SettingsPanel").GetComponent<Image>();
        _controlPanel = GameObject.Find("ControlPanel").GetComponent<Image>();
        _warningPanel = GameObject.Find("WarningPanel").GetComponent<Image>();
        _messagePanel = GameObject.Find("MessagePanel").GetComponent<Image>();
        _keyWarningPanel = GameObject.Find("KeyWarningPanel").GetComponent<Image>();
        PostProcessPanel = GameObject.Find("PostProcessPanel").GetComponent<Image>();
        PostProcessCheck = GameObject.Find("PostProcessCheckmark").GetComponent<Image>();
        // Increase/Decrease buttons
        IncreaseQualityBtn = GameObject.Find("IncreaseQualityButton").GetComponent<Button>();
        DecreaseQualityBtn = GameObject.Find("DecreaseQualityButton").GetComponent<Button>();
        IncreaseProcessBtn = GameObject.Find("IncreaseProcessButton").GetComponent<Button>();
        DecreaseProcessBtn = GameObject.Find("DecreaseProcessButton").GetComponent<Button>();
        IncreasePedestriansBtn = GameObject.Find("IncreasePedestriansButton").GetComponent<Button>();
        DecreasePedestriansBtn = GameObject.Find("DecreasePedestriansButton").GetComponent<Button>();
        IncreaseStaticVehiclesBtn = GameObject.Find("IncreaseStaticVehiclesButton").GetComponent<Button>();
        DecreaseStaticVehiclesBtn = GameObject.Find("DecreaseStaticVehiclesButton").GetComponent<Button>();
        IncreaseVehiclesBtn = GameObject.Find("IncreaseVehiclesButton").GetComponent<Button>();
        DecreaseVehiclesBtn = GameObject.Find("DecreaseVehiclesButton").GetComponent<Button>();
        IncreaseGrassBtn = GameObject.Find("IncreaseGrassButton").GetComponent<Button>();
        DecreaseGrassBtn = GameObject.Find("DecreaseGrassButton").GetComponent<Button>();
        IncreaseDistBtn = GameObject.Find("IncreaseDistanceButton").GetComponent<Button>();
        DecreaseDistBtn = GameObject.Find("DecreaseDistanceButton").GetComponent<Button>();
        // Keys buttons
        MoveForwardBtn = GameObject.Find(MoveForwardButton).GetComponent<Button>();
        MoveBackBtn = GameObject.Find(MoveBackButton).GetComponent<Button>();
        MoveLeftBtn = GameObject.Find(MoveLeftButton).GetComponent<Button>();
        MoveRightBtn = GameObject.Find(MoveRightButton).GetComponent<Button>();
        ClimbBtn = GameObject.Find(ClimbButton).GetComponent<Button>();
        DropBtn = GameObject.Find(DropButton).GetComponent<Button>();
        MoveFasterBtn = GameObject.Find(MoveFasterButton).GetComponent<Button>();
        MoveSlowerBtn = GameObject.Find(MoveSlowerButton).GetComponent<Button>();
        // Other buttons
        ControlBackBtn = GameObject.Find("ControlBackButton").GetComponent<Button>();
        // Texts
        CurQualityText = GameObject.Find("CurrentQualityText").GetComponent<Text>();
        CurProcessText = GameObject.Find("CurrentProcessText").GetComponent<Text>();
        CurPedestriansText = GameObject.Find("CurrentPedestriansText").GetComponent<Text>();
        CurStaticVehiclesText = GameObject.Find("CurrentStaticVehiclesText").GetComponent<Text>();
        CurVehiclesText = GameObject.Find("CurrentVehiclesText").GetComponent<Text>();
        CurGrassText = GameObject.Find("CurrentGrassText").GetComponent<Text>();
        CurDistText = GameObject.Find("CurrentDistanceText").GetComponent<Text>();
        ClockText = GameObject.Find("ClockText").GetComponent<Text>();
        CurDayText = GameObject.Find("CurDayText").GetComponent<Text>();
        _timeLabel = GameObject.Find("TimeLabel").GetComponent<Text>();
        _audioLabel = GameObject.Find("VolumeLabel").GetComponent<Text>();
        // Keys texts
        MoveForwardText = MoveForwardBtn.GetComponentInChildren<Text>();
        MoveBackText = MoveBackBtn.GetComponentInChildren<Text>();
        MoveLeftText = MoveLeftBtn.GetComponentInChildren<Text>();
        MoveRightText = MoveRightBtn.GetComponentInChildren<Text>();
        ClimbText = ClimbBtn.GetComponentInChildren<Text>();
        DropText = DropBtn.GetComponentInChildren<Text>();
        MoveFasterText = MoveFasterBtn.GetComponentInChildren<Text>();
        MoveSlowerText = MoveSlowerBtn.GetComponentInChildren<Text>();
        // Sliders
        TimeSld = GameObject.Find("TimeSlider").GetComponent<Slider>();
        AudioSld = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        // Day and night cycle
        DayAndNightCycle = GameObject.Find("Day And Night Cycle").GetComponent<DayAndNightCycle>();
        // Hide selected panels
        _backgroundImage.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        _menuPanel.gameObject.SetActive(false);
        _warningPanel.gameObject.SetActive(false);
        _controlPanel.gameObject.SetActive(false);
        _keyWarningPanel.gameObject.SetActive(false);
        _messagePanel.gameObject.SetActive(false);
        // Get camera movement
        _cameraMovement = Camera.main.GetComponent<CameraMovement>();
        // Load click sound
        _click = Resources.Load<AudioClip>("Sounds/Click");
        // Set current state
        _curGameState = GameState.Game;
        // Set start time
        UpdateClock();
    }

    /// <summary>
    /// Sets proper configuration before starting the simulation.
    /// </summary>
    private void SetGameConfiguration()
    {
        // Set reference to this script (needed to apply changes)
        GameSettingsManager settingsManager = this;
        // Find camera script
        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        // Try load data
        SettingsDatabase.LoadResult result = SettingsDatabase.TryLoadConfig(Application.persistentDataPath,
            SettingsDatabase.GameConfigName, SettingsDatabase.ConfigType.Game);
        // There are no configuration file or is damaged
        if (result.Equals(SettingsDatabase.LoadResult.NoFile)
            || result.Equals(SettingsDatabase.LoadResult.Error))
            // Set default parameters from database
            SettingsDatabase.SetDefaultGameSettings();
        // Set parameters from configuration file (default or saved)
        SettingsDatabase.SetGameFromConfig(ref settingsManager, ref cameraMovement);
        // Set labels for day duration
        AdjustDayDuration();
        // Set proper labels and volume for all audio sources
        AdjustAudioVolume();
        // Initialize people and cars
        InitSettings(ref _allPeople, CurPedestriansText.text);
        InitSettings(ref _allStaticVehicles, CurStaticVehiclesText.text);
        InitSettings(ref _allMobileVehicles, CurVehiclesText.text);
        // Display warning about damaged file
        if (result.Equals(SettingsDatabase.LoadResult.Error))
        {
            // Display warning window
            _warningPanel.gameObject.SetActive(true);
            // Pause game
            Time.timeScale = 0f;
            // Change state
            _curGameState = GameState.Error;
            // Search audio sources
            foreach (AudioSource audioSrc in _allAudioSources)
                // Mute sound
                audioSrc.Pause();
        }
    }

    /// <summary>
    /// Sets proper number of existing objects in the scene.
    /// </summary>
    /// <param name="objects">The objects that represent the proper group of the elements.</param>
    /// <param name="label">A label that informs about current quality settings.</param>
    private void InitSettings(ref GameObject[] objects, string label)
    {
        // High settings
        if (label.Equals(SettingsDatabase.High))
            // Break action
            return;
        // Normal settings
        if (label.Equals(SettingsDatabase.Low))
        {
            // Search objects
            for (int cnt = 0; cnt < objects.Length; cnt++)
                // Deactivate every second object
                if ((cnt % 2).Equals(0))
                    // Deactivate this object
                    objects[cnt].SetActive(false);
            // Break action
            return;
        }
        // Low settings
        if (label.Equals(SettingsDatabase.Disabled))
            // Search objects
            for (int cnt = 0; cnt < objects.Length; cnt++)
                // Deactivate every object
                objects[cnt].SetActive(false);
    }

    /// <summary>
    /// Sets the event listeners to the selected sliders.
    /// </summary>
    private void SetEventListeners()
    {
        // Add event listeners
        TimeSld.onValueChanged.AddListener(delegate { AdjustDayDuration(); });
        AudioSld.onValueChanged.AddListener(delegate { AdjustAudioVolume(); });
    }

    /// <summary>
    /// Collects proper game objects (people, cars).
    /// </summary>
    private void CollectGameObjects()
    {
        // Find proper game objects
        _allPeople = GameObject.FindGameObjectsWithTag("Human");
        _allMobileVehicles = GameObject.FindGameObjectsWithTag("Vehicle");
        _allStaticVehicles = GameObject.FindGameObjectsWithTag("StaticVehicle");
        // Find pedestrian lights
        GameObject[] pedestrianLights = GameObject.FindGameObjectsWithTag("PedestrianSound");
        // Prepare temporary list
        List<AudioSource> audioSourcesList = new List<AudioSource>();
        // Search people
        foreach (GameObject person in _allPeople)
            // Add audio source to list
            audioSourcesList.Add(person.GetComponent<AudioSource>());
        // Search vehicles
        foreach (GameObject vehicle in _allMobileVehicles)
            // Add audio source to list
            audioSourcesList.Add(vehicle.GetComponent<AudioSource>());
        // Search pedestrian lights
        foreach (GameObject lights in pedestrianLights)
            // Add audio source to list
            audioSourcesList.Add(lights.GetComponent<AudioSource>());
        // Find wind and add audio source to list
        audioSourcesList.Add(GameObject.Find("Wind").GetComponent<AudioSource>());
        // Get single reference (click sound)
        _clickSrc = GameObject.Find("SoundsSource").GetComponent<AudioSource>();
        // Add click source to list
        audioSourcesList.Add(_clickSrc);
        // Convert list to array
        _allAudioSources = audioSourcesList.ToArray();
    }

    /// <summary>
    /// Shows or hides the mouse cursor.
    /// </summary>
    private void SwitchCursor()
    {
        // It is game
        if (_curGameState.Equals(GameState.Game))
        {
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            // Hide cursor
            Cursor.visible = false;
        }
        // It is part of menu
        else
        {
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            // Show cursor
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Switches the state of the menu in the simulation.
    /// </summary>
    private void SwitchGameMenu()
    {
        // Check if key is pressed
        if (!Input.GetKeyDown(_menuKey))
            // Break action
            return;
        // Switch state
        switch (_curGameState)
        {
            case GameState.Error:
                break;
            case GameState.Game:
                ShowMenu();
                break;
            case GameState.Menu:
                ResumeGame();
                break;
            case GameState.Settings:
                ReturnToGameMenu();
                break;
            case GameState.Control:
                ReturnToGameMenu();
                break;
            case GameState.Key:
                HideKeyMessage();
                break;
        }
    }

    /// <summary>
    /// Plays click sound after press button.
    /// </summary>
    public void PlayClickSound()
    {
        // Play click sound
        _clickSrc.PlayOneShot(_click);
    }

    /// <summary>
    /// Shows the message about selecting key in the control menu.
    /// </summary>
    /// <param name="button">An object that represents a clicked button.</param>
    public void ShowKeyMessage(Button button)
    {
        // Disable other buttons
        MoveForwardBtn.interactable = false;
        MoveBackBtn.interactable = false;
        MoveLeftBtn.interactable = false;
        MoveRightBtn.interactable = false;
        ClimbBtn.interactable = false;
        DropBtn.interactable = false;
        MoveFasterBtn.interactable = false;
        MoveSlowerBtn.interactable = false;
        ControlBackBtn.interactable = false;
        // Display message window
        _messagePanel.gameObject.SetActive(true);
        // Get clicked button
        LastClickedBtn = button;
        // Change state
        _curGameState = GameState.Key;
    }

    /// <summary>
    /// Gets the new key via control panel in the game menu.
    /// </summary>
    public void GetNewKey()
    {
        // Check current state
        if (!_curGameState.Equals(GameState.Key))
            // Break action
            return;
        // Check if is key
        bool isKey = false;
        // Prepare key code
        KeyCode key;
        // Prepare counter
        int cnt;
        // Search all key codes
        for (cnt = 0; cnt < Enum.GetValues(typeof(KeyCode)).Length; cnt++)
        {
            // It is clicked button
            if (Input.GetKeyDown((KeyCode)cnt))
            {
                // Set that is key
                isKey = true;
                // Break loop
                break;
            }
        }
        // Validate loop
        if (!isKey)
            // break action
            return;
        // Set proper key code
        key = (KeyCode)cnt;
        // Choose proper button
        switch (LastClickedBtn.name)
        {
            case MoveForwardButton:
                SetNewKey(ref _cameraMovement.MoveForward, ref MoveForwardText, key);
                break;
            case MoveBackButton:
                SetNewKey(ref _cameraMovement.MoveBack, ref MoveBackText, key);
                break;
            case MoveLeftButton:
                SetNewKey(ref _cameraMovement.MoveLeft, ref MoveLeftText, key);
                break;
            case MoveRightButton:
                SetNewKey(ref _cameraMovement.MoveRight, ref MoveRightText, key);
                break;
            case ClimbButton:
                SetNewKey(ref _cameraMovement.Climb, ref ClimbText, key);
                break;
            case DropButton:
                SetNewKey(ref _cameraMovement.Drop, ref DropText, key);
                break;
            case MoveFasterButton:
                SetNewKey(ref _cameraMovement.MoveFaster, ref MoveFasterText, key);
                break;
            case MoveSlowerButton:
                SetNewKey(ref _cameraMovement.MoveSlower, ref MoveSlowerText, key);
                break;
        }
    }

    /// <summary>
    /// Sets the new key obtained from the keyboard.
    /// </summary>
    /// <param name="oldKeyCode">A value that represents the old key code.</param>
    /// <param name="text">A label that represents a description of the key code.</param>
    /// <param name="newKeyCode">A value that represents the new key code.</param>
    private void SetNewKey(ref KeyCode oldKeyCode, ref Text text, KeyCode newKeyCode)
    {
        // Validate keys
        bool isValid01 = IsKeyValid(_cameraMovement.MoveForward, newKeyCode);
        bool isValid02 = IsKeyValid(_cameraMovement.MoveBack, newKeyCode);
        bool isValid03 = IsKeyValid(_cameraMovement.MoveLeft, newKeyCode);
        bool isValid04 = IsKeyValid(_cameraMovement.MoveRight, newKeyCode);
        bool isValid05 = IsKeyValid(_cameraMovement.Climb, newKeyCode);
        bool isValid06 = IsKeyValid(_cameraMovement.Drop, newKeyCode);
        bool isValid07 = IsKeyValid(_cameraMovement.MoveFaster, newKeyCode);
        bool isValid08 = IsKeyValid(_cameraMovement.MoveSlower, newKeyCode);
        // Check result
        if (isValid01 && isValid02 && isValid03 && isValid04 && isValid05
            && isValid06 && isValid07 && isValid08)
        {
            // Set proper label
            text.text = KeyCodeToString(newKeyCode);
            // Set new key
            oldKeyCode = newKeyCode;
            // Hide key message
            HideKeyMessage();
        }
        // Some key is wrong
        else
            ShowKeyWarning();
    }

    /// <summary>
    /// Converts selected key code to the string.
    /// </summary>
    /// <param name="keyCode">A value that represents a key code.</param>
    /// <returns>
    /// The proper string from the key code.
    /// </returns>
    public string KeyCodeToString(KeyCode keyCode)
    {
        // Prepare temporary string
        string str = keyCode.ToString();
        // Prepare string builder
        StringBuilder stringBuilder = new StringBuilder();
        // Set previous character
        char previousChar = Char.MinValue;
        // Search characters
        foreach (char c in str)
        {
            // It i uppercase and it is not first character
            if (char.IsUpper(c) && stringBuilder.Length != 0 && previousChar != ' ')
                // Append space to string builder
                stringBuilder.Append(' ');
            // Append another character
            stringBuilder.Append(c);
            // Set proper previous character
            previousChar = c;
        }
        // Return proper string
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Checks if the selected key is valid.
    /// </summary>
    /// <param name="someKeyCode">A value that represents selected key code.</param>
    /// <param name="newKeyCode">A value that represents a new key code from the keyboard.</param>
    /// <returns>
    /// The boolean that is true if the key code is valid or false if not.
    /// </returns>
    private bool IsKeyValid(KeyCode someKeyCode, KeyCode newKeyCode)
    {
        // Convert key codes to strings
        string someKey = someKeyCode.ToString();
        string newKey = newKeyCode.ToString();
        // Check if it is same key
        if (someKey.Equals(newKey))
        {
            // Check if it is same button
            if (!someKey.Equals(LastClickedBtn.GetComponentInChildren<Text>().text))
                // This key is already used
                return false;
        }
        // This key is valid
        return true;
    }

    /// <summary>
    /// Hides information about selected key code.
    /// </summary>
    public void HideKeyMessage()
    {
        // Enable other buttons
        MoveForwardBtn.interactable = true;
        MoveBackBtn.interactable = true;
        MoveLeftBtn.interactable = true;
        MoveRightBtn.interactable = true;
        ClimbBtn.interactable = true;
        DropBtn.interactable = true;
        MoveFasterBtn.interactable = true;
        MoveSlowerBtn.interactable = true;
        ControlBackBtn.interactable = true;
        // Hide message panel
        _messagePanel.gameObject.SetActive(false);
        _keyWarningPanel.gameObject.SetActive(false);
        // Change state
        _curGameState = GameState.Control;
    }

    /// <summary>
    /// Shows warning about inserted key code.
    /// </summary>
    private void ShowKeyWarning()
    {
        // Show warning
        _keyWarningPanel.gameObject.SetActive(true);
        // Hide key message
        _messagePanel.gameObject.SetActive(false);
        // Disable buttons
        MoveForwardBtn.interactable = false;
        MoveBackBtn.interactable = false;
        MoveLeftBtn.interactable = false;
        MoveRightBtn.interactable = false;
        ClimbBtn.interactable = false;
        DropBtn.interactable = false;
        MoveFasterBtn.interactable = false;
        MoveSlowerBtn.interactable = false;
        ControlBackBtn.interactable = false;
        // Change state
        _curGameState = GameState.Error;
    }

    /// <summary>
    /// Hides warning about damaged game configuration file.
    /// </summary>
    public void HideGameWarning()
    {
        // Hide warning panel
        _warningPanel.gameObject.SetActive(false);
        // Unpause game
        Time.timeScale = 1f;
        // Change state
        _curGameState = GameState.Game;
        // Search audio sources
        foreach (AudioSource audioSrc in _allAudioSources)
            // Unmute sound
            audioSrc.UnPause();
    }

    /// <summary>
    /// Shows the game menu and pauses the simulation.
    /// </summary>
    public void ShowMenu()
    {
        // Lock cursor
        // Activate background
        _backgroundImage.gameObject.SetActive(true);
        // Activate menu panel
        _menuPanel.gameObject.SetActive(true);
        // Search audio sources
        foreach (AudioSource audioSrc in _allAudioSources)
            // Pause sound
            audioSrc.Pause();
        // Stop game
        Time.timeScale = 0f;
        // Change current state
        _curGameState = GameState.Menu;
    }

    /// <summary>
    /// Hides the game menu and resumes the simulation.
    /// </summary>
    public void ResumeGame()
    {
        // Deactivate background
        _backgroundImage.gameObject.SetActive(false);
        // Deactivate menu panel
        _menuPanel.gameObject.SetActive(false);
        // Deactivate settings panel
        _settingsPanel.gameObject.SetActive(false);
        // Search audio sources
        foreach (AudioSource audioSrc in _allAudioSources)
            // Resume sound
            audioSrc.UnPause();
        // resume game
        Time.timeScale = 1f;
        // Change current state
        _curGameState = GameState.Game;
    }

    /// <summary>
    /// Shows the settings window in the game menu.
    /// </summary>
    public void ShowSettings()
    {
        // Deactivate menu panel
        _menuPanel.gameObject.SetActive(false);
        // Activate settings panel
        _settingsPanel.gameObject.SetActive(true);
        // Change current state
        _curGameState = GameState.Settings;
    }

    /// <summary>
    /// Shows the control window in the game menu.
    /// </summary>
    public void ShowControl()
    {
        // Deactivate menu panel
        _menuPanel.gameObject.SetActive(false);
        // Activate control panel
        _controlPanel.gameObject.SetActive(true);
        // Change current state
        _curGameState = GameState.Control;
    }

    /// <summary>
    /// Shows the main window in the game menu and hides the other windows.
    /// </summary>
    public void ReturnToGameMenu()
    {
        // Deactivate other panels
        _settingsPanel.gameObject.SetActive(false);
        _controlPanel.gameObject.SetActive(false);
        // Activate menu panel
        _menuPanel.gameObject.SetActive(true);
        // Change current state
        _curGameState = GameState.Menu;
    }

    /// <summary>
    /// Increases current quality settings of the simulation.
    /// </summary>
    public void IncreaseQualitySettings()
    {
        // Increase index
        QualitySettings.IncreaseLevel(true);
        // Set proper label
        CurQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        // Validate settings
        if (QualitySettings.GetQualityLevel().Equals(QualitySettings.names.Length - 1))
            // Disable increasing button
            IncreaseQualityBtn.interactable = false;
        // Enable decreasing button
        DecreaseQualityBtn.interactable = true;
    }

    /// <summary>
    /// Decreases current quality settings of the simulation.
    /// </summary>
    public void DecreaseQualitySettings()
    {
        // Decrease index
        QualitySettings.DecreaseLevel(true);
        // Set proper label
        CurQualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        // Validate settings
        if (QualitySettings.GetQualityLevel().Equals(0))
            // Disable decreasing button
            DecreaseQualityBtn.interactable = false;
        // Enable increasing button
        IncreaseQualityBtn.interactable = true;
    }

    /// <summary>
    /// Increases current post-processing level.
    /// </summary>
    public void IncreasePostProcessing()
    {
        // Increase current profile
        CurProfile++;
        // Set proper settings
        Volume.profile = Profiles[CurProfile];
        // Set proper label
        CurProcessText.text = Profiles[CurProfile].name;
        // Validate post-processing
        if (CurProfile.Equals(Profiles.Length - 1))
            // Disable increasing button
            IncreaseProcessBtn.interactable = false;
        // Enable decreasing button
        DecreaseProcessBtn.interactable = true;
    }

    /// <summary>
    /// Decreases current post-processing level.
    /// </summary>
    public void DecreasePostProcessing()
    {
        // Decrease current profile
        CurProfile--;
        // Set proper settings
        Volume.profile = Profiles[CurProfile];
        // Set proper label
        CurProcessText.text = Profiles[CurProfile].name;
        // Validate post-processing
        if (CurProfile.Equals(0))
            // Disable decreasing button
            DecreaseProcessBtn.interactable = false;
        // Enable increasing button
        IncreaseProcessBtn.interactable = true;
    }

    /// <summary>
    /// Increases the number of the pedestrian in the simulation.
    /// </summary>
    public void IncreasePedestrians()
    {
        // There are not people
        if (CurPedestriansText.text.Equals(SettingsDatabase.Disabled))
        {
            // Search people
            for (int cnt = 0; cnt < _allPeople.Length; cnt++)
            {
                // Activate every second person
                if (!(cnt % 2).Equals(0))
                    // Check another person
                    continue;
                // Activate this person
                _allPeople[cnt].SetActive(true);
                // Reset person
                _allPeople[cnt].GetComponent<HumanBehavior>().ResetPerson();
            }
            // Set new label
            CurPedestriansText.text = SettingsDatabase.Low;
        }
        // There are some people
        else if (CurPedestriansText.text.Equals(SettingsDatabase.Low))
        {
            // Search people
            for (int cnt = 0; cnt < _allPeople.Length; cnt++)
            {
                // Activate every person
                _allPeople[cnt].SetActive(true);
                // Reset person
                _allPeople[cnt].GetComponent<HumanBehavior>().ResetPerson();
            }
            // Set new label
            CurPedestriansText.text = SettingsDatabase.High;
            // Disable button
            IncreasePedestriansBtn.interactable = false;
        }
        // Enable decreasing button
        DecreasePedestriansBtn.interactable = true;
    }

    /// <summary>
    /// Decreases the number of the pedestrian in the simulation.
    /// </summary>
    public void DecreasePedestrians()
    {
        // There are all people
        if (CurPedestriansText.text.Equals(SettingsDatabase.High))
        {
            // Search people
            for (int cnt = 0; cnt < _allPeople.Length; cnt++)
                // Deactivate every second person
                if ((cnt % 2).Equals(0))
                    // Deactivate this person
                    _allPeople[cnt].SetActive(false);
            // Set new label
            CurPedestriansText.text = SettingsDatabase.Low;
        }
        // There are some people
        else if (CurPedestriansText.text.Equals(SettingsDatabase.Low))
        {
            // Search people
            for (int cnt = 0; cnt < _allPeople.Length; cnt++)
                // Deactivate every person
                _allPeople[cnt].SetActive(false);
            // Set new label
            CurPedestriansText.text = SettingsDatabase.Disabled;
            // Disable button
            DecreasePedestriansBtn.interactable = false;
        }
        // Enable increasing button
        IncreasePedestriansBtn.interactable = true;
    }

    /// <summary>
    /// Increases the number of the static vehicles in the simulation.
    /// </summary>
    public void IncreaseStaticVehicles()
    {
        // There are not static vehicles
        if (CurStaticVehiclesText.text.Equals(SettingsDatabase.Disabled))
        {
            // Search static vehicles
            for (int cnt = 0; cnt < _allStaticVehicles.Length; cnt++)
            {
                // Activate every second static vehicle
                if (!(cnt % 2).Equals(0))
                    // Check another vehicle
                    continue;
                // Activate this static vehicle
                _allStaticVehicles[cnt].SetActive(true);
                // Set wheels
                _allStaticVehicles[cnt].GetComponent<VehicleBehavior>().SetWheels();
            }
            // Set new label
            CurStaticVehiclesText.text = SettingsDatabase.Low;
        }
        // There are some static vehicles
        else if (CurStaticVehiclesText.text.Equals(SettingsDatabase.Low))
        {
            // Search static vehicles
            for (int cnt = 0; cnt < _allStaticVehicles.Length; cnt++)
            {
                // Activate every static vehicles
                _allStaticVehicles[cnt].SetActive(true);
                // Set wheels
                _allStaticVehicles[cnt].GetComponent<VehicleBehavior>().SetWheels();
            }
            // Set new label
            CurStaticVehiclesText.text = SettingsDatabase.High;
            // Disable button
            IncreaseStaticVehiclesBtn.interactable = false;
        }
        // Enable decreasing button
        DecreaseStaticVehiclesBtn.interactable = true;
    }

    /// <summary>
    /// Decreases the number of the static vehicles in the simulation.
    /// </summary>
    public void DecreaseStaticVehicles()
    {
        // There are all static vehicles
        if (CurStaticVehiclesText.text.Equals(SettingsDatabase.High))
        {
            // Search static vehicles
            for (int cnt = 0; cnt < _allStaticVehicles.Length; cnt++)
                // Deactivate every second static vehicles
                if ((cnt % 2).Equals(0))
                    // Deactivate this static vehicle
                    _allStaticVehicles[cnt].SetActive(false);
            // Set new label
            CurStaticVehiclesText.text = SettingsDatabase.Low;
        }
        // There are some static vehicles
        else if (CurStaticVehiclesText.text.Equals(SettingsDatabase.Low))
        {
            // Search static vehicles
            for (int cnt = 0; cnt < _allStaticVehicles.Length; cnt++)
                // Deactivate every static vehicle
                _allStaticVehicles[cnt].SetActive(false);
            // Set new label
            CurStaticVehiclesText.text = SettingsDatabase.Disabled;
            // Disable button
            DecreaseStaticVehiclesBtn.interactable = false;
        }
        // Enable increasing button
        IncreaseStaticVehiclesBtn.interactable = true;
    }

    /// <summary>
    /// Increases the number of the mobile vehicles in the simulation.
    /// </summary>
    public void IncreaseVehicles()
    {
        // There are not vehicles
        if (CurVehiclesText.text.Equals(SettingsDatabase.Disabled))
        {
            // Search vehicles
            for (int cnt = 0; cnt < _allMobileVehicles.Length; cnt++)
            {
                // Activate every second vehicle
                if (!(cnt % 2).Equals(0))
                    // Check another vehicle
                    continue;
                // Activate this vehicle
                _allMobileVehicles[cnt].SetActive(true);
                // Get vehicle behavior
                VehicleBehavior vehicleBehavior = _allMobileVehicles[cnt].GetComponent<VehicleBehavior>();
                // Reset vehicle
                vehicleBehavior.ResetVehicle();
            }
            // Set new label
            CurVehiclesText.text = SettingsDatabase.Low;
        }
        // There are some vehicles
        else if (CurVehiclesText.text.Equals(SettingsDatabase.Low))
        {
            // Search vehicles
            for (int cnt = 0; cnt < _allMobileVehicles.Length; cnt++)
            {
                // Activate every vehicles
                _allMobileVehicles[cnt].SetActive(true);
                // Get vehicle behavior
                VehicleBehavior vehicleBehavior = _allMobileVehicles[cnt].GetComponent<VehicleBehavior>();
                // Reset vehicle
                vehicleBehavior.ResetVehicle();
            }
            // Set new label
            CurVehiclesText.text = SettingsDatabase.High;
            // Disable button
            IncreaseVehiclesBtn.interactable = false;
        }
        // Enable decreasing button
        DecreaseVehiclesBtn.interactable = true;
    }

    /// <summary>
    /// Decreases the number of the mobile vehicles in the simulation.
    /// </summary>
    public void DecreaseVehicles()
    {
        // There are all vehicles
        if (CurVehiclesText.text.Equals(SettingsDatabase.High))
        {
            // Search vehicles
            for (int cnt = 0; cnt < _allMobileVehicles.Length; cnt++)
                // Deactivate every second vehicles
                if ((cnt % 2).Equals(0))
                    // Deactivate this vehicle
                    _allMobileVehicles[cnt].SetActive(false);
            // Set new label
            CurVehiclesText.text = SettingsDatabase.Low;
        }
        // There are some vehicles
        else if (CurVehiclesText.text.Equals(SettingsDatabase.Low))
        {
            // Search vehicles
            for (int cnt = 0; cnt < _allMobileVehicles.Length; cnt++)
                // Deactivate every vehicle
                _allMobileVehicles[cnt].SetActive(false);
            // Set new label
            CurVehiclesText.text = SettingsDatabase.Disabled;
            // Disable button
            DecreaseVehiclesBtn.interactable = false;
        }
        // Enable increasing button
        IncreaseVehiclesBtn.interactable = true;
    }

    /// <summary>
    /// Increases the grass density in the simulation.
    /// </summary>
    public void IncreaseGrassDensity()
    {
        // Grass is disabled
        if (CurGrassText.text.Equals(SettingsDatabase.Disabled))
        {
            // Improve density
            Terrain.detailObjectDensity = SettingsDatabase.PoorGrass;
            // Set new label
            CurGrassText.text = SettingsDatabase.Low;
        }
        // Grass is poor
        else if (CurGrassText.text.Equals(SettingsDatabase.Low))
        {
            // Improve density
            Terrain.detailObjectDensity = SettingsDatabase.GreatGrass;
            // Set new label
            CurGrassText.text = SettingsDatabase.High;
            // Disable button
            IncreaseGrassBtn.interactable = false;
        }
        // Refresh terrain
        Terrain.Flush();
        // Enable decreasing button
        DecreaseGrassBtn.interactable = true;
    }

    /// <summary>
    /// Decreases the grass density in the simulation.
    /// </summary>
    public void DecreaseGrassDensity()
    {
        // Grass is great
        if (CurGrassText.text.Equals(SettingsDatabase.High))
        {
            // Lower density
            Terrain.detailObjectDensity = SettingsDatabase.PoorGrass;
            // Set new label
            CurGrassText.text = SettingsDatabase.Low;
        }
        // Grass is poor
        else if (CurGrassText.text.Equals(SettingsDatabase.Low))
        {
            // Lower density
            Terrain.detailObjectDensity = SettingsDatabase.DisabledGrass;
            // Set new label
            CurGrassText.text = SettingsDatabase.Disabled;
            // Disable button
            DecreaseGrassBtn.interactable = false;
        }
        // Refresh terrain
        Terrain.Flush();
        // Enable increasing button
        IncreaseGrassBtn.interactable = true;
    }

    /// <summary>
    /// Increases the drawing distance in the simulation.
    /// </summary>
    public void IncreaseDrawingDistance()
    {
        // Drawing distance is low
        if (CurDistText.text.Equals(SettingsDatabase.Low))
        {
            // Improve drawing distance
            Camera.main.farClipPlane = RenderSettings.fogEndDistance = SettingsDatabase.NormalDist;
            // Set new label
            CurDistText.text = SettingsDatabase.Medium;
        }
        // Drawing distance is normal
        else if (CurDistText.text.Equals(SettingsDatabase.Medium))
        {
            // Improve drawing distance
            Camera.main.farClipPlane = RenderSettings.fogEndDistance = SettingsDatabase.FarDist;
            // Set new label
            CurDistText.text = SettingsDatabase.High;
            // Disable button
            IncreaseDistBtn.interactable = false;
        }
        // Enable decreasing button
        DecreaseDistBtn.interactable = true;
    }

    /// <summary>
    /// Decreases the drawing distance in the simulation.
    /// </summary>
    public void DecreaseDrawingDistance()
    {
        // Drawing distance is high
        if (CurDistText.text.Equals(SettingsDatabase.High))
        {
            // Lower drawing distance
            Camera.main.farClipPlane = RenderSettings.fogEndDistance = SettingsDatabase.NormalDist;
            // Set new label
            CurDistText.text = SettingsDatabase.Medium;
        }
        // Drawing distance is normal
        else if (CurDistText.text.Equals(SettingsDatabase.Medium))
        {
            // Lower drawing distance
            Camera.main.farClipPlane = RenderSettings.fogEndDistance = SettingsDatabase.NearDist;
            // Set new label
            CurDistText.text = SettingsDatabase.Low;
            // Disable button
            DecreaseDistBtn.interactable = false;
        }
        // Enable increasing button
        IncreaseDistBtn.interactable = true;
    }

    /// <summary>
    /// Turns on or turns off the post-processing in the simulation.
    /// </summary>
    public void SwitchPostProcessing()
    {
        // Post-processing is on
        if (IsPostProcess)
        {
            // Disable checkmark
            PostProcessCheck.enabled = false;
            // Turn off post-processing
            Layer.enabled = false;
            // Hide post-processing components
            PostProcessPanel.gameObject.SetActive(false);
            // Set that post-processing is inactive
            IsPostProcess = false;
        }
        // Post-processing is off
        else
        {
            // Enable checkmark
            PostProcessCheck.enabled = true;
            // Turn on post-processing
            Layer.enabled = true;
            // Hide post-processing components
            PostProcessPanel.gameObject.SetActive(true);
            // Set that post-processing is active
            IsPostProcess = true;
        }
    }

    /// <summary>
    /// Changes the duration of the day via slider.
    /// </summary>
    public void AdjustDayDuration()
    {
        // Change day duration
        DayAndNightCycle.SecondsInAFullDay = TimeSld.value;
        // Change label
        _timeLabel.text = DayAndNightCycle.SecondsInAFullDay + "s";
    }

    /// <summary>
    /// Changes the audio volume via slider.
    /// </summary>
    public void AdjustAudioVolume()
    {
        // Prepare volume label
        int audioValue = (int)Mathf.Round(AudioSld.value * 100f);
        // Set proper label
        _audioLabel.text = audioValue + "%";
        // Search audio sources
        foreach (AudioSource audioSource in _allAudioSources)
            // Change audio value
            audioSource.volume = AudioSld.value;
    }

    /// <summary>
    /// Updates time of a day displayed on the blackboard.
    /// </summary>
    private void UpdateClock()
    {
        // Prepare labels
        string h, m = "";
        // Calculate minutes
        int minutes = (int)((DayAndNightCycle.CurrentTime * 1440f) % 60f);
        // Calculate hours
        int hours = (int)(DayAndNightCycle.CurrentTime * 1440f / 60f);
        // Validate hours
        if (hours < 10)
            // Set proper label
            h = "0" + hours;
        else
            h = hours.ToString();
        // Validate minutes
        if (minutes < 10)
            // Set proper label
            m = "0" + minutes;
        else
            m = minutes.ToString();
        // Set new time
        ClockText.text = h + ":" + m;
        // Update day
        CurDayText.text = DayAndNightCycle.CurrentDay.ToString();
    }

    /// <summary>
    /// Quits the simulation and displays the main menu.
    /// </summary>
    public void ExitSimulation()
    {
        // Find camera script
        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        // Copy variables to configuration structure
        SettingsDatabase.CopyGameToConfig(this, cameraMovement);
        // Save configuration
        SettingsDatabase.TrySaveConfig(Application.persistentDataPath,
            SettingsDatabase.GameConfigName, SettingsDatabase.ConfigType.Game);
        // Resume game time
        Time.timeScale = 1f;
        // Display main menu scene
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}