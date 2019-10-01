using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestTaskPerson.Api.DAL
{
    public class PersonLoader
    {
        public const int PersonCount = 30;
        public const string OpenApiUrl = "https://uinames.com/api/?amount=30&ext";

        private readonly PersonContext personContext;

        public PersonLoader(PersonContext personContext)
        {
            this.personContext = personContext;
        }

        private async Task RunAsync()
        {            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var stringTask = client.GetStringAsync(OpenApiUrl);
                var personDtos = JsonConvert.DeserializeObject<PersonDto[]>(await stringTask);
                var creditcardCache = personDtos.Select(x => x.CreditCard).ToList();
                var persons = MapDtoToEntity(personDtos, creditcardCache);

                personContext.AddRange(persons);
                personContext.SaveChanges();
            }  
        }

        private List<Person> MapDtoToEntity(PersonDto[] personDtos, List<CreditCardDto> creaditCardCache)
        {
            var rand = new Random();
            var persons = personDtos.Select(x => new Person
            {
                Name = x.Name,
                Surname = x.Surname,
                Birthday = x.Birthday.Date,
                CreditCards = creaditCardCache.Skip(rand.Next(0, PersonCount))
                    .Take(rand.Next(0, PersonCount)).Select(y => new CreditCard
                    {
                        Number = y.Number,
                        Pin = y.Pin,
                        Security = y.Security
                    }).ToList()
            });

            return persons.ToList();
        }

        public void LoadPersonsFromOpenApi()
        {
            // gets result propagates exception if exists.
            RunAsync().GetAwaiter().GetResult();
            RunAsync().GetAwaiter().GetResult(); 
        }
    }
}