using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TestTaskPerson.Api.DAL
{
    /// <summary>
    /// Uses to transfer data from open Api to app storage.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PersonDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("birthday", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public BirthdayDto Birthday { get; set; }

        [JsonProperty("credit_card", ItemTypeNameHandling = TypeNameHandling.Auto)]
        public CreditCardDto CreditCard { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class BirthdayDto
    {
        [JsonProperty("mdy")]
        public DateTime? Date { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CreditCardDto
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("security")]
        public string Security { get; set; }
    }
}