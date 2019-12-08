//
//  InstagramKitHandler.m
//  Cross Platform Instagram Kit
//
//  Created by Ayyappa Reddy on 20/12/18.
//  Copyright (c) 2018 Voxel Busters Interactive LLP. All rights reserved.
//

#import "InstagramKitHandler.h"
#import <Photos/PHPhotoLibrary.h>
#import <Photos/PHAssetCreationRequest.h>
#import <Photos/PHImageManager.h>
#import <MobileCoreServices/MobileCoreServices.h>

@implementation InstagramKitHandler

#define kUnityGameObject            "InstagramKitInternal"
#define kShareFinished              "ShareFinished"

#define kInstagramScheme            @"instagram://"

@synthesize isLoggedIn;

#pragma mark - Singleton Instance
+ (id)sharedInstance
{
    static InstagramKitHandler *sharedInstance = nil;
    @synchronized(self) {
        if (sharedInstance == nil)
            sharedInstance = [[self alloc] init];
    }
    return sharedInstance;
}

#pragma mark - Query Methods

- (BOOL)isAvailable
{
    return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:kInstagramScheme]];
}

#pragma mark - Sharing Methods

- (void)share :(NSString*) contentDataPath isVideoSharing:(BOOL)isVideoSharing
{

}
- (void)share :(NSString*) contentDataPath stickerFilePath:(NSString*) stickerFilePath attachmentUrl:(NSString*) attachementUrl isVideoSharing:(BOOL) isVideoSharing
{
    NSData *contentData = [self GetNSDataFromFilePath:contentDataPath];
    NSData *stickerData = [self GetNSDataFromFilePath:stickerFilePath];


    NSURL *urlScheme = [NSURL URLWithString:@"instagram-stories://share"];

    if ([[UIApplication sharedApplication] canOpenURL:urlScheme])
    {
        NSMutableDictionary *pasteboardItems = [NSMutableDictionary dictionary];

        if(contentData != nil)
        {
            if(isVideoSharing)
            {
                [pasteboardItems setObject:contentData forKey:@"com.instagram.sharedSticker.backgroundVideo"];
            }
            else
            {
                [pasteboardItems setObject:contentData forKey:@"com.instagram.sharedSticker.backgroundImage"];
            }
        }

        if(attachementUrl != nil)
        {
            [pasteboardItems setObject:attachementUrl forKey:@"com.instagram.sharedSticker.contentURL"];
        }


        if(stickerData != nil)
        {
            [pasteboardItems setObject:stickerData forKey:@"com.instagram.sharedSticker.stickerImage"];
        }

        NSArray *items = @[pasteboardItems];
        NSDictionary *pasteboardOptions = @{UIPasteboardOptionExpirationDate : [[NSDate date] dateByAddingTimeInterval:60 * 5]};

        [[UIPasteboard generalPasteboard] setItems:items options:pasteboardOptions];

        [[UIApplication sharedApplication] openURL:urlScheme options:@{} completionHandler:^(BOOL success)
         {
             UnitySendMessage(kUnityGameObject, kShareFinished, success ? "" : "Failed Sharing");
         }];
    }
}


#pragma mark - Utility Methods

 - (NSData*) GetNSDataFromFilePath:(NSString*) filePath
 {
     if(filePath == nil)
         return nil;


     CFStringRef fileExtension = (CFStringRef) [filePath pathExtension];
     CFStringRef fileUTI = UTTypeCreatePreferredIdentifierForTag(kUTTagClassFilenameExtension, fileExtension, NULL);

     if (UTTypeConformsTo(fileUTI, kUTTypeImage))
     {

         UIImage *image = [UIImage imageWithContentsOfFile:filePath];

         if([filePath hasSuffix:@"jpg"] || [filePath hasSuffix:@"jpeg"])
         {
             return UIImageJPEGRepresentation(image, 1.0f);
         }
         else
         {
             return UIImagePNGRepresentation(image);
         }
     }
     else if (UTTypeConformsTo(fileUTI, kUTTypeMovie))
     {
         return [NSData dataWithContentsOfFile:filePath];
     }

     return  nil;
 }

@end
