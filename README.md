# Unity_Android_Screen_Recorder

<https://killertee.wordpress.com/2018/01/08/unity-android-record-game-screen-with-audio-using-your-ad-hoc-plug-in/>

**<p align="center">If you are using this library in one of your applications and would like to thank me:</p>**

<p align="center"><a href="https://www.buymeacoffee.com/KamperTee" target="_blank" ><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 164px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a></p>

<p>or via PayPal:</p>

<https://www.paypal.com/paypalme/kampertee>

## How to use

+ You need to create a gameobject name 'AndroidUtils' and add 'AndroidUtils.cs' script to it.
+ When you want to start capturing your screen, call 'androidUtils.StartRecording()'. 'androidUtils' is a reference to AndroidUtils.cs instance.
+ When you want to stop capturing your screen, call 'androidUtils.StopRecording()'. 'androidUtils' is a reference to AndroidUtils.cs instance.

## A simple demo of AndroidUtils Plugin which provide 3 main function

+ Record android screen.
+ Request runtime permissions like Audio..
+ Refresh android gallery so you capture file will show up on top.

## It also contains 2 helper functions

+ NativeShare for native send or share a video, image or text via social network, sms and email.
+ ShowToast show android toast message.

## Note that

+ You might need to adjust the 'SCREEN_WIDTH' constant in AndroidUtils.cs for best video resolution. If 'SCREEN_WIDTH' is too big, your app may crash.
+ The plugin only support api 21 and higher.
+ Some Mediatek devices will crash if DEFAULT video encoder is used. Change to H264 may solve the problem.

```cs
androidRecorder.Call("setupVideo", width, height,bitrate, fps,audioEnable,VideoEncoder.H264.ToString());
```

Visit this site to get a details explaining of Unity_Android_Screen_Recorder.


<https://killertee.wordpress.com/2018/01/08/unity-android-record-game-screen-with-audio-using-your-ad-hoc-plug-in/>

