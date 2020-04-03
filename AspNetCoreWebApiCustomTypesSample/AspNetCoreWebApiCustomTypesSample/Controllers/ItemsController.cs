using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreWebApiCustomTypesSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ItemsController : ControllerBase
    {
        private static readonly Model[] _items = new Model[]
        {
            new Model{ Date = new Date(2020, 1, 10) },
            new Model{ Date = new Date(2020, 4, 1) },
            new Model{ Date = new Date(2020, 4, 11) },
            new Model{ Date = new Date(2020, 4, 13) },
        };

        /// <summary>
        /// Retrieves an array of items for a specific date.
        /// </summary>
        /// <param name="date">The date of the items to be retrieved.</param>
        /// <returns>An array of items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Model[]), StatusCodes.Status200OK)]
        public IEnumerable<Model> Get([BindRequired, FromQuery(Name = "date")]Date date) =>
            _items.Where(x => x.Date.Equals(date)).ToArray();
    }
}