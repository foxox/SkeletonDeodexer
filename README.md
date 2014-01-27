# SkeletonDeodexer

I wrote most of this in January 2014. I apologize if some of this is out of date by the time you see this, but I can assure you that the journey you take to figure out "what the heck is going on" will be worthwhile toward your knowledge of just what the heck is going on with this Android stuff. I made this program to compensate for the deterioration of out-of-date tools made 3 or 4 years ago and learned a bit along the way :)


## Introduction:
This software requires that you have first pulled some "odexed" .odex and .apk (matching) files from your phone or phone ROM. To do this, you can use the ADB tool in the Android SDK package, available from https://developer.android.com/sdk/index.html Download the SDK ADT bundle and then look in "sdk/platform-tools".

Craft an adb command like this to get the files onto your computer:
* adb pull /system/framework C:\files_from_my_phone\framework

Then, you can run this deodex software and pass in "C:\files_from_my_phone\framework" as the first (input) argument and maybe something like "C:\files_from_my_phone\framework_deodexed" as the second (output) argument. 
* SkeletonDeodexer.exe "C:\files_from_my_phone\framework" "C:\files_from_my_phone\framework\deodexed"


## Setup:
1. Clone the SkeletonDeodexer repository from github. Presumably you have already done this.
2. Open the project in Visual Studio. I used VS2010. Build the project.
3. Done!

Note that I have a "Post-build Event" configured on the Project. This copies the 7za.exe, smali.jar, and baksmali.jar files from SkeletonDeodexer/external/ to the executable output directory. The same files are used for both Debug and Release.

### Optional Setup: (if you think these need to be updated)

#### 7zip
1. Get the 7zip command line version from: http://www.7-zip.org/download.html
2. Navigate to the folder SkeletonDeodexer\SkeletonDeodexer\bin\Debug (or \Release if you built it in Release mode).
3. Place 7za.exe into this folder.

#### Smali/Baksmali
1. Get smali and baksmali from: https://bitbucket.org/JesusFreke/smali/downloads
2. Navigate to the folder SkeletonDeodexer\SkeletonDeodexer\bin\Debug (or \Release if you built it in Release mode).
3. Place smali.jar and baksmali.jar into this folder.
4. Make sure smali.jar and baksmali.jar are named exactly like that -- i.e. remove the version number from the filenames.


## Run:
Run the program with command line arguments like this:
* SkeletonDeodexer.exe input/dir/ output/dir/
  * ...where input/dir/ is the directory the software should check for *.odex files to deodex. The corresponding *.apk files (with matching names) should be in that directory too.
  * ..and where output/dir/ is the directory the software should put the resulting deodexed *.apk files as it deodexes them.


## Troubleshooting:
* If you run into trouble, consider trying to update 7zip and/or Smali/Baksmali (see the Setup section above). I originally made this with version 2.0.3.

* If ADB indicates that your device is "offline" or "unauthorized", make sure you have an up-to-date version of the Android SDK ("offline" device) and make sure that USB Debugging is enabled on the phone ("unauthorized" device). Sometimes you may have to uncheck and re-check the "USB Debugging" setting in order for it to work correctly.

* Make sure your phone is plugged into your computer while trying to pull the files from it (duh!).

* I had some trouble with 7zip being unable to add a file from a directory other than the current working directory into an existing archive. This is why I set the current working directory when I invoke 7zip. Keep an eye out for issues like this. (Check to see if "temp" appears in your apk files as a separate directory -- it shouldn't unless it was in there in the first place).

* Remember that APK files are just ZIP files. You can rename one to *.zip and then use it in ordinary ways you would use a ZIP file. This may help troubleshooting.


