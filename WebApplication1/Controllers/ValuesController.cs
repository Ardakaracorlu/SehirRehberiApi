using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DataContext _Context;

        public ValuesController(DataContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetValues()
        {
            var values = await _Context.Values.ToListAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetValue (int id)
        {
            var value = await _Context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

    }
}
