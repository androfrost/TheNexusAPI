using System.Reflection;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class ErrorLogService
    {
        private readonly DataContext _dataContext;

        public ErrorLogService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<ErrorLog> GetAllErrorLogs()
        {
            return _dataContext.ErrorLog.ToList();
        }

        public ErrorLog? GetErrorLogByErrorLogId(int errorLogId)
        {
            return _dataContext.ErrorLog.FirstOrDefault(item => item.ErrorLogId == errorLogId);
        }

        public ErrorLog AddErrorLog(ErrorLog newErrorLog)
        {
            _dataContext.ErrorLog.Add(newErrorLog);
            _dataContext.SaveChanges();
            return newErrorLog;
        }

        public void GenericAddToErrorLog(string processName, string errorAction, string errorMessage)
        {
            ErrorLog errorLog = new ErrorLog
            {
                Source = processName,
                TargetSite = errorAction,
                Message = errorMessage,
                ErrorDateTime = DateTime.UtcNow,
                UserId = 0
            };

            AddErrorLog(errorLog);
        }

        public void GenericAddToErrorLog(Exception ex, int userId = 0)
        {
            ErrorLog errorLog = new ErrorLog
            {
                Source = ex.Source ?? "",
                TargetSite = ex.TargetSite?.Name ?? "",
                Message = ex.Message,
                ErrorDateTime = DateTime.UtcNow,
                UserId = userId
            };

            AddErrorLog(errorLog);
        }
    }
}
