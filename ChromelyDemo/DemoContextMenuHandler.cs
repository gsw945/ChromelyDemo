using Chromely.Browser;
using Chromely.Core.Configuration;
using System;
using Xilium.CefGlue;

namespace ChromelyDemo
{
    public class DemoContextMenuHandler : DefaultContextMenuHandler
    {
        private readonly IChromelyConfiguration _config;

        public DemoContextMenuHandler(IChromelyConfiguration config) : base(config)
        {
            _config = config;
        }

        protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
        {// To disable the menu then call clear
            model.Clear();

            // Removing existing menu item
            // Remove "View Source" option
            // model.Remove((int)CefMenuId.ViewSource);
            model.AddItem((int)CefMenuId.Forward, "前进");
            model.AddItem((int)CefMenuId.Reload, "重新载入");
            model.AddItem((int)CefMenuId.Back, "后退");

            if (_config.DebuggingMode)
            {
                model.AddSeparator();
                model.AddItem((int)(CefMenuId)ShowDevTools, "Show DevTools");
                model.AddItem((int)(CefMenuId)CloseDevTools, "Close DevTools");
            }
        }


        protected override bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
        {
            return false;
        }

        protected override bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags)
        {
            if (_config.DebuggingMode)
            {
                if (commandId == ShowDevTools)
                {
                    var host = browser.GetHost();
                    var wi = CefWindowInfo.Create();
                    wi.SetAsPopup(IntPtr.Zero, "DevTools");
                    host.ShowDevTools(wi, new DevToolsWebClient(), new CefBrowserSettings(), new CefPoint(0, 0));
                }
                else if (commandId == CloseDevTools)
                {
                    browser.GetHost().CloseDevTools();
                }
            }
            return false;
        }

        protected override void OnContextMenuDismissed(CefBrowser browser, CefFrame frame)
        {
        }

        private class DevToolsWebClient : CefClient
        {
        }
    }
}
