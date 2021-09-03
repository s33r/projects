#load "TestLib.csx"
#load "TestLib2.csx"

using System;
using Newtonsoft.Json;



Environment.CurrentDirectory


foreach (var arg in Environment.GetCommandLineArgs())
{
    Console.WriteLine(arg);
}

