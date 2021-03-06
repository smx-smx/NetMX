using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NetMX.Remote.HttpAdaptor.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetMX.Remote.HttpAdaptor.Formatters
{    
    public class MBeanAttributeJsonFormatter: MediaTypeFormatter
    {
        public MBeanAttributeJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.netmx.attr+json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            return true; // type != typeof(IKeyValueModel);
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof (MBeanAttributeResource);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        using (var streamReader = new StreamReader(readStream, Encoding.UTF8))
                        {
                            using (var jsonTextReader = new JsonTextReader(streamReader))
                            {
                                var deserialized = JObject.Load(jsonTextReader);
                                return (object) new MBeanAttributeResource
                                                    {
                                                        Value = DeserializeValue(deserialized["value"])
                                                    };
                            }
                        }
                    });
        }

        private static object DeserializeValue(JToken deserialized)
        {
            if (deserialized == null)
            {
                return null;
            }
            if (deserialized.Type == JTokenType.String)
            {
                return deserialized.Value<string>();
            }
            if (deserialized.Type == JTokenType.Array)
            {
                var array = (JArray) deserialized;
                if (!array.Any())
                {
                    return null;
                }
                var firstElement = array.First();
                if (firstElement.Type == JTokenType.String)
                {
                    return array.Select(x => x.Value<string>()).ToArray();
                }
                if (firstElement.Type == JTokenType.Object)
                {
                    return array.Select(DeserializeCompositeValue).ToArray();
                }
            }
            if (deserialized.Type == JTokenType.Object)
            {
                return DeserializeCompositeValue(deserialized);
            }
            throw new NotSupportedException("Not supported value type: " + deserialized.Type);
        }

        private static CompositeData DeserializeCompositeValue(JToken compositeValue)
        {
            var jsonObject = (JObject) compositeValue;
            var properties = jsonObject.Properties().Select(x => new CompositeDataProperty(x.Name, x.Value.Value<string>()));
            return new CompositeData(properties);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var jsonObject = new JObject();
                        var typedValue = (MBeanAttributeResource) value;
                        jsonObject["parent"] = typedValue.MBeanHRef;
                        jsonObject["name"] = typedValue.Name;
                        jsonObject["value"] = SerializeValue(typedValue.Value);
                        
                        using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(writeStream, Encoding.UTF8)) {CloseOutput = false})
                        {
                            jsonObject.WriteTo(jsonTextWriter);
                            jsonTextWriter.Flush();
                        }
                    });
        }

        private static JToken SerializeValue(object value)
        {
            var stringValue = value as String;
            if (stringValue != null)
            {
                return stringValue;
            }
            var arrayValue = value as string[];
            if (arrayValue != null)
            {
                return new JArray(arrayValue);
            }
            var compositeValue = value as CompositeData;
            if (compositeValue != null)
            {
                return new JObject(compositeValue.Properties.Select(x => new JProperty(x.Name, x.Value)));
            }
            var tabularValue = value as CompositeData[];
            if (tabularValue != null)
            {
                return new JArray(tabularValue.Select(r => new JObject(r.Properties.Select(x => new JProperty(x.Name, x.Value)))));
            }
            throw new NotSupportedException("Not supported value type: "+value.GetType().FullName);
        }
    }
}