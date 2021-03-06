using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using productionorderservice.Model;
using productionorderservice.Services.Interfaces;
using productionorderservice.Validation;

namespace productionorderservice.Services {
    public class AssociateProductionOrderService : IAssociateProductionOrderService {
        private readonly IProductionOrderTypeService _productionOrderTypeService;
        private readonly IProductionOrderService _productionOrderService;
        private readonly IStateManagementService _stateManagementService;
        private readonly IThingGroupService _thingGroupService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient client = new HttpClient ();

        public AssociateProductionOrderService (IProductionOrderTypeService productionOrderTypeService,
            IProductionOrderService productionOrderService,
            IStateManagementService stateManagementService,
            IThingGroupService thingGroupService,
            IConfiguration configuration) {
            _configuration = configuration;
            _productionOrderTypeService = productionOrderTypeService;
            _productionOrderService = productionOrderService;
            _stateManagementService = stateManagementService;
            _thingGroupService = thingGroupService;
        }

        public async Task<(ProductionOrder, string)> AssociateProductionOrder (int thingId, int productioOrderId) {
            var PO = await _productionOrderService.getProductionOrder (productioOrderId);
            if (PO == null)
                return (null, "Production Order Not Found");
            if (PO.currentStatus != stateEnum.active.ToString ())
                return (null, "Production Order must be Active to Be set in Production");
            var POType = await _productionOrderTypeService.getProductionOrderType (PO.productionOrderTypeId.Value);
            if (POType == null)
                return (null, "Production Order Type Not Found");
            var POOnThing = await _productionOrderService.getProductionOrderOnThing (thingId);
            var thingGroups = POType.thingGroups;
            bool contains = false;
            foreach (var group in thingGroups) {
                var (completeGroup, status) = await _thingGroupService.getGroup (group.thingGroupId);
                if (status == HttpStatusCode.OK)
                    if (completeGroup.things.Select (x => x.thingId).Contains (thingId) == true)
                        contains = true;
            }
            if (!contains)
                return (null, "This Production Order can't  be associated with this thing.");
            await _productionOrderService.setProductionOrderToThing (PO, thingId);
            UpdateStatusAPI (POType.typeScope, POType.typeDescription, "productionOrderNumber", PO.productionOrderNumber, thingId);
            Trigger (PO);
            PO = await _productionOrderService.getProductionOrder (productioOrderId);
            return (PO, "Production Order Set to Thing");
        }

        public async Task<(ProductionOrder, string)> DisassociateProductionOrder (ProductionOrder productionOrder) {
            var PODB = await _productionOrderService.getProductionOrder (productionOrder.productionOrderId);
            if (PODB == null)
                return (null, "Production Order Not Found");
            if (PODB.currentStatus != stateEnum.active.ToString ())
                return (null, "Production Order Not Active");
            var POType = await _productionOrderTypeService.getProductionOrderType (productionOrder.productionOrderTypeId.Value);
            if (POType == null)
                return (null, "Production Order Type Not Found");
            UpdateStatusAPI (POType.typeScope, POType.typeDescription, "productionOrderNumber", "", PODB.currentThingId.Value);
            await _productionOrderService.setProductionOrderToThing (PODB, null);
            Trigger (PODB);
            PODB = await _productionOrderService.getProductionOrder (productionOrder.productionOrderId);
            return (PODB, "Production Order Disassociated");
        }

        private async void UpdateStatusAPI (string context, string contextDescription,
            string statusName, string value, int thingId) {
            try {
                if (_configuration["stateServiceEndpoint"] != null) {
                    dynamic state = new JObject ();
                    state.context = context;
                    state.contextDescription = contextDescription;
                    state.statusName = statusName;
                    state.value = value;
                    var json = JsonConvert.SerializeObject (state);
                    client.DefaultRequestHeaders.Accept.Clear ();
                    client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
                    var content = new StringContent (json, Encoding.UTF8, "application/json");
                    var url = _configuration["stateServiceEndpoint"] + "/api/contextstatus/" + thingId + "/" + context + "?recurrent=true";
                    HttpResponseMessage response = await client.PutAsync (url, content);
                    if (response.IsSuccessStatusCode) {
                        Console.WriteLine ("Data posted on State API");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine (ex.Message);
            }
        }

        private async void Trigger (ProductionOrder productionOrder) {
            try {
                if (_configuration["AssociationPostEndpoint"] != null) {
                    client.DefaultRequestHeaders.Accept.Clear ();
                    client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
                    var content = new StringContent (JsonConvert.SerializeObject (productionOrder), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync (_configuration["AssociationPostEndpoint"], content);
                    if (response.IsSuccessStatusCode) {
                        Console.WriteLine ("Data posted on AssociationPostEndpoint");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine (ex.Message);
            }
        }

    }
}