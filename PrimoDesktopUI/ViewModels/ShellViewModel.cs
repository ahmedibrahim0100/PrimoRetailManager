﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PrimoDesktopUI.EventModels;
using PrimoDesktopUI.Library.Models;

namespace PrimoDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        private IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user)
        {
            _salesVM = salesVM;
            _events = events;
            _user = user;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());

        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryClose();
        } 
    }
}
