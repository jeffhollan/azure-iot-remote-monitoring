using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository
{
    /// <summary>
    /// Repository storing available actions for rules.
    /// </summary>
    public class ActionRepository : IActionRepository
    {
        // Currently this list is not editable in the app
        private Dictionary<string,string> _actionIds = new Dictionary<string, string>()
        {
            { "Send Message", "https://prod-07.eastus.logic.azure.com:443/workflows/037b0fdb254d4b359de46dda62ca8503/triggers/manual/run?api-version=2015-08-01-preview&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=hqLBsRShN0Uf8UwRW9zOf-MkPpj-yaz5E8aL90XrMUI" },
            { "Raise Alarm", "https://prod-07.eastus.logic.azure.com:443/workflows/037b0fdb254d4b359de46dda62ca8503/triggers/manual/run?api-version=2015-08-01-preview&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=hqLBsRShN0Uf8UwRW9zOf-MkPpj-yaz5E8aL90XrMUI" }
        };

        public async Task<List<string>> GetAllActionIdsAsync()
        {
            return await Task.Run(() => { return new List<string>(_actionIds.Keys); });
        }

        public async Task<bool> ExecuteLogicAppAsync(string actionId, string deviceId, string measurementName, double measuredValue)
        {
            return await Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(_actionIds[actionId],
                        new StringContent(JsonConvert.SerializeObject(
                            new
                            {
                                deviceId = deviceId,
                                measurementName = measurementName,
                                measuredValue = measuredValue
                            }),
                            System.Text.Encoding.UTF8,
                            "application/json"));

                    return response.IsSuccessStatusCode;
                }
            });
            
        }
    }
}
