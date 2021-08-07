// Copyright (C) 2021 Aaron C. Willows (aaron@aaronwillows.com)
// 
// This program is free software; you can redistribute it and/or modify it under the terms of the
// GNU Lesser General Public License as published by the Free Software Foundation; either version
// 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See
// the GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston,
// MA 02111-1307 USA

using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Aaron.MassEffect.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            _ = builder.Services.AddScoped(serviceProvider =>
                new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            TestIo();

            await builder.Build().RunAsync();
        }

        private static void DirPrinter(string directoryLocation, int depth)
        {
            foreach (string directory in Directory.GetDirectories(directoryLocation))
            {
                Console.WriteLine($"{new string(' ', depth)}📁 {directory}");
            }

            foreach (string file in Directory.GetFiles(directoryLocation))
            {
                Console.WriteLine($"{new string(' ', depth)}🗎 {file}");
            }

            foreach (string directory in Directory.GetDirectories(directoryLocation))
            {
                DirPrinter(directory, depth++);
            }
        }

        private static void ReadRandom()
        {
            string fileLocation = "./dev/random";
            int totalData = 8;

            using BinaryReader input = new BinaryReader(File.OpenRead(fileLocation));

            byte[] data = input.ReadBytes(totalData);

            foreach (byte bite in data) { Console.Write("{0:X2} ", bite); }

            Console.WriteLine();
        }

        private static async void TestIo()
        {
            string fileLocation = "file.txt";
            string message = "Hello World";

            ReadRandom();

            DirPrinter(".", 0);

            await File.WriteAllTextAsync(fileLocation, message);


            Console.WriteLine("---------------------");
            Console.WriteLine($"Environment.UserName = {Environment.UserName}");
            Console.WriteLine($"Environment.MachineName = {Environment.MachineName}");
            Console.WriteLine($"Environment.OSVersion = {Environment.OSVersion}");
            Console.WriteLine($"Environment.OSVersion = {Environment.Is64BitOperatingSystem}");
            Console.WriteLine($"Environment.OSVersion = {Environment.NewLine}");

            Console.WriteLine($"RuntimeEnvironment.GetRuntimeDirectory = {RuntimeEnvironment.GetRuntimeDirectory()}");
            Console.WriteLine($"RuntimeInformation.OSArchitecture = {RuntimeInformation.OSArchitecture}");
            Console.WriteLine($"RuntimeInformation.OSDescription = {RuntimeInformation.OSDescription}");
            Console.WriteLine($"RuntimeInformation.RuntimeIdentifier = {RuntimeInformation.RuntimeIdentifier}");
            Console.WriteLine($"RuntimeInformation.FrameworkDescription = {RuntimeInformation.FrameworkDescription}");


            string result = await File.ReadAllTextAsync(fileLocation);

            Console.WriteLine(result);
        }
    }
}