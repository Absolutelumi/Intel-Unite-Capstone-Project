using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using Intel.Unite.Common.Command;
using Intel.Unite.Common.Context;
using Intel.Unite.Common.Core;
using Intel.Unite.Common.Display;
using Intel.Unite.Common.Display.Hub;
using Intel.Unite.Common.Logging;
using Intel.Unite.Common.Manifest;
using Intel.Unite.Common.Module.Common;
using Intel.Unite.Common.Module.Feature.Hub;
using UnitePlugin.HubViewModel;
using UnitePluginTest.Views;

namespace UniteSketchpadPlugin
{
    public class SketchpadPluginHub : HubFeatureModuleBase
    {
        private string _html = @"<!DOCTYPE html>";

        private const string guid = "a9bbad72-eeb3-47cc-b147-345cc48738cf";
        private const string name = "Sketechpad Plugin Hub";
        private const string description = "Sketechpad Plugin Hub";
        private const string copyright = "Intel Corporation 2019";
        private const string vendor = "Intel Corporation";
        private const string version = "1.0.0.5";

        private const string minimumUniteVersion = "4.0.0.0";
        private const string entryPoint = "UnitePluginTest.dll";

        private readonly List<FrameworkElement> views = new List<FrameworkElement>();

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

        public override string HtmlUrlOrContent => _html;

        public override Dispatcher CurrentUiDispatcher { get; set; }

        public override ModuleManifest ModuleManifest => moduleManifest;

        public override ModuleInfo ModuleInfo => moduleInfo;

        public SketchpadPluginHub() : base()
        {

        }

        public SketchpadPluginHub(IModuleRuntimeContext runtimeContext) : base(runtimeContext)
        {
            FeatureModuleType = FeatureModuleType.Html;
        }

        public override void HubConnected(HubInfo hubInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void HubDisconnected(HubInfo hubInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void HubInfoChanged(HubInfo hubInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void IncomingMessage(Message message)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void Load()
        {
            // Populate views
            CurrentUiDispatcher.Invoke(() =>
            {
                foreach (var display in RuntimeContext.DisplayManager.AvailableDisplays)
                {
                    // TODO: Refactor such that this doesn't need to be done per view ???

                    var launchButtonView = new LaunchButtonView
                    {
                        DataContext = new HubViewModel
                        {
                            HubAllocationInfo = new HubAllocationInfo
                            {
                                FriendlyName = "LaunchButton",
                                ModuleOwnerId = ModuleInfo.Id,
                                PhysicalDisplay = display,
                                ViewType = HubDisplayViewType.QuickAccessAppIconView
                            }
                        }
                    };

                    views.Add(launchButtonView);
                }
            });

            // Allocate launch button views to hub
            // TODO: Pretty sure that allocating the views on a type basis is pointless ???
            // Ergo for now all views in list are allocated
            views.ForEach(view => AllocateView(view));

            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override bool OkToSleepDisplay()
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");

            return true;
        }

        public override void SessionKeyChanged(KeyValuePair sessionKey)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void Unload()
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void UserConnected(UserInfo userInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void UserDisconnected(UserInfo userInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        public override void UserInfoChanged(UserInfo userInfo)
        {
            RuntimeContext.LogManager.LogMessage(
                ModuleInfo.Id,
                LogLevel.Debug,
                MethodBase.GetCurrentMethod().Name,
                "Called");
        }

        private void AllocateView(FrameworkElement view)
        {
            HubViewModel hubViewModel = null;

            CurrentUiDispatcher.Invoke(delegate
            {
                hubViewModel = (HubViewModel)view.DataContext;
            });

            RuntimeContext.DisplayManager.AllocateUiInHubDisplayAsync(
                CreateContract(view),
                hubViewModel.HubAllocationInfo,
                hubViewModel.AllocatedCallBack
                );

        }
    }
}
