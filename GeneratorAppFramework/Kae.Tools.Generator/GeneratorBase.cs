// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.Tools.Generator.Context;
using Kae.Tools.Generator.utility;
using Kae.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator
{
    public abstract class GeneratorBase : IGenerator
    {
        private Logger logger;
        private static string version = "0.0.1";
        private static string CIMOOAofOOADomainName = "OOAofOOA";
        public string Version { get; set; }

        protected IList<ContextParam> contextParams;
        public IList<ContextParam> ContextParams { get { return contextParams; } }

        private ColoringContext coloring;
        public ColoringRepository Coloring { get; set; }

        protected GenFolder genFolder;
        public GenFolder GenFolder { get { return genFolder; } }

        protected string OOAofOOAModelFilePath;
        protected string MetaDataTypeDefFilePath;
        protected string BaseDataTypeDefFilePath;
        protected string DomainModelFilePath;
        protected string GenFolderPath;

        protected bool resolvedContext = false;
        protected bool loadedMetaModel = false;
        protected bool loadedDomainModels = false;
        protected bool generatedEnvironment = false;

        protected XTUML.Tools.CIModelResolver.ConceptualInformationModelResolver modelResolver;

        public static readonly string CPKeyOOAofOOAModelFilePath = "metamodel-path";
        public static readonly string CPKeyMetaDataTypeDefFilePath = "meta-datatype-path";
        public static readonly string CPKeyBaseDataTypeDefFilePaht = "base-datatype-path";
        public static readonly string CPKeyDomainModelFilePath = "domainmodel-path";
        public static readonly string CPKeyGenFolderPath = "genpath";

        public GeneratorBase(Logger logger, string version = null)
        {
            if (!string.IsNullOrEmpty(version))
            {
                Version = version;
            }
            this.logger = logger;

            CreateRequiredContext();
            CreateAdditionalContext();
        }

        protected void CreateRequiredContext()
        {
            contextParams = new List<ContextParam>();

            var ooaOfOOAModelFilePath = new PathSelectionParam(CPKeyOOAofOOAModelFilePath) { IsFolder = false };
            var domainModelFilePath = new PathSelectionParam(CPKeyDomainModelFilePath) { IsFolder = true };
            var metaDataTypeDefFilePath = new PathSelectionParam(CPKeyMetaDataTypeDefFilePath) { IsFolder = false };
            var baseDataTypeDefFilePath = new PathSelectionParam(CPKeyBaseDataTypeDefFilePaht) { IsFolder = false };
            var genFolderPath = new PathSelectionParam(CPKeyGenFolderPath) { IsFolder = true };
            contextParams.Add(ooaOfOOAModelFilePath);
            contextParams.Add(domainModelFilePath);
            contextParams.Add(metaDataTypeDefFilePath);
            contextParams.Add(baseDataTypeDefFilePath);
            contextParams.Add(genFolderPath);
        }

        protected abstract void CreateAdditionalContext();



        public void ResolveContext()
        {
            var requiredContextParams = new List<string>() { CPKeyOOAofOOAModelFilePath, CPKeyBaseDataTypeDefFilePaht, CPKeyDomainModelFilePath, CPKeyGenFolderPath };
            foreach (var c in contextParams)
            {
                if (c.ParamName == CPKeyOOAofOOAModelFilePath)
                {
                    OOAofOOAModelFilePath = ((PathSelectionParam)c).Path;
                    if (!string.IsNullOrEmpty(OOAofOOAModelFilePath))
                    {
                        requiredContextParams.Remove(CPKeyOOAofOOAModelFilePath);
                    }
                }
                else if (c.ParamName == CPKeyMetaDataTypeDefFilePath)
                {
                    MetaDataTypeDefFilePath = ((PathSelectionParam)c).Path;
                }
                else if (c.ParamName == CPKeyDomainModelFilePath)
                {
                    DomainModelFilePath = ((PathSelectionParam)c).Path;
                    if (!string.IsNullOrEmpty(DomainModelFilePath))
                    {
                        requiredContextParams.Remove(CPKeyDomainModelFilePath);
                    }
                }
                else if (c.ParamName == CPKeyBaseDataTypeDefFilePaht)
                {
                    BaseDataTypeDefFilePath = ((PathSelectionParam)c).Path;
                    if (!string.IsNullOrEmpty(BaseDataTypeDefFilePath))
                    {
                        requiredContextParams.Remove(CPKeyBaseDataTypeDefFilePaht);
                    }
                }
                else if (c.ParamName == CPKeyGenFolderPath)
                {
                    GenFolderPath = ((PathSelectionParam)c).Path;
                    if (!string.IsNullOrEmpty(GenFolderPath))
                    {
                        genFolder = new GenFolder() { BaseFolder = GenFolderPath };
                        requiredContextParams.Remove(CPKeyGenFolderPath);
                    }
                }
            }
            if (requiredContextParams.Count > 0)
            {
                throw new ArgumentOutOfRangeException("required context parameters are missing!");
            }
            if (AdditionalWorkForResloveContext())
            {
                resolvedContext = true;
            }
        }
        protected abstract bool AdditionalWorkForResloveContext();

        public void LoadMetaModel()
        {
            if (resolvedContext)
            {
                modelResolver = new XTUML.Tools.CIModelResolver.ConceptualInformationModelResolver();
                modelResolver.LoadOOAofOOA(MetaDataTypeDefFilePath, OOAofOOAModelFilePath);
                logger.LogInfo($"Loaded ${OOAofOOAModelFilePath} as OOA of OOA model schmea");
                if (AdditionalWorkForMetaModel())
                {
                    loadedMetaModel = true;
                }
            }
            else
            {
                throw new ApplicationException("This method should be called after calling ResolveContext()!");
            }
        }
        protected abstract bool AdditionalWorkForMetaModel();

        public void LoadDomainModels()
        {
            if (loadedMetaModel)
            {
                string[] domainModels = { BaseDataTypeDefFilePath, DomainModelFilePath };
                modelResolver.LoadCIInstances(domainModels);
                logger.LogInfo($"Loaded Domain Models.");
                if (AdditionalWorkForDomainModels())
                {
                    loadedDomainModels = true;
                }
            }
            else
            {
                throw new ApplicationException("This method should be called after calling LoadMetaModel()!");
            }
        }
        protected abstract bool AdditionalWorkForDomainModels();

        /// <summary>
        /// Depricated methods. you should use GenerateEnvrionment and GenerateContent methods!
        /// </summary>
        [Obsolete("Please use GenerateEnvironment and GenerateContents.")]
        public void Generate()
        {
            if (!loadedDomainModels)
            {
                throw new ApplicationException("This method should be called after calling LoadDomainModels");
            }
            if (GenerateEnvironment())
            {
                GenerateContents();
            }
        }

        public bool GenerateEnvironment()
        {
            generatedEnvironment = false;
            if (loadedDomainModels)
            {
                generatedEnvironment = GenerateEnvironmentYourOwn();
            }
            return generatedEnvironment;
        }

        public void GenerateContents()
        {
            if (generatedEnvironment)
            {
                GenerateContentsYourOwn();
            }
        }


        protected abstract bool GenerateEnvironmentYourOwn();

        protected abstract void GenerateContentsYourOwn();

    }
}
