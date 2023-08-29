using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketGameProtocol;
using System.Net.Sockets;
using Assets.Scripts;

public class SignupPanel : MonoBehaviour
{
    public Button signupBtn, backBtn;
    public InputField usernameInput, passwordInput, confirmPasswordInput;
    public static bool signuped = false;
    void Start()
    {
        signupBtn.onClick.AddListener(signupClick);
        backBtn.onClick.AddListener(backClick);
    }

    public void signupClick()
    {
        if (usernameInput.text != "" && passwordInput.text != "" && confirmPasswordInput.text != "")
        {
            if (passwordInput.text == confirmPasswordInput.text)
            {
                MainPack pack = new MainPack();
                LoginPack loginPack = new LoginPack();
                loginPack.Username = usernameInput.text;
                loginPack.Password = passwordInput.text;
                pack.Loginpack = loginPack;
                pack.Actioncode = ActionCode.Signup;
                Client client = new Client();
                Client.Send(pack);
                
            }
            else
            {
                Debug.LogError("两次密码输入不一致");
            }

        }
    }
    public void backClick()
    {
        SceneManager.LoadScene("LoginScene");
    }
    void Update()
    {
        if (signuped)
        {
            Client.Close();
            SceneManager.LoadScene("LoginScene");
        }
    }
}
