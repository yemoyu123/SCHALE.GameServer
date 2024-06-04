# Redirect server via Frida

## Install Frida's CLI tools

Make sure [Python](https://python.org/) is installed before you start.

Install Frida's CLI tools via Pypi.

```
pip install frida-tools
```

## Run Frida server on device/emulator

Download Frida server [here](https://github.com/frida/frida/releases/).

Make sure adb is enabled and Android is rooted.

Run:

```
adb push frida-server /data/local/tmp
adb shell
# in adb shell
su
chmod 755 /data/local/tmp/frida-server
/data/local/tmp/frida-server
```

## Hook client with frida

Set your server address in `ba.js`.

Launch the client, then immediately run the following command on host:

```
frida -U "ブルアカ" -l ba.js --realm=emulated
```
