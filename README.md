# SriYantraVR

Experience a guided Sri Yantra-based meditation in mobile VR.

![](https://i.imgur.com/OmY30mZ.gif)

[Download Latest Build](https://github.com/Juwce/SriYantraVR/blob/master/latest%20build.apk)

**Setup**

Refer to the [Wiki](https://github.com/Juwce/SriYantraVR/wiki) for setup instructions.

**Unity Version**

Developed in Unity 5.6.4

**Hardware Requirements**

Required: Android device capable of running Google Cardboard VR.

**Project Difficulties/Setbacks**

Learning new languages/IDE's:

This is only our second time working with the Unity IDE and C#, and the first
time working with Android Studios. As such, there was a learning curve for us
to get acquainted with the IDE and code, and we were often lost when we ran
into problems/bugs with Android Studio and Unity.


Unity and SDK/JDK Compatability:

Unity was only compatible with certain versions of JDK and SDK. After a lot of 
searching online, we've finally discovered that we needed to downgrade JDK
to version 8, and have the most recent version of SDK.


Broadcast Receiver via Android Plugin in Unity app:

We've tried to implement a receiver to receive messages broadcasted by a 
separate native app in the phone we build our VR application into. We do this
by embedding an Android plugin (written in Java) into our VR app (written in 
Unity) which wil be called by the Unity app. Though the VR app is able to call
the plugin (confirmed through printing out debug logs to the console), the
onReceive() method (that exists in the plugin) which receives the broadcasted 
messages is never able to be called. After working several weeks with an
android and bluetooth expert, we still were not able to discover the root of
the problem, and have moved to an alternative solution via TCP socket 
connection.


Unable to build same app from different laptop onto the same android device:

If a Unity app is already built onto an android device from one laptop/PC, we
cannot build the same app from a different laptop, because of conflicting ID
issues. The work around for this is to delete all of the app's existing data by searching through the android device's file directory, on top of unistalling theapp. A quicker way would just to push changes onto a remote repository (github) pull those onto the orginal laptop, and just build from the same laptop.

This issue proved a minor inconvenience when working together in a group.


Unable to build onto Oculus Go:

Although Oculus Go was expected to be binary compatible with Android, we were
unable to build the VR app onto Oculus Go the same way we did onto our Android
device.



**Links to Tutorials We've Used**

Tutorial on embedding an Android Plugin into Unity, and how to extract a .jar
file from Android Studio:
https://xinyustudio.wordpress.com/2015/12/31/step-by-step-guide-for-developing-android-plugin-for-unity3d-i/


Tutorial for implementing an Android Receiver within Unity:
http://jeanmeyblum.weebly.com/scripts--tutorials/communication-between-an-android-app-and-unity
