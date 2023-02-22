﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogManager
{
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string cancel = "OK");
        Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");
        Task<string> DisplayPromptAsync(string title, string message, string send);
        string DisplayPrompt(string title, string message, string send);
        void ShowAlert(string title, string message, string cancel = "OK");
        void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No");
    }

    internal class AlertService : IAlertService
    {
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
        public void ShowAlert(string title, string message, string cancel = "OK")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () => await ShowAlertAsync(title, message, cancel));
        }
        public void ShowConfirmation(string title, string message, Action<bool> callback, string accept = "Yes", string cancel = "No")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(answer);
            });
        }
        public Task<string> DisplayPromptAsync(string title, string message, string send)
        {
            return Application.Current.MainPage.DisplayPromptAsync(title, message, send);
        }

        public string DisplayPrompt(string title, string message, string send)
        {
            string answer=string.Empty;
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                answer = await DisplayPromptAsync(title, message, send);
            });
            return answer;
        }
    }

}