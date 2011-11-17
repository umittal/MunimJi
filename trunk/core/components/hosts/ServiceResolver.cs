using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;
using munimji.core.lib.extentions;
using munimji.core.worker.configuration;

namespace munimji.core.components.hosts {
    internal static class ServiceResolver {
        internal static IEnumerable<Type> Resolve() {
            switch (GetConfiguredServiceType()) {
                case ServiceTypes.Wcf:
                    return WcfResolver();
                case ServiceTypes.Worker:
                    return WorkerResolver();
                default:
                    throw new NotSupportedException();
            }
        }

        #region Wcf

        private static IEnumerable<Type> WcfResolver() {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var wcfConfiguration = ServiceModelSectionGroup.GetSectionGroup(config);
            if (wcfConfiguration != null && wcfConfiguration.Services.Services.Count > 0) {
                foreach (ServiceElement service in wcfConfiguration.Services.Services) {
                    foreach (ServiceEndpointElement endpoint in service.Endpoints) {
                        if (endpoint.Contract.ToUpper() == "IMETADATAEXCHANGE") {
                            continue;
                        }
                        var contractType = GetContractType(endpoint.Contract);
                        if (contractType == null) {
                            throw new TypeLoadException(string.Format("Cannot resolve type for contract '{0}'",
                                                                      endpoint.Contract));
                        }
                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                            foreach (var type in assembly.GetExportedTypes()) {
                                var criteria = type.GetInterface(contractType.FullName, true);
                                if (criteria == null) {
                                    continue;
                                }
                                yield return type;
                            }
                        }
                    }
                }
            }
        }

        private static Type GetContractType(string typeName) {
            Type contractType = null;

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path.IsNotEmpty()) {
                var refAssemblies = Directory.GetFiles(path, "*.dll");
                foreach (var refAssemblyName in from refAssemblyName in refAssemblies
                                                let refAssembly = Assembly.ReflectionOnlyLoadFrom(refAssemblyName)
                                                let reflectedType = refAssembly.GetType(typeName, false, true)
                                                where reflectedType != null
                                                select refAssemblyName) {
                    Assembly.LoadFrom(refAssemblyName);
                }
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                contractType = assembly.GetType(typeName, false, true);
                if (contractType != null) {
                    break;
                }
            }

            return contractType;
        }

        #endregion

        #region Worker

        private static IEnumerable<Type> WorkerResolver() {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var workerConfig = WorkerModelSection.GetSection(config);
            if (workerConfig != null && workerConfig.Workers.Count > 0) {
                foreach (WorkerElement worker in workerConfig.Workers) {
                    var workerType = Type.GetType(worker.Implementation);
                    yield return workerType;
                }
            }
        }

        #endregion

        internal static ServiceTypes GetConfiguredServiceType() {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var wcfConfiguration = ServiceModelSectionGroup.GetSectionGroup(config);
            if (wcfConfiguration != null && wcfConfiguration.Services.Services.Count > 0) {
                return ServiceTypes.Wcf;
            }
            var workerConfig = WorkerModelSection.GetSection(config);
            if (workerConfig != null && workerConfig.Workers.Count > 0) {
                return ServiceTypes.Worker;
            }
            throw new NotSupportedException();
        }
    }
}