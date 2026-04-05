using Microsoft.AspNetCore.Mvc;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;
using TheNexusAPI.Services;

namespace TheNexusAPI.Controllers
{
    public class ChangeLogController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ChangeLogService _changeLogService;

        public ChangeLogController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _changeLogService = new ChangeLogService(_dataContext);
        }

        #region Get
        // Get all ChangeLog entries
        [HttpGet]
        public List<ChangeLog> GetAllChangeLogs()
        {
            return _changeLogService.GetAllChangeLogs();
        }

        public ChangeLog GetChangeLogByChangeLogId(int id)
        {
            var changeLog = _changeLogService.GetChangeLogByChangeLogId(id);
            if (changeLog == null)
            {
                throw new Exception("ChangeLog not found");
            }
            return changeLog;
        }
        #endregion

        #region Add
        public ChangeLog AddChangeLog(ChangeLog newChangeLog)
        {
            return _changeLogService.AddChangeLog(newChangeLog);
        }
        #endregion
    }
}
