﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DotNetRu.Clients.Portable.Extensions;
using DotNetRu.DataStore.Audit.Models;
using DotNetRu.Utils.Helpers;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;

namespace DotNetRu.Clients.Portable.ViewModel
{
    public class NotificationsViewModel : ViewModelBase
	{
		public NotificationsViewModel() : base()
		{

		}


		public ObservableRangeCollection<Notification> Notifications { get; } = new ObservableRangeCollection<Notification>();
		public ObservableRangeCollection<Grouping<string, Notification>> NotificationsGrouped { get; } = new ObservableRangeCollection<Grouping<string, Notification>>();



		void SortNotifications()
		{

			var groups = from notification in this.Notifications
						 orderby notification.Date descending
						 group notification by notification.Date.GetSortName()
				into notificationGroup
						 select new Grouping<string, Notification>(notificationGroup.Key, notificationGroup);

		    this.NotificationsGrouped.ReplaceRange(groups);
		}

		ICommand forceRefreshCommand;
		public ICommand ForceRefreshCommand => this.forceRefreshCommand ?? (this.forceRefreshCommand = new Command(async () => await this.ExecuteForceRefreshCommandAsync()));

		async Task ExecuteForceRefreshCommandAsync()
		{
			await this.ExecuteLoadNotificationsAsync(true);
		}

		ICommand loadNotificationsCommand;
		public ICommand LoadNotificationsCommand => this.loadNotificationsCommand ?? (this.loadNotificationsCommand = new Command<bool>(async (f) => await this.ExecuteLoadNotificationsAsync()));

		async Task<bool> ExecuteLoadNotificationsAsync(bool force = false)
		{
			if (this.IsBusy)
				return false;

			try
			{
			    this.IsBusy = true;

#if DEBUG
				await Task.Delay(1000);
#endif
			    this.Notifications.ReplaceRange(await this.StoreManager.NotificationStore.GetItemsAsync(force));

			    this.SortNotifications();

			}
			catch (Exception ex)
			{
			    this.Logger.Report(ex, "Method", "ExecuteLoadNotificationsAsync");
				MessagingService.Current.SendMessage(MessageKeys.Error, ex);
			}
			finally
			{
			    this.IsBusy = false;
			}

			return true;
		}
	}
}

