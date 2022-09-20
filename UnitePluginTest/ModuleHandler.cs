using Intel.Unite.Common.Command;
using Intel.Unite.Common.Context;
using Intel.Unite.Common.Core;
using Intel.Unite.Common.Manifest;
using Intel.Unite.Common.Module.Common;
using Intel.Unite.Common.Module.Feature.Hub;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace UnitePluginTest
{
    public class ModuleHandler : HubFeatureModuleBase
    {
        private const string guid = "a9bbad72-eeb3-47cc-b147-345cc48738cf";
        private const string name = "Unite Plugin Example";
        private const string description = "Unite Plugin Example";
        private const string copyright = "Intel Corporation 2019";
        private const string vendor = "Intel Corporation";
        private const string version = "1.0.0.2";

        private const string minimumUniteVersion = "4.0.0.0";
        private const string entryPoint = "UnitePluginTest.dll";

        private static readonly ModuleInfo moduleInfo = new ModuleInfo
        {
            ModuleType = ModuleType.Feature,
            Id = Guid.Parse(guid),
            Name = name,
            Description = description,
            Copyright = copyright,
            Vendor = vendor,
            Version = Version.Parse(version),
            SupportedPlatforms = ModuleSupportedPlatform.Mac | ModuleSupportedPlatform.Windows | ModuleSupportedPlatform.Android
        };

        private static readonly ManifestOsSet files = new ManifestOsSet
        {
            Windows = new Collection<ManifestFile>
            {
                new ManifestFile()
                {
                    SourcePath = entryPoint,
                    TargetPath = entryPoint
                }
            }
        };

        private static readonly ModuleManifest moduleManifest = new ModuleManifest
        {
            Owner = UniteModuleOwner.Hub,
            ModuleId = moduleInfo.Id,
            Name = new MultiLanguageString(moduleInfo.Name),
            Description = new MultiLanguageString(moduleInfo.Description),
            ModuleVersion = moduleInfo.Version,
            MinimumUniteVersion = Version.Parse(minimumUniteVersion),
            Settings = new Collection<ConfigurationSetting>(),
            Files = files,
            Installers = new Collection<ManifestInstaller>(),
            EntryPoint = entryPoint,
            ModuleType = moduleInfo.ModuleType,
        };

        public override string HtmlUrlOrContent => throw new System.NotImplementedException();

        public override Dispatcher CurrentUiDispatcher { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override ModuleManifest ModuleManifest => moduleManifest;

        public override ModuleInfo ModuleInfo => moduleInfo;

        public ModuleHandler()
        {

        }

        public ModuleHandler(IModuleRuntimeContext runtimeContext) : base(runtimeContext)
        {

        }

        public override void HubConnected(HubInfo hubInfo)
        {
            throw new System.NotImplementedException();
        }

        public override void HubDisconnected(HubInfo hubInfo)
        {
            throw new System.NotImplementedException();
        }

        public override void HubInfoChanged(HubInfo hubInfo)
        {
            throw new System.NotImplementedException();
        }

        public override void IncomingMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

        public override void Load()
        {
            throw new System.NotImplementedException();
        }

        public override bool OkToSleepDisplay()
        {
            throw new System.NotImplementedException();
        }

        public override void SessionKeyChanged(KeyValuePair sessionKey)
        {
            throw new System.NotImplementedException();
        }

        public override void Unload()
        {
            throw new System.NotImplementedException();
        }

        public override void UserConnected(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }

        public override void UserDisconnected(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }

        public override void UserInfoChanged(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
