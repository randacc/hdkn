﻿using System;
using System.Collections.Generic;
using System.IO;
using Hadouken.Framework.SemVer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hadouken.Plugins.Metadata
{
    public sealed class Manifest : IManifest
    {
        public const string FileName = "manifest.json";

        private static readonly IDictionary<int, Func<IManifestReader>> ManifestReaders =
            new Dictionary<int, Func<IManifestReader>>();

        static Manifest()
        {
            ManifestReaders.Add(1, () => new ManifestV1Reader());
        }

        public Manifest()
        {
            Dependencies = new Dependency[] {};
        }

        public string Name { get; set; }

        public SemanticVersion Version { get; set; }

        public Dependency[] Dependencies { get; set; }

        public static bool TryParse(Stream json, out IManifest manifest)
        {
            Exception exception;
            return TryParse(json, out manifest, out exception);
        }

        public static bool TryParse(Stream json, out IManifest manifest, out Exception exception)
        {
            manifest = null;
            exception = null;

            try
            {
                using (var streamReader = new StreamReader(json))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var obj = JToken.ReadFrom(jsonReader) as JObject;

                    if (obj == null)
                        return false;

                    JToken manifestVersionToken;

                    if (!obj.TryGetValue("manifest_version", out manifestVersionToken))
                        return false;

                    if (manifestVersionToken.Type != JTokenType.Integer)
                        return false;

                    var manifestVersion = manifestVersionToken.Value<int>();

                    if (!ManifestReaders.ContainsKey(manifestVersion))
                        return false;

                    var manifestReader = ManifestReaders[manifestVersion]();
                    manifest = manifestReader.Read(obj);

                    return true;
                }
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
        }
    }
}
