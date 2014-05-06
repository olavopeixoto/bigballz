using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace BigBallz.Services
{
    public static class CronJob
    {
        private static CacheItemRemovedCallback _onCacheRemove;

        private const string CronJobCachePrefix = "CronJobTask|";

        public static void AddTask(ICronJobTask cronJobTask)
        {
            _onCacheRemove = CacheItemRemoved;
            if (cronJobTask.AbsoluteExpiration.HasValue)
            {
                var gmtTimeZone = DateTime.Now.Subtract(DateTime.UtcNow).TotalHours + 3;
                var expirationTime = cronJobTask.AbsoluteExpiration.Value.AddHours(gmtTimeZone);

                var cacheKey = CronJobCachePrefix + cronJobTask.Name + "|" + expirationTime.Ticks;

                if (HttpRuntime.Cache[cacheKey] == null)
                {
                    HttpRuntime.Cache.Insert(cacheKey, cronJobTask, null,
                                             expirationTime, Cache.NoSlidingExpiration,
                                             CacheItemPriority.NotRemovable, _onCacheRemove);
                }
            }
            else if (cronJobTask.SlidingExpiration.HasValue)
            {
                var cacheKey = CronJobCachePrefix + cronJobTask.Name + "|" + cronJobTask.SlidingExpiration.Value.Ticks;

                if (HttpRuntime.Cache[cacheKey] == null)
                {
                    HttpRuntime.Cache.Insert(cacheKey, cronJobTask,
                                             null,
                                             Cache.NoAbsoluteExpiration, cronJobTask.SlidingExpiration.Value,
                                             CacheItemPriority.NotRemovable, _onCacheRemove);
                }
            }
        }

        private static void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            var task = v as ICronJobTask;
            if (task != null)
            {
                if (r == CacheItemRemovedReason.Expired)
                {
                    task.Run();
                    if (task.Recurring) AddTask(task);
                }
                else
                {
                    AddTask(task);
                }
            }
        }

        public static ICronJobTask[] GetScheduledTasks()
        {
            var tasks = new List<ICronJobTask>();

            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLowerInvariant().StartsWith(CronJobCachePrefix.ToLowerInvariant()))
                {
                    tasks.Add(enumerator.Value as ICronJobTask);
                }
            }

            return tasks.OrderBy(x => x.AbsoluteExpiration).ToArray();
        }
    }
}