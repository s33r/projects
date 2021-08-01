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
using System.Linq;
using Aaron.MassEffect.Core;
using Newtonsoft.Json;

namespace Aaron.MassEffect.Coalesced
{
    public static class AnnotationCollection
    {
        public static List<Annotation> Deserialze(string json)
        {
            AnnotationJson jsonObject = JsonConvert.DeserializeObject<AnnotationJson>(json);

            jsonObject.Annotations.ForEach(a => a.Game = jsonObject.Game);

            return jsonObject.Annotations;
        }


        public static string Serialize(Games game, List<Annotation> annotations)
        {
            AnnotationJson jsonObject = new AnnotationJson
            {
                Game = game, Annotations = annotations.Select(a => a.GetWireVersion()).ToList(),
            };

            JsonSerializerSettings settings =
                new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore};

            return JsonConvert.SerializeObject(jsonObject, settings);
        }


        private class AnnotationJson
        {
            public List<Annotation> Annotations { get; set; }
            public Games Game { get; set; }
        }
    }
}