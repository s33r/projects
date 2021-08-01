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
using System.Collections.Generic;
using System.IO;
using Aaron.MassEffect.Coalesced.Me3;
using Aaron.MassEffect.Coalesced.Me3.DataStructures;
using Aaron.MassEffect.Core;
using Aaron.MassEffect.Core.Exceptions;

namespace Aaron.MassEffect.Coalesced
{
    public static class CoalescedFile
    {
        private static readonly Dictionary<Games, CodecFactory> _codecs = new Dictionary<Games, CodecFactory>();

        static CoalescedFile()
        {
            _codecs.Add(Games.Me1, name => new Me1.Codec(name));
            _codecs.Add(Games.Me2, name => new Me1.Codec(name)); // Mass Effect 2 uses the same format as ME1
            _codecs.Add(Games.Me3, name => new Codec(name));
        }

        public static void Compare(Games game, byte[] oldData, byte[] newData)
        {
            ICodec oCodec = _codecs[game]("old");
            ICodec nCodec = _codecs[game]("new");

            Container oContainer = oCodec.Decode(oldData);
            Container nContainer = nCodec.Decode(newData);

            oCodec.Dump();
            nCodec.Dump();

            oContainer.DumpRecords("old.container.txt");
            nContainer.DumpRecords("new.container.txt");
        }


        public static Container Load(Games game, byte[] data, string name)
        {
            if (!_codecs.ContainsKey(game)) { throw new GameNotSupportedException(game); }

            ICodec codec = _codecs[game](name);
            Container container = codec.Decode(data);

            return container;
        }

        public static Container Load(Games game, byte[] data)
        {
            return Load(game, data, $"New {Enum.GetName(typeof(Games), game)} Loading Codec");
        }

        public static Container Load(Games game, string fileLocation, string name)
        {
            byte[] data = File.ReadAllBytes(fileLocation);

            return Load(game, data, name);
        }

        public static Container Load(Games game, string fileLocation)
        {
            byte[] data = File.ReadAllBytes(fileLocation);

            return Load(game, data, $"New {Enum.GetName(typeof(Games), game)} Loading Codec");
        }

        public static byte[] Save(Games game, Container container, string name)
        {
            if (!_codecs.ContainsKey(game)) { throw new GameNotSupportedException(game); }

            ICodec codec = _codecs[game](name);
            byte[] data = codec.Encode(container);

            return data;
        }

        public static byte[] Save(Games game, Container container)
        {
            return Save(game, container, $"New {Enum.GetName(typeof(Games), game)} Loading Codec");
        }

        public static void Save(Games game, Container container, string outputLocation, string name)
        {
            byte[] data = Save(game, container, name);

            File.WriteAllBytes(outputLocation, data);
        }

        private static bool CompareBytes(byte[] oldData, byte[] newData)
        {
            if (oldData.Length != newData.Length) { return false; }

            for (int index = 0; index < oldData.Length; index++)
            {
                byte originalByte = oldData[index];
                byte newByte = newData[index];

                if (originalByte != newByte)
                {
                    Console.WriteLine("[{0}] {1:X2} | {2:X2}", index, originalByte, newByte);
                    return false;
                }
            }

            return true;
        }


        private static void DumpStringTable(Codec codec, StreamWriter output)
        {
            foreach (StringTableEntry entry in codec.StringTable.Entries)
            {
                output.WriteLine("[{0,8}] ({1,10}) {2}", entry.Offset, entry.Checksum, entry.Value);
            }
        }

        private delegate ICodec CodecFactory(string name);
    }
}