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
        Alert = (1 << 2)
    }
}
