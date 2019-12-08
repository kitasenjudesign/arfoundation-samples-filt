//
//  InstagramKitBinding.h
//  Cross Platform Instagram Kit
//
//  Created by Ayyappa Reddy on 20/12/18.
//  Copyright (c) 2018 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

// Query Methods
UIKIT_EXTERN BOOL instagramkit_isAvailable ();

// Share Methods
UIKIT_EXTERN void instagramkit_share_feed (char* contentDataPath, bool isVideoSharing);
UIKIT_EXTERN void instagramkit_share_story (char* contentDataPath, char* stickerFilePath, char* attachmentUrl, bool isVideoSharing);
