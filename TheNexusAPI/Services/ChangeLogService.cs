using System.Reflection;
using TheNexusAPI.Data;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Services
{
    public class ChangeLogService
    {
        private readonly DataContext _dataContext;

        public ChangeLogService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<ChangeLog> GetAllChangeLogs()
        {
            return _dataContext.ChangeLog.ToList();
        }

        public ChangeLog? GetChangeLogByChangeLogId(int changeLogId)
        {
            return _dataContext.ChangeLog.FirstOrDefault(item => item.ChangeLogId == changeLogId);
        }

        public ChangeLog AddChangeLog(ChangeLog newChangeLog)
        {
            _dataContext.ChangeLog.Add(newChangeLog);
            _dataContext.SaveChanges();
            return newChangeLog;
        }

        public void GenericAddToChangeLog(string tableChanged, int tableChangedPrimaryId, string? originalValueString = "", string? newValueString = "")
        {
            ChangeLog changeLog = new ChangeLog
            {
                ChangedTable = tableChanged,
                ChangedTablePrimaryId = tableChangedPrimaryId,
                OldValue = originalValueString,
                NewValue = newValueString,
                ChangeDateTime = DateTime.UtcNow,
                ChangeUserId = 0
            };

            AddChangeLog(changeLog);
        }

        public ChangeLog ConvertChangesForLogging<T>(T? originalEntity, T? newEntity) where T : class
        {
            var changeLog = new ChangeLog();

            if (originalEntity == null && newEntity == null)
                return changeLog;

            // Determine the runtime type to inspect
            Type entityType = (originalEntity ?? newEntity)!.GetType();

            // Optional: record which table/entity changed
            changeLog.ChangedTable = entityType.Name;

            // Use EF Core metadata to find primary key property name(s)
            var efEntityType = _dataContext.Model.FindEntityType(entityType);
            var primaryKeyProps = efEntityType?.FindPrimaryKey()?.Properties;
            string? primaryKeyName = primaryKeyProps?.FirstOrDefault()?.Name;

            // Try to obtain the primary key value (prefer originalEntity, fallback to newEntity)
            int? primaryKeyValue = null;
            if (!string.IsNullOrEmpty(primaryKeyName))
            {
                var pkProp = entityType.GetProperty(primaryKeyName, BindingFlags.Public | BindingFlags.Instance);
                object? pkValObj = pkProp != null
                    ? (originalEntity != null ? pkProp.GetValue(originalEntity) : pkProp.GetValue(newEntity))
                    : null;

                if (pkValObj != null && int.TryParse(pkValObj.ToString(), out var parsed))
                {
                    primaryKeyValue = parsed;
                }
            }

            var propertyInfo = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in propertyInfo)
            {
                var originalValue = originalEntity is null ? null : prop.GetValue(originalEntity);
                var newValue = newEntity is null ? null : prop.GetValue(newEntity);

                if (!Equals(originalValue, newValue))
                {
                    changeLog.OldValue += $"{prop.Name}: {originalValue}; ";
                    changeLog.NewValue += $"{prop.Name}: {newValue}; ";

                    // If there are changes, persist a change log entry using the primary key if found
                    if (!string.IsNullOrEmpty(changeLog.OldValue) && primaryKeyValue.HasValue)
                    {
                        GenericAddToChangeLog(changeLog.ChangedTable, primaryKeyValue.Value, (string?)originalValue, (string?)newValue);
                    }
                }
            }

            return changeLog;
        }
    }
}
