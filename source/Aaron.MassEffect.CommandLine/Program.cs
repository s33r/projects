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

using System.Collections.Generic;
using System.IO;
using Aaron.MassEffect.Coalesced;
using Aaron.MassEffect.Coalesced.Records;
using Aaron.MassEffect.Core;

namespace MassEffectEditor
{
    internal class Program
    {
        private static void BuildAnnotations(Games game, Container container, string outputLocation)
        {
            List<Annotation> annotations = new List<Annotation>();

            foreach (FileRecord fileRecord in container.Files)
            {
                annotations.Add(new Annotation(game, fileRecord));

                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    annotations.Add(new Annotation(game, sectionRecord));
                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        annotations.Add(new Annotation(game, entryRecord));
                    }
                }
            }


            string json = AnnotationCollection.Serialize(game, annotations);
            File.WriteAllText(outputLocation, json);
        }

        private static List<Annotation> LoadAnnotations(string inputLocation)
        {
            string json = File.ReadAllText(inputLocation);

            return AnnotationCollection.Deserialze(json);
        }


        private static void Main(string[] args)
        {
            Configuration.Instance.Initialize();

            string m1Location = Path.Join(Configuration.Instance.WorkingLocation, "me1.annotations.json");
            string m2Location = Path.Join(Configuration.Instance.WorkingLocation, "me2.annotations.json");
            string m3Location = Path.Join(Configuration.Instance.WorkingLocation, "me3.annotations.json");


            Container me1 = CoalescedFile.Load(Games.Me1,
                Configuration.Instance.Game[Games.Me1].CoalescedConfigurationLocation);
            Container me2 = CoalescedFile.Load(Games.Me2,
                Configuration.Instance.Game[Games.Me2].CoalescedConfigurationLocation);
            Container me3 = CoalescedFile.Load(Games.Me3,
                Configuration.Instance.Game[Games.Me3].CoalescedConfigurationLocation);

            BuildAnnotations(Games.Me1, me1, m1Location);
            BuildAnnotations(Games.Me2, me2, m2Location);
            BuildAnnotations(Games.Me3, me3, m3Location);


            List<Annotation> me1a = LoadAnnotations(m1Location);
            List<Annotation> me2a = LoadAnnotations(m2Location);
            List<Annotation> me3a = LoadAnnotations(m3Location);
        }
    }
}