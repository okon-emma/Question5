using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Question5.Rest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Question5.Controllers
{
    public class Country
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CountryController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult<Response> GetCountries()
        {
            XDocument db = XDocument.Load(_hostingEnvironment.ContentRootPath + @"\database.xml");

            //// IList Class Declaration
            IList<Country> countryIList = db.Element("Countries").Elements("Country").Select(i => new Country()
            {
                id = Convert.ToInt32(i.Attribute("id").Value),
                code = i.Attribute("code").Value,
                name = i.Attribute("name").Value
            }).ToList();

            return StatusCode(StatusCodes.Status200OK, new Response()
            {
                status = "success",
                message = "List of Countries",
                data = countryIList
            });
        }


        [HttpGet("{id}")]
        public ActionResult<Response> GetCountry([FromRoute] int id)
        {
            XDocument db = XDocument.Load(_hostingEnvironment.ContentRootPath + @"\database.xml");
            Country countryItem = db.Element("Countries").Elements("Country")
                .Where(i => Convert.ToInt32(i.Attribute("id").Value) == id)
                .Select(j => new Country()
                {
                    id = Convert.ToInt32(j.Attribute("id").Value),
                    code = j.Attribute("code").Value,
                    name = j.Attribute("name").Value
                }).FirstOrDefault();

            if (countryItem == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                {
                    status = "Error",
                    message = "No Data Found"
                });
            }

            return StatusCode(StatusCodes.Status200OK, new Response()
            {
                status = "success",
                message = "Single Country Info",
                data = countryItem
            });
        }

        [HttpPost]
        public ActionResult<Response> PostCountry([FromBody] Country requestBody)
        {
            if (requestBody == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                {
                    status = "Error",
                    message = "Bad Request"
                });
            }

            if (string.IsNullOrWhiteSpace(requestBody.code) || string.IsNullOrWhiteSpace(requestBody.name))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response()
                {
                    status = "Error",
                    message = "Empty Values"
                });
            }

            XDocument db = XDocument.Load(_hostingEnvironment.ContentRootPath + @"\database.xml");
            int objectCount = db.Element("Countries").Elements("Country").Count();
            objectCount++;

            Country data = new Country()
            {
                id = objectCount,
                code = requestBody.code,
                name = requestBody.name,
            };

            db.Element("Countries").Add(new XElement("Country", new XAttribute("id", data.id), new XAttribute("code", data.code), new XAttribute("name", data.name)));
            db.Save(_hostingEnvironment.ContentRootPath + @"\database.xml");

            return StatusCode(StatusCodes.Status200OK, new Response()
            {
                status = "success",
                message = "New country has been added",
                data = data
            });
        }
    }
}
