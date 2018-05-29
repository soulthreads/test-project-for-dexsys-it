using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StandApi.Interfaces;
using StandApi.Models;

namespace StandApi.Controllers
{
    [Route("[controller]")]
    public class StandController : Controller
    {
        private readonly IStandRepository _standRepository;

        public StandController(IStandRepository standRepository)
        {
            _standRepository = standRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _standRepository.GetAllEntries());
        }

        [HttpGet("now/{url}", Name = "GetStandLastEntry")]
        public async Task<IActionResult> GetLastByUrl(string url)
        {
            try
            {
                var entry = await _standRepository.GetLastEntryForStand(url);
                if (entry != null)
                    return Ok(entry);
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("{url}", Name = "GetStandEntries")]
        public async Task<IActionResult> GetStandEntries(string url)
        {
            try
            {
                return Ok(await _standRepository.GetAllEntriesForStand(url));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> AddEntry([FromBody] StandEntry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (entry == null)
            {
                return BadRequest();
            }

            try
            {
                _standRepository.AddEntry(entry);
                await _standRepository.SaveChanges();

                return CreatedAtRoute("GetStandLastEntry", new { url = entry.Url }, entry);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpPut("register")]
        public async Task<IActionResult> RegisterStand([FromBody, Bind("Url,Name")] Stand stand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            if (stand == null)
            {
                return BadRequest();
            }

            _standRepository.AddStand(stand);
            await _standRepository.SaveChanges();

            return NoContent();
        }
    }
}
