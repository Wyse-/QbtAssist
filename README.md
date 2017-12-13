# QbtAssist


## Screenshots


![alt tag](https://i.imgur.com/7KzyNfb.png)

Note: command line window intended for debugging and/or demonstration purposes only, should be hidden with Quiet during normal usage.


## Descritpion
This software interacts with the Qbittorrent WebUI: its purpose is to limit download and upload speeds to values set by the user when a specifig process is running. 

Said process (or processes) can be configured by the user and should obviously be some kind of applications that benefit from having unrestricted bandwidth usage.
For example, if you have a 500kb/s upload cap and Qbittorrent is hogging all available upload bandwidth (i.e. you're constantly seeding at 500kb/s) and try playing online games
your ping is probably going to be way higher than the one you have normally. The same goes for your download bandwidth. 

This is what this software does: it logs into the Qbittorrent WebUI with user supplied info, it then checks if one of the user supplied processes is running. If at least a process 
is running it caps upload and download speeds on Qbittorrent, if not it uncaps them. Everything is done automatically, no user interaction required, except for configuration.

Everything is configured through an XML file stored in the directory where the main executable is located.

**.NET Framework 4.6.1 is required.**


## How-to
- Head over to the [releases](https://github.com/Wyse-/QbtAssist/releases) section and download the latest executable and config.xml file. Just fyi: a x64 compiled executable is provided, however it doesn't seem to have any
advantages over the x86 and uses slightly more memory than the x86 one.
- Open up the XML file in a text editor and configure it, see XML configuration instructions below.
- Download [Quiet](http://www.joeware.net/freetools/tools/quiet/index.htm), this is needed to hide the QbtAssist console window.
- Place everything in the same directory.
- Enable the Qbittorrent WebUI from the Qbittorrent options panel: just check "Web User Interface (Remote control)" and make sure the username and password are the same as the onees
in the XML file, leave everything else as is.
- Add start.bat to Windows autostart.
- Done!


## XML config
Host: this is the complete url to the Qbittorrent WebUI, the correct syntax is `http://IP:port`. If the Qbittorrent client is on the machine from which you're running the QbtAssist
executable (i.e. you're connecting to the WebUI on localhost) the default value (`http://127.0.0.1:8080`) will do, unless you changed the port on your WebUI config panel.

Username: username used to connect to the Qbittorrent WebUI, must be the same as the one entered on Qbittorrent.

Password: password used to connect to the Qbittorrent WebUI, must be the same as the one entered on Qbittorrent.

UploadLimit: value in **bytes** per second to which the upload speed will be capped when a process is running.

DownloadLimit: value in **bytes** per second to which the download speed will be capped when a process is running.

Process: name of a process which should trigger the upload and download cap. This is the same name as the executable, without the ".exe" part. You can name as many process names as you want, just add a <<Process></Process>> line for each one you want to add.


## License
This software is released under [The Unlicense](https://github.com/Wyse-/QbtAssist/blob/master/LICENSE), however the [qBittorrentSharp library](https://github.com/teug91/qBittorrentSharp) is released under a [different license](https://github.com/Wyse-/QbtAssist/blob/master/LICENSE_qBittorrentSharp)


## Compiling & testing
This project has been compiled with Visual Studio Community 2017 and tested on Windows 7 x64 with qBittorrent 4.0.2.
