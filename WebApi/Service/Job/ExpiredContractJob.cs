using Microsoft.Extensions.Options;
using Quartz;
using WebApi.Configs;
using WebApi.Service.Admin;

namespace WebApi.Service.Job
{
    public class ExpiredContractJob : IJob
    {
        private readonly ILogger<ExpiredContractJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly JobSettings _settings;

        public ExpiredContractJob(ILogger<ExpiredContractJob> logger,
            IServiceScopeFactory scopeFactory, IOptions<JobSettings> appSettings)
        {
            _scopeFactory = scopeFactory;
            _settings = appSettings.Value;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (_settings.ExpiredJob != null)
            {
                using (var serviceScope = _scopeFactory.CreateScope())
                {
                    _logger.LogInformation($"ExpiredContractJob >> START");
                    var _contractService = serviceScope.ServiceProvider.GetService<ContractsManagementService>();

                    try
                    {
                        await _contractService.ExpiredContract();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"ExpiredContractJob >>" + ex.ToString());
                    }
                    _logger.LogInformation($"ExpiredContractJob >> DONE");
                }
            }
        }
    }
}
