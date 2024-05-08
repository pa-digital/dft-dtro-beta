using DfT.DTRO.Converters;
using DfT.DTRO.Models.DtroJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dft.DTRO.Tests.CodeiumTests.JsonConverters
{
    public class BoundingBoxJsonConverterTests
    {
        private JsonSerializer serializer;


        public BoundingBoxJsonConverterTests()
        {
            serializer = new JsonSerializer();
            serializer.Converters.Add(new BoundingBoxJsonConverter());
        }

        [Fact]
        public void WriteJson_ShouldSerializeBoundingBoxToJson()
        {
            var boundingBox = new BoundingBox(1.0, 2.0, 3.0, 4.0);
            var writer = new StringWriter();
            serializer.Serialize(writer, boundingBox);

            Assert.Equal("[1.0,2.0,3.0,4.0]", writer.ToString());
        }

        [Fact]
        public void ReadJson_ShouldDeserializeJsonToBoundingBox()
        {
            var json = "[1.0,2.0,3.0,4.0]";
            var reader = new JsonTextReader(new StringReader(json));
            var boundingBox = serializer.Deserialize<BoundingBox>(reader);

            Assert.Equal(1.0, boundingBox.westLongitude);
            Assert.Equal(2.0, boundingBox.southLatitude);
            Assert.Equal(3.0, boundingBox.eastLongitude);
            Assert.Equal(4.0, boundingBox.northLatitude);
        }

        [Fact]
        public void ReadJson_ShouldThrowExceptionForInvalidJson()
        {
            var json = "[1.0,2.0,3.0]";
            var reader = new JsonTextReader(new StringReader(json));

            Assert.Throws<JsonReaderException>(() => serializer.Deserialize<BoundingBox>(reader));
        }
    }
}