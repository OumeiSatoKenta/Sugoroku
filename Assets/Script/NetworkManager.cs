using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.PunBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ログを全て表示する
        PhotonNetwork.logLevel = PhotonLogLevel.Full;

    	//ロビーに自動で入る
    	PhotonNetwork.autoJoinLobby = true;

    	//ゲームのバージョン設定
    	PhotonNetwork.ConnectUsingSettings ("0.1");
    }

    [SerializeField]
    private Text informationText;
    [SerializeField]
    private GameObject loginUI;
    // 部屋リストを表示するドロップボタン
    [SerializeField]
    private Dropdown roomLists;
    // 部屋の名前　
    [SerializeField]
    private InputField roomName;
    // ログアウトボタン
    [SerializeField]
    private GameObject logoutButton;
    [SerializeField]
    private InputField playerName;

    private GameObject player;

    public override void OnJoinedLobby(){
    	Debug.Log("ロビーに入る");
    	loginUI.SetActive(true);
    }

    public void LoginGame(){
    	// ルームオプションを設定
    	RoomOptions ro = new RoomOptions(){
    		// ルームを見えるようにする
    		IsVisible = true,
    		// 部屋の入室最大人数
    		MaxPlayers = 10
    	};

    	if(roomName.text != ""){
    		//部屋の名前（RoomName）に部屋の名前を入力していればその名前で部屋を作成し入室します。
    		PhotonNetwork.JoinOrCreateRoom(roomName.text, ro, TypedLobby.Default);
    	}
    	else {
    		//部屋の名前（RoomName)に何も入力されていなければドロップダウンリストに他の部屋があれば選択している部屋に入室します。
    		if(roomLists.options.Count != 0){
    			Debug.Log(roomLists.options[roomLists.value].text);
    			PhotonNetwork.JoinRoom(roomLists.options[roomLists.value].text);

    		}
    		//ドロップダウンリストに他の部屋がなければDefaultRoomという名前の部屋を作成し入室します
    		else {
    			PhotonNetwork.JoinOrCreateRoom("DefaultName", ro, TypedLobby.Default);
    		}
    	}

    }

    // 部屋が更新された時の処理
    public override void OnReceivedRoomListUpdate() {
    	Debug.Log("部屋更新");
    	// 部屋情報を取得する
    	RoomInfo[] rooms = PhotonNetwork.GetRoomList();
    	// ドロップダウンリストに追加する文字列用のリストを作成
    	List<string> list = new List<string>();

   		// 部屋情報を部屋リストに表示
   		foreach (RoomInfo room in rooms){
   			if(room.PlayerCount < room.MaxPlayers){
   				list.Add(room.name);
   			}
   		}
   		// ドロップダウンリストをリセット
   		roomLists.ClearOptions();

   		// 部屋が１つでもあればドロップダウンリストに追加
   		if(list.Count != 0){
   			roomLists.AddOptions(list);
   		}

    }

    //部屋に入手した時に呼ばれるメソッド
    public override void OnJoinedRoom(){
    	loginUI.SetActive(false);
    	logoutButton.SetActive(true);
    	Debug.Log("入室");

    	// InputFieldに入力した名前を設定
    	PhotonNetwork.player.NickName = playerName.text;
    
    	//　プレイヤーキャラを登場させる。
    	StartCoroutine("SetPlayer", 0f);
    }

    IEnumerator SetPlayer(float time){
    	yield return new WaitForSeconds(time);
    	player = PhotonNetwork.Instantiate("unitychan2", Vector3.up, Quaternion.identity, 0);

    }

    //部屋の入室に失敗した
    void OnPhotonJoinRoomFailed(){
    	Debug.Log("入室に失敗");

    	//ルームオプションを設定
    	RoomOptions ro = new RoomOptions(){
    		//ルームを見えるようにする
    		IsVisible = false,
    		// 部屋の入室最大人数
    		MaxPlayers = 10
    	};
    	//入室に失敗したらDefaultRoomを作成して入室
    	PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);

    }

    public void LogoutGame(){
    	PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom(){
    	Debug.Log("退室");
    	logoutButton.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        informationText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }
}
