//
//  InstagramBinding.mm
//  Cross Platform Instagram Kit
//
//  Created by Ayyappa Reddy on 20/12/18.
//  Copyright (c) 2018 Voxel Busters Interactive LLP. All rights reserved.
//

#import "InstagramKitBinding.h"
#import "InstagramKitHandler.h"
#include "PluginBase/AppDelegateListener.h"



#pragma mark - Helpers

NSString* instagramkit_convertToObjCString (const char * charStr)
{
    if (charStr == NULL)
        return NULL;
    else
        return [NSString stringWithUTF8String:charStr];
}


#pragma mark - Query

BOOL instagramkit_isAvailable ()
{
    return [[InstagramKitHandler sharedInstance] isAvailable];
}

#pragma mark - Share

void instagramkit_share_feed (char* contentDataPath, bool isVideoSharing)
{
    [[InstagramKitHandler sharedInstance] share:instagramkit_convertToObjCString(contentDataPath)
                                 isVideoSharing:isVideoSharing];
}

void instagramkit_share_story (char* contentDataPath, char* filePath, char* attachmentUrl, bool isVideoSharing)
{
    [[InstagramKitHandler sharedInstance] share:instagramkit_convertToObjCString(contentDataPath)
                               stickerFilePath:instagramkit_convertToObjCString(filePath)
                                 attachmentUrl:instagramkit_convertToObjCString(attachmentUrl)
                                isVideoSharing:isVideoSharing];
}
