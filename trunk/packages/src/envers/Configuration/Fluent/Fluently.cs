using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Configuration.Fluent
{
    public static class Fluently
    {
        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <returns>Fluent Configuration</returns>
        public static FluentConfiguration Configure()
        {
            return new FluentConfiguration();
        }

        /// <summary>
        /// Begin fluently configuring NHibernate
        /// </summary>
        /// <param name="cfg">Instance of an NHibernate Configuration</param>
        /// <returns>Fluent Configuration</returns>
        //public static FluentConfiguration Configure(NHibernate.Cfg.Configuration cfg)
        //{
        //    return new FluentConfiguration(cfg);
        //}
    }
}
