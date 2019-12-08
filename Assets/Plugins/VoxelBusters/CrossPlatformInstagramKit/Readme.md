
# Cross Platform Instagram Kit

## Project Paths
Demos : Assets/Plugins/VoxelBusters/CrossPlatformInstagramKit/Demo/Scenes/
 
## Online Links

[Tutorials](https://assetstore.instagramkit.voxelbusters.com)
[Support](https://join.skype.com/ln8JcMryHXpv)

## Plugin Usage

For using this plugin methods you need to import the required namespace.

Please include the below statement before using any plugin methods

  

>_using VoxelBusters.InstagramKit;_

We expose ****InstagramKitManager**** class to allow the access for plugin functionalities.

### Check Service Availability

Check if the instagram kit features can be usable. As the apps need instagram app on the running platform, its good to check this before calling any of the methods.

```
public void IsAvailable()
{
	bool isAvailable  =  InstagramKitManager.IsAvailable();

	string message  = isAvailable ? "Instagram Kit is available!" : "Instagram Kit is not available. Instagram app may not be installed.";

	Debug.Log(message);

}
```

### Sharing Content

Instagram Kit allows to share image, video. Along with the sharing media, you can attach stickers of your choice and attachment links (which allows users on instagram platform to click them - This needs approval from Instagram (More Info : https://help.instagram.com/contact/605050879855497?fbclid=IwAR1xLRtNHLGxOFu77Rhi8dUOB7xv-lDp6CvwjgMOOaa-tG0BJK_XbMsDdB4) )

Sharing content can be of three types

* Image

> _Images must be JPG or PNG._

* Video

>  Videos can be upto 20 sec
> Format can be H.264, H.265, WebM

---

For any sharing content (image/video), you can add three kinds of extra data.

  

* Stickers

> _Stickers allow to show additional content to attribute the existing sharing content._
> _Only one sticker is allowed._
> _A still sticker must be a PNG 1MB or smaller._
> _An animated sticker must be a GIF or WebP (preferred) 1MB or smaller._

* Attachment URL
> _URL that can be attached to the sharing content. Usually, you can link your game or attach a deep link to your game content._
> _The attachment URL must be a properly formatted URL in string format._
>  This needs approval from Instagram (More Info : https://help.instagram.com/contact/605050879855497?fbclid=IwAR1xLRtNHLGxOFu77Rhi8dUOB7xv-lDp6CvwjgMOOaa-tG0BJK_XbMsDdB4) )

First, you need to create StoryContent instance and fill in details about extra data you want to add (stickers/url)

**1. Create StoryContent with the content path**
> **For Sharing Photo**
```
StoryContent content = new StoryContent (path_to_file, false);
```

> **For Sharing Video**
```
StoryContent content = new StoryContent (path_to_file, true);
```

**3. Create additional data you want to add to the sharing content and set it (Stickers /  Attachment URL)**

> **Create Sticker**
```
private Sticker CreateSticker()
{
	Sticker sticker = null;
	sticker = new Sticker(Application.persistentDataPath + "/" + SharingStickerName);

	return sticker;
}
```
>**Set Sticker to content**
```
content.SetSticker(sticker);

```
> **Set Attachment URL**

```
content.SetAttachmentUrl("https://www.google.com");
```

**4. Share the content and receive the status in callback**
```
InstagramKitManager.Share(content, OnShareComplete);
```
Receive error if any in the callback function
```
private void OnShareComplete(bool success, string error)
{
	string message  =  success ? "Successfully Shared" : "Failed to share " + error;
	Debug.Log(message);
}
```


