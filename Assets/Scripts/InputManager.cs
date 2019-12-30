using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region INPUT MANAGER PROPERTY

    private static InputManager _IM;

    public static InputManager IM
    {
        get
        {
            if (_IM == null)
                _IM = FindObjectOfType<InputManager>();
            return _IM;
        }
    }

    #endregion

    public CustomEvent<InputType> ButtonInputEvent = new CustomEvent<InputType>();
    public CustomEvent<Vector2> ControllerInputEvent = new CustomEvent<Vector2>();

    public TextAsset JoystickData;

    public TextAsset JoystickControls;
    public TextAsset KeyboardControls;

    private bool _joystickSetUp;
    private bool _keyboardSetUp;

    private Dictionary<int, string> _joystickButtons = new Dictionary<int, string>();

    private Dictionary<InputType, int> _joystickControls = new Dictionary<InputType, int>();
    private Dictionary<InputType, KeyCode> _keyboardControls = new Dictionary<InputType, KeyCode>();

    private Vector2 _lastControllerInput;

    private bool _lastPressedShieldJoystick;
    private bool _lastPressedShootJoystick;

    private bool _lastPressedShieldKeyboard;
    private bool _lastPressedShootKeyboard;

    private void Awake()
    {

        #region INPUT MANAGER PROPERTY SET-UP

        if (_IM == null)
            _IM = this;

        if (_IM.Equals(this) == false)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        #endregion

        JoystickControlsSetUp();
        KeyboardControlsSetUp();

        JoystickSetUp();

    }

    private void Update()
    {

        if (Input.anyKeyDown && GameManager.GM && GameManager.GM.SceneIndex == 0)
            ButtonInputEvent.Invoke(InputType.ANY);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        CheckJoystickInput();
        CheckKeyboardInput();

    }

    private void CheckJoystickInput()
    {

        if (_joystickSetUp == false)
            return;

        if (Input.GetKeyDown(_joystickButtons[_joystickControls[InputType.PAUSE]]))
            ButtonInputEvent.Invoke(InputType.PAUSE);

        if (Input.GetKeyDown(_joystickButtons[_joystickControls[InputType.SELECT]]))
            ButtonInputEvent.Invoke(InputType.SELECT);

        bool currentPressedShield = Input.GetKey(_joystickButtons[_joystickControls[InputType.SHIELD]]);
        bool currentPressedShoot = Input.GetKey(_joystickButtons[_joystickControls[InputType.SHOOT]]);

        if (_lastPressedShieldJoystick != currentPressedShield)
        {

            _lastPressedShieldJoystick = currentPressedShield;

            if (currentPressedShield)
                ButtonInputEvent.Invoke(InputType.SHIELD);
            else
                ButtonInputEvent.Invoke(InputType.RELEASED_SHIELD);

        }

        if (_lastPressedShootJoystick != currentPressedShoot)
        {

            _lastPressedShootJoystick = currentPressedShoot;

            if (currentPressedShoot)
                ButtonInputEvent.Invoke(InputType.SHOOT);
            else
                ButtonInputEvent.Invoke(InputType.RELEASED_SHOOT);

        }

        Vector2 currentControllerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_lastControllerInput.Equals(currentControllerInput) == false)
        {
            ControllerInputEvent.Invoke(currentControllerInput);
            _lastControllerInput = currentControllerInput;
        }

    }

    private void CheckKeyboardInput()
    {

        if (_joystickSetUp)
            return;

        if (_keyboardSetUp == false)
            return;

        if (Input.GetKeyDown(_keyboardControls[InputType.PAUSE]))
            ButtonInputEvent.Invoke(InputType.PAUSE);

        if (Input.GetKeyDown(_keyboardControls[InputType.SELECT]))
            ButtonInputEvent.Invoke(InputType.SELECT);

        bool currentPressedShield = Input.GetKey(_keyboardControls[InputType.SHIELD]);
        bool currentPressedShoot = Input.GetKey(_keyboardControls[InputType.SHOOT]);

        if (_lastPressedShieldKeyboard != currentPressedShield)
        {

            _lastPressedShieldKeyboard = currentPressedShield;

            if (currentPressedShield)
                ButtonInputEvent.Invoke(InputType.SHIELD);
            else
                ButtonInputEvent.Invoke(InputType.RELEASED_SHIELD);

        }

        if (_lastPressedShootKeyboard != currentPressedShoot)
        {

            _lastPressedShootKeyboard = currentPressedShoot;

            if (currentPressedShoot)
                ButtonInputEvent.Invoke(InputType.SHOOT);
            else
                ButtonInputEvent.Invoke(InputType.RELEASED_SHOOT);

        }

        Vector2 currentControllerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_lastControllerInput.Equals(currentControllerInput) == false)
        {
            ControllerInputEvent.Invoke(currentControllerInput);
            _lastControllerInput = currentControllerInput;
        }

    }

    #region JOYSTICK INPUT SET-UP

    private void JoystickSetUp()
    {

        if (_joystickSetUp == false)
            return;

        if (JoystickData == null)
        {
            _joystickSetUp = false;
            return;
        }

        string[] data = JoystickData.text.Split('\n');
        string[] joysticks = Input.GetJoystickNames();

        string buttonNamePrefab = null;

        for (int index = 0; index < joysticks.Length; index++)
        {
            if (joysticks[index].Trim().Equals(data[0].Trim()))
            {
                buttonNamePrefab = "joystick " + (index + 1) + " button ";
                break;
            }
        }

        if (buttonNamePrefab == null)
        {
            _joystickSetUp = false;
            return;
        }

        for (int index = 2; index < data.Length; index++)
        {

            try
            {

                int key = System.Int32.Parse(data[index].Trim().Split('\t')[0]);
                int value = System.Int32.Parse(data[index].Trim().Split('\t')[1]);

                _joystickButtons.Add(key, buttonNamePrefab + (value - 1));

            }

            catch
            {
                _joystickSetUp = false;
                return;
            }

        }

        if (_joystickButtons.Count.ToString().Equals(data[1].Trim()) == false)
        {
            _joystickSetUp = false;
            return;
        }

        foreach (InputType input in _joystickControls.Keys)
        {
            if (_joystickButtons.ContainsKey(_joystickControls[input]) == false || _joystickButtons[_joystickControls[input]].EndsWith("-1"))
            {
                _joystickSetUp = false;
                return;
            }
        }

    }

    private void JoystickControlsSetUp()
    {

        if (JoystickControls == null)
            return;

        string[] data = JoystickControls.text.Split('\n');

        for (int index = 0; index < data.Length; index++)
        {

            try
            {
                InputType key = (InputType)System.Enum.Parse(typeof(InputType), data[index].Trim().Split('\t')[0]);
                int value = System.Int32.Parse(data[index].Trim().Split('\t')[1]);
                _joystickControls.Add(key, value);
            }

            catch
            {
                _joystickSetUp = false;
                return;
            }

        }

        _joystickSetUp = true;

    }

    #endregion

    #region KEYBOARD INPUT SET-UP

    private void KeyboardControlsSetUp()
    {

        if (KeyboardControls == null)
            return;

        string[] data = KeyboardControls.text.Split('\n');

        for (int index = 0; index < data.Length; index++)
        {

            try
            {
                InputType key = (InputType)System.Enum.Parse(typeof(InputType), data[index].Trim().Split('\t')[0]);
                KeyCode value = (KeyCode)System.Enum.Parse(typeof(KeyCode), data[index].Trim().Split('\t')[1]);
                _keyboardControls.Add(key, value);
            }

            catch
            {
                _keyboardSetUp = false;
                return;
            }

        }

        _keyboardSetUp = true;

    }

    #endregion

}
