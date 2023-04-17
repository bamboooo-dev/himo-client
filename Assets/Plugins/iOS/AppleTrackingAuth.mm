#import <Foundation/Foundation.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>

#ifdef __cplusplus
extern "C" {
#endif
//ATTダイアログを表示するメソッド
void requestIDFA() {
    if (@available(iOS 14,*)){
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
        // Tracking authorization completed. Start loading ads here.
        // [self loadAd];
        }];
    }
}
#ifdef __cplusplus
}
#endif
