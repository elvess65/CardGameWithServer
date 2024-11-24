# CardGameWithServer
Card Game with a server and Android SDK 

1. Run server instance from 'CardGameWithServer\Executables\Server\Server.exe'
2. Copy Paste URL from the Console Window:
<img width="555" alt="p" src="https://github.com/user-attachments/assets/a1546498-d73e-49b6-8c86-28bfaa36600b">

3. Expose server's local URL to the WEB using NGROK (https://ngrok.com/) and copy endpoint:
![1](https://github.com/user-attachments/assets/8957e45d-bee3-41d6-9758-aa4feeb3d18a)

4. Paste NGROK endpoint into the url field at the start screen of the game:
![2](https://github.com/user-attachments/assets/6e381c26-67c5-440b-9d11-a58112109343)

5. Install an .apk from CardGameWithServer\Executables\AndroidClient\CardGame.apk

6. Press 'Play' button
<img width="217" alt="play" src="https://github.com/user-attachments/assets/27c9b01a-c640-46d6-aecf-282620a07b02">

Note, if You want to use Updater Tool in the Editor please follow the same steps or bypass NGROK step if server is run on the same machine.

Updated Tool:
To open the tool go to Custom Tools/Updater Tool:

<img width="520" alt="Screenshot_1" src="https://github.com/user-attachments/assets/a2e3e487-dd02-443c-9fad-a90c7c75f923">

or press 'Updater Tool' button on the GameManager object uner GameSetup root:

<img width="758" alt="Screenshot_2" src="https://github.com/user-attachments/assets/b83afe24-9d0f-4aba-b86a-4f1f59018619">

If server setup correctly after using the tool RulesAssetWrapper object is updated under 'Assets/Data' folder. 
'Version' field is incremented and 'Asset' reference is replaced by the file downloaded from the server:
<img width="770" alt="Screenshot_3" src="https://github.com/user-attachments/assets/60ac0b4a-ef3f-4afd-8a4d-8d336b0f8427">


If entering play mode new version of rules are displayed on the start screen:

<img width="288" alt="Screenshot_4" src="https://github.com/user-attachments/assets/9fddd422-3e63-4e5a-bfd4-9289a10b3eda">


 
