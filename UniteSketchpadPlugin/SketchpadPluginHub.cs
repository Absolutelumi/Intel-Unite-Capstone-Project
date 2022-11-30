using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
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
using UniteSketchpadPlugin.ViewModels;
using UniteSketchpadPlugin.Views;
using Point = System.Drawing.Point;

namespace UniteSketchpadPlugin
{
    public class SketchpadPluginHub : HubFeatureModuleBase
    {
        private const string _html = @"<!DOCTYPE html>";

        private const string guid = "a9bbad72-eeb3-47cc-b147-345cc48738cf";
        private const string name = "Sketechpad Plugin Hub";
        private const string description = "Sketechpad Plugin Hub";
        private const string copyright = "Intel Corporation 2019";
        private const string vendor = "Intel Corporation";
        private const string version = "1.0.7.4";

        private const string minimumUniteVersion = "4.0.0.0";
        private const string entryPoint = "UnitePluginTest.dll";

        internal CanvasManager canvasManager;
        internal readonly List<FrameworkElement> views = new List<FrameworkElement>();

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
            // Populate Views / Canvas Managers
            CurrentUiDispatcher.Invoke(() =>
            {
                foreach (var display in RuntimeContext.DisplayManager.AvailableDisplays)
                {
                    // Initiate CanvasManager for this display
                    if (canvasManager == null)
                    {
                        canvasManager = new CanvasManager(display.Size.Width, display.Size.Height);
                    }

                    Func<Point, Image> onPress = (point) => { return canvasManager.OnPress(point); };
                    Func<Point, Image> onPressMove = (point) => { return canvasManager.OnPressMove(point); };
                    Func<Point, Image> onPressRelease = (point) => { return canvasManager.OnRelease(point); };

                    var canvasView = new CanvasView(
                        onPress, 
                        onPressMove, 
                        onPressRelease)
                    {
                        DataContext = new HubViewModel
                        {
                            HubAllocationInfo = new HubAllocationInfo
                            {
                                FriendlyName = "Canvas",
                                ModuleOwnerId = ModuleInfo.Id,
                                PhysicalDisplay = display,
                                ViewType = HubDisplayViewType.PresentationView
                            }
                        }
                    };

                    Action<CanvasManager.Settings> onSettingsUpdate = (settings) => { canvasView.UpdateCanvasImage(canvasManager.SetSettings(settings)); };

                    var canvasControlsView = new CanvasControlsView(canvasManager.GetSettings(), onSettingsUpdate)
                    {
                        DataContext = new HubViewModel
                        {
                            HubAllocationInfo = new HubAllocationInfo
                            {
                                FriendlyName = "CanvasControls",
                                ModuleOwnerId = ModuleInfo.Id,
                                PhysicalDisplay = display,
                                ViewType = HubDisplayViewType.PresentationRibbonView
                            }
                        }
                    };

                    Action onLaunch = () =>
                    {
                        var launchViews = RuntimeContext.DisplayManager.GetAllDisplayViews().Where(view =>
                            view.HubAllocationInfo.ViewType == HubDisplayViewType.QuickAccessAppIconView).ToList();

                        var mainViews = views.Where(view =>
                        (view.DataContext as HubViewModel).HubAllocationInfo.ViewType == HubDisplayViewType.PresentationView ||
                        (view.DataContext as HubViewModel).HubAllocationInfo.ViewType == HubDisplayViewType.PresentationRibbonView).ToList();

                        mainViews.ForEach(view => AllocateView(view));
                    };

                    var launchButtonView = new LaunchButtonView(onLaunch)
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
                    views.Add(canvasView);
                    views.Add(canvasControlsView);
                }

                var launchButtonViews = views.Where(view =>
                (view.DataContext as HubViewModel).HubAllocationInfo.ViewType == HubDisplayViewType.QuickAccessAppIconView).ToList();

                launchButtonViews.ForEach(view => AllocateView(view));
            });

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

        public void AllocateView(FrameworkElement view)
        {
            HubViewModel hubViewModel = null;

            CurrentUiDispatcher.Invoke(() =>
            {
                hubViewModel = (HubViewModel)view.DataContext;
            });

            RuntimeContext.DisplayManager.AllocateUiInHubDisplayAsync(
                CreateContract(view),
                hubViewModel.HubAllocationInfo,
                hubViewModel.AllocatedCallBack
                );
        }

        public void DeallocateView(DisplayView view)
        {
            RuntimeContext.DisplayManager.DeallocateUiFromHubDisplayAsync(
                view,
                (_) => { }
                );
        }
    }
}
