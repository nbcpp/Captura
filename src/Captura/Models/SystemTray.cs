﻿using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows.Controls.Primitives;

namespace Captura.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class SystemTray : ISystemTray
    {
        bool _first = true;

        /// <summary>
        /// Using a Function ensures that the <see cref="TaskbarIcon"/> object is used only after it is initialised.
        /// </summary>
        readonly Func<TaskbarIcon> _trayIcon;
        readonly Settings _settings;

        readonly NotificationStack _notificationStack = new NotificationStack();

        public SystemTray(Func<TaskbarIcon> TaskbarIcon, Settings Settings)
        {
            _trayIcon = TaskbarIcon;
            _settings = Settings;

            _notificationStack.Opacity = 0;
        }

        public void HideNotification()
        {
            _notificationStack.Hide();
        }

        void Show()
        {
            var trayIcon = _trayIcon.Invoke();

            if (trayIcon != null && _first)
            {
                trayIcon.ShowCustomBalloon(_notificationStack, PopupAnimation.None, null);

                _first = false;
            }

            _notificationStack.Show();
        }

        public void ShowScreenShotNotification(string FilePath)
        {
            if (!_settings.UI.TrayNotify)
                return;

            _notificationStack.Add(new ScreenShotBalloon(FilePath));

            Show();
        }

        public INotification ShowNotification(bool Progress)
        {
            var vm = new NotificationViewModel();

            if (!Progress)
                vm.Finished = true;

            if (!_settings.UI.TrayNotify)
                return vm;

            _notificationStack.Add(new NotificationBalloon(vm));

            Show();

            return vm;
        }
    }
}
