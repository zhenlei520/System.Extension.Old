using System;
using System.Collections.Generic;
using System.Linq;
using EInfrastructure.Infrastructure.Interfaces;

namespace EInfrastructure.Ddd.Config
{
    /// <summary>
    /// 全局启动任务
    /// </summary>
    public class StartTaskRegister
    {
        /// <summary>
        /// 开始任务
        /// </summary>
        public static void StartTask()
        {
            InitConfig();//初始化配置
            StartGlobleTask();
        }

        #region 初始化配置
        /// <summary>
        /// 初始化配置
        /// </summary>
        private static void InitConfig()
        {
        }
        #endregion

        #region 运行继承IStartupTask类的方法
        /// <summary>
        /// 运行继承IStartupTask类的方法
        /// </summary>
        private static void StartGlobleTask()
        {
            var taskAssignableList =
                AppDomain.CurrentDomain.GetAssemblies().ToArray().SelectMany(
                    x =>
                        x.GetTypes()).Where(x => typeof(IStartupTask).IsAssignableFrom(x) && x != (typeof(IStartupTask)));
            List<IStartupTask> taskList = new List<IStartupTask>();
            foreach (var taskAssignable in taskAssignableList)
            {
                taskList.Add(Activator.CreateInstance(taskAssignable) as IStartupTask);
            }
            foreach (var taskInfo in taskList.OrderBy(x => x.GetOrder()))
            {
                taskInfo.Execute();
            }
        }
        #endregion
    }
}