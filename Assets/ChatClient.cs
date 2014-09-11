using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;

public class ChatClient : MonoBehaviour {
	public static string apiKey = "f65c4a48ca04ec3c584337519785a092fd3e8c2d917742416b94bbc0d8857705";
	public static string secretKey = "8cf72997bb49957072726afa0863c344784fe97e242c59528f23576704ceb7a2";
	public static string roomid = "1124805239";

	public static ChatClient instance;

	public GUIStyle chatClientStyle;
	public GUIStyle labelStyle;

	public string inputText = "";
	public string chatLog = "hello-start";
	public ChatClientListener listener = new ChatClientListener();

	public string username = "dood1";

	void Awake() {
		instance = this;
	}

	void Start() {
		WarpClient.initialize(apiKey, secretKey);
		WarpClient.GetInstance().AddConnectionRequestListener(listener);
		WarpClient.GetInstance().AddChatRequestListener(listener);
		WarpClient.GetInstance().AddRoomRequestListener(listener);
		WarpClient.GetInstance().AddNotificationListener(listener);

		username = "dood" + Random.Range(1,9000);
		WarpClient.GetInstance().Connect(username);
	}

	void OnGUI() {
		GUI.Label(new Rect(0,90,Screen.width,Screen.height-70), chatLog, labelStyle);
		GUI.Label(new Rect(0,0,Screen.width,30), "You are: " + username, labelStyle);

		inputText = GUI.TextField(new Rect(0,30,Screen.width-110,50), inputText, chatClientStyle);
		if ( GUI.Button(new Rect(Screen.width-100,30,100,50), "Send") ){
			WarpClient.GetInstance().SendChat(inputText);
			inputText = "";
		}
	}

	void OnDisable() {
		WarpClient.GetInstance().Disconnect();
	}

	public static void Log(string msg) {
		if (instance != null) {
			instance.chatLog = msg + "\n" + instance.chatLog;
		}
	}


	public class ChatClientListener : ConnectionRequestListener, RoomRequestListener, ChatRequestListener, NotifyListener {
		public void onConnectDone(ConnectEvent eventObj) {
			Log("connecting server result: " + (eventObj.getResult()== 0) + "(" + eventObj.getResult() + ")");

			if (eventObj.getResult() == 0) {
				WarpClient.GetInstance().SubscribeRoom(roomid);
			}
		}
		public void onDisconnectDone(ConnectEvent eventObj) {}
		public void onInitUDPDone(byte resultCode) {}

		public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj){}
		public void onJoinRoomDone(RoomEvent eventObj){
			Log("join room result: " + (eventObj.getResult()== 0) + "(" + eventObj.getResult() + ")");
		}
		public void onLeaveRoomDone(RoomEvent eventObj){}
		public void onLockPropertiesDone(byte result){}
		public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj){}	
		public void onSubscribeRoomDone(RoomEvent eventObj){
			WarpClient.GetInstance().JoinRoom(roomid);
		}
		public void onUnlockPropertiesDone(byte result){}
		public void onUnSubscribeRoomDone(RoomEvent eventObj){}
		public void onUpdatePropertyDone(LiveRoomInfoEvent lifeLiveRoomInfoEvent){}

		public void onSendChatDone (byte result){
			if (result == 0) {
				print("chat sent");
			}
		}
		public void onSendPrivateChatDone(byte result) {}

		public void onChatReceived(ChatEvent eventObj){
			Log(eventObj.getSender() + ":" + eventObj.getMessage());
		}
		public void onGameStarted(string sender, string roomId, string nextTurn){}
		public void onGameStopped(string sender, string roomId){}
		public void onMoveCompleted(MoveEvent moveEvent){}
		public void onPrivateChatReceived(string sender, string message){}
		public void onPrivateUpdateReceived(string sender, byte[] update, bool fromUdp){}
		public void onRoomCreated(RoomData eventObj){}
		public void onRoomDestroyed(RoomData eventObj){}
		public void onUpdatePeersReceived(UpdateEvent eventObj){}
		public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable){}
		public void onUserJoinedLobby(LobbyData eventObj, string username){}
		public void onUserJoinedRoom(RoomData eventObj, string username){}
		public void onUserLeftLobby(LobbyData eventObj, string username){}
		public void onUserLeftRoom(RoomData eventObj, string username){}
		public void onUserPaused(string locid, bool isLobby, string username){}
		public void onUserResumed(string locid, bool isLobby, string username){}
	}
}
