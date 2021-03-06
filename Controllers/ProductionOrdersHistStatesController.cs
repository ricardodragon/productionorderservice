using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using productionorderservice.Model;
using productionorderservice.Services.Interfaces;

namespace productionorderservice.Controllers
{
    [Route("api/[controller]")]
    public class ProductionOrdersHistStatesController : Controller
    {
        private readonly IHistStateService _histStatesService;

        public ProductionOrdersHistStatesController(IHistStateService histStatesService)
        {
            _histStatesService = histStatesService;
        } 

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int productionOrderId)
        {
            if(productionOrderId > 0)
            {
                var histStates = await _histStatesService.getHistStates(productionOrderId);

                if(histStates != null)
                {
                    return Ok(histStates);
                }
            }
            return NotFound();
        }

        [HttpGet("productionOrderList")]
        public async Task<IActionResult> Get([FromQuery]string statusSearch, [FromQuery]long startDate, [FromQuery]long endDate)
        {
            if(!string.IsNullOrEmpty(statusSearch))
            {
                var productionOrderIds = await _histStatesService.getHistStatesPerStatusAndDate(statusSearch,startDate,endDate);

                if(productionOrderIds.Count() != 0)
                {
                    return Ok(productionOrderIds);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] int productionOrderId,[FromQuery]string state)
        {
            if(productionOrderId>0)
            {
                var histstate = await _histStatesService.addHistStates(productionOrderId,state);
                if(histstate == null)
                {
                    return StatusCode(500);
                }
                return Created($"api/ProductionOrdersHistStates/{histstate.histStatesId}", histstate);
            }
            return BadRequest("ProductionOrderId invalid");
        }
        
    }
}