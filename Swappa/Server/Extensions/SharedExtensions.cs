﻿namespace Swappa.Server.Extensions
{
    public static class SharedExtensions
    {
        public static bool GetOrdefaul(this bool? boolean) =>
            boolean ?? false;
    }
}
