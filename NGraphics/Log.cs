﻿using System;
using System.Diagnostics;

namespace NGraphics.Custom
{
    internal static class Log
    {
        public static void Error(Exception ex)
        {
            Debug.WriteLine("ERROR: " + ex);
        }
    }
}