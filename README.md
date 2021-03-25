# Remote utilities
This project aims to facilitate communication, information management, file and clipboard transfer within the same LAN network. This is especially useful when you are working in an office and efficiently, or for example, transferring information to any device without the need to use Bluetooth technology.

**Tools versions:**

 - C# 7.3
 - .NET Framewok 4.7.2


 

**NuGet Packages**

 1. ToastNotifications [OFFICIAL]: to facilitate the display of pop-up notifications in windows 10
 

**OPCODES**
The first byte of the transmission buffers correspond to an internal operational code of the program, and they indicate to the receiving device what action to execute with the information that continues to the array.


0: Communication:
1: Chat message
2: Run a program -
3: Send file
4: Receive file
5: turn off or anything like that
6: Copy to clipboard
7: Show message on screen

As development progresses, obviously more opcodes will be added
 
Indeed, this description will be modified over time and is only provisional.
