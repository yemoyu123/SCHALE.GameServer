# SCHALE.GameServer

## Running
By default the server is configured to run with SQL Server Express in `appsettings.json`. If you wanna use other edition of SQL Server Express change the `ConnectionStrings` in there.

Alternatively this software can run in docker too (`docker compose up --build`).

## Connecting
- Run the game with this [frida script](https://gist.githubusercontent.com/raphaeIl/c4ca030411186c9417da22d8d7864c4d/raw/00b69c5bacdf79c24972411bd80d785eed3841ce/ba.js)

## Discuss
[Discord Server](https://discord.gg/fbsRYc7bBA)

## How to use (E.g Windows and LDPlayer 9)
1. Download this repo.
```sh
git clone https://github.com/rafi1212122/SCHALE.GameServer.git
```
2. Download .NET SDK, SQL Server Express, and install them.
- .NET SDK<br>
<https://dotnet.microsoft.com/zh-cn/download/dotnet?cid=getdotnetcorecli>
- SQL Server Express<br>
<https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x804&culture=zh-cn&country=cn>
3. Download Visual Studio and Install C# and Database Extensions.<br>
[https://visualstudio.microsoft.com/zh-hans/vs/](https://visualstudio.microsoft.com/vs/)
4. Use Visual Studio to open `SCHALE.GameServer\SCHALE.GameServer.sln` and make it.
5. Download `Excel.zip` and unzip and add the excels to the following path: `SCHALE.GameServer\bin\Debug\net8.0\Resources\excel`.
6. Modify the IP address in `SCHALE.GameServer-master\SCHALE.GameServer\bin\Debug\net8.0\Config.json`.
7. Open `SCHALE.GameServer-master\SCHALE.GameServer\bin\Debug\net8.0\SCHALE.GameServer.exe`.
8. Download python and install frida<br>You may need to add python site packages to PATH if "frida" command is missing.<br>
<https://www.python.org/>
```sh
pip install frida-tools
pip install frida
```
9. Download frida-server(if you use emulator, download x86), find "frida-server-[Latest Version]-android-x86_64.xz" and download & unzip it, before push you may rename the bin file to "frida-server".<br>
<https://github.com/frida/frida/releases>
10. Download adb.<br>
<https://developer.android.google.cn/tools/releases/platform-tools?hl=zh-cn#downloads>
11. Use adb connect emulator and start frida-server(enable root first)
```sh
adb root
adb push frida-server /data/local/tmp/
adb shell "chmod 755 /data/local/tmp/frida-server"
adb shell "/data/local/tmp/frida-server &"
```
12. Start BluearchiveJP and use frida script
    1. download this [frida script](https://gist.githubusercontent.com/raphaeIl/c4ca030411186c9417da22d8d7864c4d/raw/00b69c5bacdf79c24972411bd80d785eed3841ce/ba.js) and modify `SERVER_ADDRESS`
    2. Start BluearchiveJP first and then start frida
    3. `frida -U "ブルアカ" -l ba.js --realm=emulated`
13. Skip Tutorial<br>
If you have never finished any tutorials try this<br>
`INSERT INTO [dbo].[AccountTutorials] ([AccountServerId], [TutorialIds]) VALUES (1, N'[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25]');`<br>
or you have finished the front part of the tutorial use this<br>
`UPDATE [dbo].[AccountTutorials] SET [TutorialIds] = N'[1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25]' WHERE [AccountServerId] = [Your Server ID];`

## Command
Go to club and input `/help` to view command usage

## Troubleshooting

##### YostarRequestFail:INIT_FAILED(Initialization failed)
- change `SERVER_ADDRESS` in `ba.js` to your IPV4 and not `0.0.0.0`

##### アカウント情報の連携中にエラーガ発生乚ま乚た
- restart server or without `Resources\excel`

##### can enter game but black screen
- close server and delete database, then open server.<br>
if it isn't work, you may need to pass the official tutorial first.  

##### failed to load club
- change `Address` in `config.json` to your IPV4

##### command not work
- the excel you are using may have problem, change your excel.
