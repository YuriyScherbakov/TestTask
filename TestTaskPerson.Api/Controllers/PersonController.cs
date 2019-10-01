using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestTaskPerson.Api.DAL;
using System.Linq;

namespace TestTaskPerson.Api.Controllers
{   
    [Route("api/persons")]
    public class PersonController : ControllerBase
    {
        PersonContext personContext;
        public PersonController(PersonContext personContext)
        {
            this.personContext = personContext;
        }

        [HttpGet("get-all")]
        public IEnumerable<Person> GetAll()
        {
            return  personContext.Persons.ToList();
            
        }

        /// <summary>
        /// Gets only duplicate names. 
        /// if duplicateCount = 0 returns only unique person names. 
        /// In other CASES: if duplicate Count > 0 returns duplicate names with defined matches.
        /// </summary>
        /// <param name="duplicateCount">0 or greater than 0</param>
        /// <returns>array of duplicate names</returns>
        [HttpGet("get-duplicate-names/{duplicateCount}")]
        public IEnumerable<string> GetDuplicateNames([FromRoute]int duplicateCount = 0)
        {


            var grouppedNames = from p in personContext.Persons
                                group p by p.Name into grp
                                select new {
                                    name = grp.Key, cnt = grp.Count(), person = grp.Select(p=>p)
                                };
            return grouppedNames.Where(dc => dc.cnt == duplicateCount+1).Select(p=>p.name);
            // TODO:
            // Implement logic which provides two different modes: 
            // 1) gets only unique names; 
            // 2) gets only duplicate names with defined matches (with duplicateCount);
        }

        /// <summary>
        /// 
        /// It should search by base fields and collection fields (like CreditCards) too.
        /// Gets persons by some criteria: Name, Surname, CreditCard_Number, CreditCard_Pin.
        /// Search works in several modes: by Contains, by Equals operator.
        /// </summary>
        /// <param name="fieldName">Name, Surname, CreditCard_Number, CreditCard_Pin</param>
        /// <param name="fieldValue">Alice, Tom</param>
        /// <param name="isContains"> true -> 'Contains'; false -> 'Equals' </param>
        /// <returns></returns>
        [HttpGet("get-by-filter")]
        public IEnumerable<Person> GetByFilter([FromQuery]string fieldName, [FromQuery]string fieldValue, [FromQuery]bool isContains)
        {
            // TODO:
            //Person property(field) names: (Name, Surname, CreditCards.Number, CreditCard.Pin, etc)
            // Implement logic where persons will be found by all person fields (including collection fields).
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets paged person list with total count.
        /// Implement 
        /// For example:
        /// result: { Items: [{name:test, surname:test}, {name:test1, surname:test1}], Total: 300 }
        /// request: { SortedFieldName : Surname, Asc: true, Page: 5, PageSize : 10 } 
        /// </summary>
        /// <param name="request">PagedRequest</param>
        /// <returns>DataSourceResult</returns>
        [HttpPost("get-paged-list")]
        public DataSourceResult<Person> GetPagedQuery([FromBody]PagedRequest request)
        {
            // TODO:
            // Add paging, sorting, ordering functionality.
            throw new NotImplementedException();
        }
    }
}
