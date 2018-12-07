using System;
using ObjCRuntime;

namespace JPush
{
    [Native]
    public enum JPAuthorizationOptions : ulong
    {
        None = 0,
        Badge = 1,
        Sound = 2,
        Alert = 4,
        CarPlay = 8,
        CriticalAlert = 16,
        ProvidesAppNotificationSettings = 32,
        Provisional = 64
    }
}
