﻿using System;

using System.Globalization;
using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using FYK.Azure.C19Functions.Config;
//
//    var config = Config.FromJson(jsonString);

namespace Fyk.C19.C19DataLoader.Config
{
    public partial class Config
    {
        [JsonProperty("c19")]
        public C19 C19 { get; set; }

        [JsonProperty("tenantid")]
        public Guid TenantTd { get; set; }

        internal static string ReadUrlText(string url)
        {
            var rv = "";
            using (var client = new WebClient())
            {
                rv = client.DownloadString(url);
            }
            return rv;
        }
    }

    public class C19
    {
        [JsonProperty("target")]
        public Target Target { get; set; }

        [JsonProperty("transfers")]
        public TransferElement[] Transfers { get; set; }
    }

    public class Target
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("authenticationtype")]
        public string AuthenticationType { get; set; }

        [JsonProperty("sqlauthentication")]
        public Sqlauthentication SqlAuthentication { get; set; }

        [JsonProperty("preload", NullValueHandling = NullValueHandling.Ignore)]
        public string PreLoad { get; set; }

        [JsonProperty("postload", NullValueHandling = NullValueHandling.Ignore)]
        public string PostLoad { get; set; }

        public const string SQL = "SQL";
        public const string Windows = "Windows";
    }

    public class Sqlauthentication
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class TransferElement
    {
        [JsonProperty("transfer")]
        public TransferTransfer Transfer { get; set; }
    }

    public class TransferTransfer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("loginfo")]
        public string LogInfo { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("batchsize")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long BatchSize { get; set; }
    }

    public class Source
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("columnseparator")]
        public string ColumnSeparator { get; set; }

        [JsonProperty("decimalseparator")]
        public string DecimalSeparator { get; set; }
        [JsonProperty("encoding", NullValueHandling = NullValueHandling.Ignore)]
        public string Encoding { get; set; }
    }

    public partial class Config
    {
        public static Config FromJson(string json) => JsonConvert.DeserializeObject<Config>(json, Fyk.C19.C19DataLoader.Config.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Config self) => JsonConvert.SerializeObject(self, Fyk.C19.C19DataLoader.Config.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
