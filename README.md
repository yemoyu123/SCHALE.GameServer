# SCHALE.GameServer

> [!TIP]
> For original README please refer to <https://github.com/rafi1212122/SCHALE.GameServer>

## Prerequisites

- Some computer knowledge
- [.NET SDK 8.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)
- [SQL Express](https://www.microsoft.com/zh-tw/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/zh-tw/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)
- [LD Player 9](https://www.ldplayer.tw/)
- [Python](https://www.python.org/)
- [Frida](https://frida.re/)
- [frida-server-16.2.5-android-x86_64](https://github.com/frida/frida/releases)

## Steps

1. Start SQL server
2. Start private game server
3. Start LD Player
4. Start Frida server
5. Start ブルアカ
6. Inject Frida script
7. Enjoy :smile:

### SQL server

Use SSMS to connect with default settings except that you have to check "Trust server certificate".

### Game server

```bash
# in this repo
cd SCHALE.GameServer
dotnet run
```

### Frida server

1. Extract `frida-server-16.2.5-android-x86_64.xz`
to `LDPlayer/frida-server-16.2.5-android-x86_64`.
2. Turn on LD Player
3. Turn on root and adb in the settings of LD Player.
4.

```bash
# in LDPlayer
cd LDPlayer9
./adb.exe push ../frida-server-16.2.5-android-x86_64 /data/local/tmp/frida-server
./adb.exe shell
su
cd /data/local/tmp
chmod 755 frida-server
./frida-server
```

### Inject Frida script

> [!NOTE]  
> Edit line 5 of [ba.js](./ba.js) to your own server IP.

> [!WARNING]  
> Do this fast when you open ブルアカ and see the Yostar logo.

```bash
# in this repo
frida -U "ブルアカ" -l ba.js --realm=emulated
```
