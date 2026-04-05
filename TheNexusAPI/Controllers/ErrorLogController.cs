using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    public class ErrorLogController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ErrorLogService _errorLogService;

        public ErrorLogController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _errorLogService = new ErrorLogService(_dataContext);
        }

        #region Get
        // Get all ErrorLog entries
        [HttpGet]
        public List<ErrorLog> GetAllErrorLogs()
        {
            return _errorLogService.GetAllErrorLogs();
        }

        public ErrorLog GetErrorLogByErrorLogId(int id)
        {
            var errorLog = _errorLogService.GetErrorLogByErrorLogId(id);
            if (errorLog == null)
            {
                throw new Exception("ErrorLog not found");
            }
            return errorLog;
        }
        #endregion

        #region Add
        public ErrorLog AddErrorLog(ErrorLog newErrorLog)
        {
            return _errorLogService.AddErrorLog(newErrorLog);
        }
        #endregion
    }
}