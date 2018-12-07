using Foundation;
using UIKit;
using JPush;
using System;
using ObjCRuntime;
using UserNotifications;

namespace Sample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            JPUSHRegisterEntity entity = new JPUSHRegisterEntity();

            if(UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                entity.Types = (nint)((long)JPAuthorizationOptions.Alert | (long)JPAuthorizationOptions.Badge | (long)JPAuthorizationOptions.Sound | (long)JPAuthorizationOptions.ProvidesAppNotificationSettings);
            }
            else
            {
                entity.Types = (nint)((long)JPAuthorizationOptions.Alert | (long)JPAuthorizationOptions.Badge | (long)JPAuthorizationOptions.Sound);
            }

            if(UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // 可以添加自定义categories
                //if(UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                //{
                    //      NSSet<UNNotificationCategory *> *categories;
                //    NSSet categories = null;
                //   entity.Categories = categories;
                //}
                //else
                //{
                    //      NSSet<UIUserNotificationCategory *> *categories;
                //   NSSet categories = null;
                //    entity.Categories = categories;
                //}
            }

            JPUSHService.RegisterForRemoteNotificationConfig(entity, new PUSHRegisterDelegate());
            JPUSHService.SetupWithOption(launchOptions, "Your-Own-AppKey", "Channel", false);

            JPUSHService.RegistrationIDCompletionHandler((resCode, registrationID) =>
            {
                if(resCode == 0)
                {
                    System.Diagnostics.Debug.WriteLine("registrationID获取成功："+ registrationID);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("registrationID获取失败：" + registrationID);
                }
            });
            return true;
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
            application.ApplicationIconBadgeNumber = 0;
            application.CancelAllLocalNotifications();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            System.Diagnostics.Debug.WriteLine("Device Token: " + deviceToken.Description);
            JPUSHService.RegisterDeviceToken(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            System.Diagnostics.Debug.WriteLine("did Fail To Register For Remote Notifications With Error: " + error.Description);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            JPUSHService.HandleRemoteNotification(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            JPUSHService.ShowLocalNotificationAtFront(notification, null);
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }

    public class PUSHRegisterDelegate : JPUSHRegisterDelegate
    {
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            // Do something, e.g. set a Boolean property to track the foreground state.
            NSDictionary userInfo = response.Notification.Request.Content.UserInfo;
            UNNotificationRequest request = response.Notification.Request; // 收到推送的请求
            UNNotificationContent content = request.Content; // 收到推送的消息内容

            NSNumber badge = content.Badge;  // 推送消息的角标
            string body = content.Body;    // 推送消息体
            string subtitle = content.Subtitle;  // 推送消息的副标题
            string title = content.Title;  // 推送消息的标题

            if (request.Trigger is UNPushNotificationTrigger)
            {
                // iOS10 前台收到远程通知 处理业务逻辑
                // User tapped on notification badge
                System.Diagnostics.Debug.WriteLine("iOS10 收到远程通知:");
            }
            else
            {
                // iOS10 前台收到本地通知
                System.Diagnostics.Debug.WriteLine("iOS10 收到本地通知:");
            }

            // 前台情况下alert
            completionHandler();
        }

        public override void OpenSettingsForNotification(UNUserNotificationCenter center, UNNotification notification)
        {
            string title = null;
            if(notification != null)
            {
                title = "从通知界面直接进入应用";
            }
            else
            {
                title = "从系统设置界面进入应用";
            }
            UIAlertController test = UIAlertController.Create(title, "pushtest", UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.ShowViewController(test, null);
            
       }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<nint> completionHandler)
        {
            // Do something, e.g. set a Boolean property to track the foreground state.
            NSDictionary userInfo = notification.Request.Content.UserInfo;
            UNNotificationRequest request = notification.Request; // 收到推送的请求
            UNNotificationContent content = request.Content; // 收到推送的消息内容
            NSNumber badge = content.Badge;  // 推送消息的角标
            string body = content.Body;    // 推送消息体
            string subtitle = content.Subtitle;  // 推送消息的副标题
            string title = content.Title;  // 推送消息的标题

            if (request.Trigger is UNPushNotificationTrigger)
            {
                // iOS10 前台收到远程通知 处理业务逻辑
                // User tapped on notification badge
                //if (userInfo != null && userInfo.ContainsKey(new NSString("dataType")) && userInfo.ContainsKey(new NSString("data")))
                //    Acr.UserDialogs.UserDialogs.Instance.Alert(userInfo["dataType"].ToString(), userInfo["data"].ToString(), "ok");
            }
            else
            {
                // iOS10 前台收到本地通知
            }

            // 前台情况下alert
            completionHandler((System.nint)((System.nint)((long)UNNotificationPresentationOptions.Badge | (long)UNNotificationPresentationOptions.Sound) | (long)UNNotificationPresentationOptions.Alert));
        }
    }
} 

