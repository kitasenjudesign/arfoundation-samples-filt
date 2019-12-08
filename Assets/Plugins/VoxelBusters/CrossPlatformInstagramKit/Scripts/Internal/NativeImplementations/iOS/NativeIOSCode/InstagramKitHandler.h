//
//  InstagramKitHandler.h
//  Cross Platform Instagram Kit
//
//  Created by Ayyappa Reddy on 20/12/18.
//  Copyright (c) 2018 Voxel Busters Interactive LLP. All rights reserved.
//
#import <Foundation/Foundation.h>

@interface InstagramKitHandler : NSObject

@property(nonatomic)            BOOL                        isLoggedIn;

+ (id)sharedInstance;

- (BOOL)isAvailable;

- (void)share :(NSString*) contentDataPath isVideoSharing:(BOOL) isVideoSharing;

- (void)share :(NSString*) contentDataPath stickerFilePath:(NSString*) stickerFilePath attachmentUrl:(NSString*) attachementUrl isVideoSharing:(BOOL) isVideoSharing;

@end
