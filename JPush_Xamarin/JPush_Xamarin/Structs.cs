using System;
using ObjCRuntime;

namespace JPush
{
    [Native]
    public enum JPAuthorizationOptions : ulong
    {
        None = 0,
        Badge = (1 << 0),
        Sound = (1 << 1),
        Alert = (1 << 2),
        CarPlay = (1 << 3),
        CriticalAlert = (1 << 4),
        ProvidesAppNotificationSettings = (1 << 5),
        Provisional = (1 << 6)
    }
}
