using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// GUID ��� Custom ID �α��� ��,
/// �г��� �Է� �� �α��� ���� �� ��� �޽����� ǥ���ϴ� ��ũ��Ʈ
/// </summary>
public class PlayFabGuidLoginWithNickname : MonoBehaviour
{
    private const string PREFS_CUSTOM_ID = "CUSTOM_PLAYFAB_ID";

    [Header("Nickname UI")]
    [Tooltip("�г��� �Է� UI Prefab (InputField + Button)")]
    public GameObject nicknamePanelPrefab;
    [Tooltip("UI�� ���� �θ� Canvas �Ǵ� UI Parent")]
    public Transform uiParent;

    [Header("Login Error UI")]
    [Tooltip("�α��� ���� �� ǥ���� ��� �޽��� Prefab")]
    public GameObject loginErrorPanelPrefab;

    [Header("Debug")]
    [Tooltip("���� DisplayName�� �־ UI�� ����� ����")]
    public bool forceShowNicknameUI = false;

    private GameObject nicknamePanelInstance;
    private GameObject loginErrorPanelInstance;
    private TMP_InputField nicknameInput;
    private Button submitButton;

    private void Start()
    {
        // Custom ID �б� �Ǵ� ����
        string customId = PlayerPrefs.GetString(PREFS_CUSTOM_ID, null);
        if (string.IsNullOrEmpty(customId))
        {
            customId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString(PREFS_CUSTOM_ID, customId);
            PlayerPrefs.Save();
        }

        // PlayFab �α��� ��û
        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"PlayFab �α��� ����: {result.PlayFabId}");
        // ���� ���� ��ȸ
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnAccountInfoReceived, OnLoginError);
    }

    private void OnAccountInfoReceived(GetAccountInfoResult infoResult)
    {
        string currentName = infoResult.AccountInfo.TitleInfo.DisplayName;
        if (string.IsNullOrEmpty(currentName) || forceShowNicknameUI)
            ShowNicknameUI();
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.LogError("PlayFab ����: " + error.GenerateErrorReport());
        ShowLoginError();
    }

    private void ShowNicknameUI()
    {
        if (nicknamePanelInstance != null) return;
        if (nicknamePanelPrefab == null) return;
        nicknamePanelInstance = uiParent != null
            ? Instantiate(nicknamePanelPrefab, uiParent, false)
            : Instantiate(nicknamePanelPrefab);
        nicknameInput = nicknamePanelInstance.GetComponentInChildren<TMP_InputField>();
        submitButton = nicknamePanelInstance.GetComponentInChildren<Button>();
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitNickname);
    }

    /// <summary>
    /// �α��� ���� �� ȣ��: ��� �޽��� �г��� ǥ��
    /// </summary>
    private void ShowLoginError()
    {
        if (loginErrorPanelInstance != null) return;
        if (loginErrorPanelPrefab == null) return;
        loginErrorPanelInstance = uiParent != null
            ? Instantiate(loginErrorPanelPrefab, uiParent, false)
            : Instantiate(loginErrorPanelPrefab);
    }

    private void OnSubmitNickname()
    {
        string name = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(name)) return;
        var setNameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = name };
        PlayFabClientAPI.UpdateUserTitleDisplayName(setNameRequest,
            _ => { if (nicknamePanelInstance != null) Destroy(nicknamePanelInstance); },
            err => Debug.LogError("�г��� ���� ����: " + err.GenerateErrorReport()));
    }
}
