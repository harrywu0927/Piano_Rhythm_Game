using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using System.Net.Sockets;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;
public class LoginPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public Button loginBtn, signupBtn;
    public InputField username, password;
    public static bool isLogin = false;
    public static Client client;
    
    void Start()
    {
        Screen.SetResolution(1005, 459, false);
        loginBtn.onClick.AddListener(loginClick);
        signupBtn.onClick.AddListener(gotoSignupClick);
        username.text = "admin";
        password.text = "123";
    }
    private void loginClick()
    {
        MainPack pack = new MainPack();
        pack.Actioncode = ActionCode.Login;
        pack.Requestcode = RequestCode.UserControl;
        LoginPack loginPack = new LoginPack();
        loginPack.Username = username.text;
        loginPack.Password = password.text;
        pack.Loginpack = loginPack;
        client = new Client();
        
        Client.Send(pack);

        //socket.Send(Message.PackData(pack));

    }
    private void gotoSignupClick()
    {
        SceneManager.LoadScene("SignupScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (isLogin == true)
        {
            //Client.Close();
            SceneController.Push(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("MainMenu");
            
        }
    }
}
