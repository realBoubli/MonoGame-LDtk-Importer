﻿using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using System.Text.Json;

namespace MonoGame_LDtk_Importer
{
    [ContentImporter(".json, .ldtk", DisplayName = "LDtk Project Importer", DefaultProcessor = "LDtk Project Processor")]
    public class Importer1 : ContentImporter<JsonElement>
    {
        public override JsonElement Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing LDtk project: {0}", filename);
            return JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(filename));
        }
    }
}