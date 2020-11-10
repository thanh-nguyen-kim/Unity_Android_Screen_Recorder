# Unity_Android_Screen_Recorder

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
