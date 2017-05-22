# README ets2_savegame_editor

.NET Framework (C#) version 4.5.2 is Project Default, lower versions may work, without warranty

(upgraded to .NET Framework version 4.6 on May, 22nd 2017; the solution on github has not been updated yet and still uses v4.5.2)

No other special files or libaries are required

Compiled binary file is currently x64 Windows only

For x86 Windows or other OS like Linux or MacOSX (x86/x64) compile the program yourself or create a ticket/issue

Eventually you have to change the Project path in the solutions explorer


Click "Open File" to load the ETS2 savegame file

Before you open it, don't forget to decrypt it with the savegame decrypter which you can find here
http://forum.scssoft.com/viewtopic.php?f=34&t=164103

You will be able to create a backup, just enter a filename or create/select it in the Explorer.
If you don't want to create a backup, just leave the textfield empty


Analyze will analyze the savegame data. You can skip this if you are an expert and enter the values to wish directly. However you will be able to modify your garages only if you analyzed your savegame.

In the garage explorer you will see all garages and some other stuff, ignore that, changing that will mess around with your savegame.
Click "upgrade garage" or "downgrade garage" to increase or decrease the level of your garage

Level 0 (color: dark red): You don't own this garage

Level 1 (color: red): Tiny garage

Level 2 (color: orange): Small garage

Level 3 (color: (olive-)green): Big garage


Click "Apply Settings" to apply all changes. Select "don't change" if you don't wish the value to be changed


Quick setup (this will override current data):

"Slow Start": This will give **(GIVE, NOT ADD)** you 5,000 exp and 100,000 money. Other values won't be changed

"Quick Start": This will give **(GIVE, NOT ADD)** you 5,000,000 exp and 100,000,000 money. It also sets all skills to the maximum level. Other values won't be changed

**NOTICE: I can't gurantee that this program does not mess up your savegame. Use the backup function.
Tests with several of my savegames were working fine, yet there are still a few issues with it if your savegame is too large.**
